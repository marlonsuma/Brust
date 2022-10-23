using System;
using System.Collections.Generic;
using System.Web.UI;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class ManageMilkKit : Page
    {
        private string _searchValue;
        private Global.UserInfo _userInfo;
        public IDonorRepository DonorRepository { get; set; }
        public ILabKitRepository LabKitRepository { get; set; }
        public ITransactionLog Logger { get; set; }
        public ILotRepository LotRepository { get; set; }
        public IMilkKitRepository MilkKitRepository { get; set; }

        protected void Cancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }

        protected void ClearSearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userInfo = (Global.UserInfo)Session["ui"];
            if (_userInfo.Administrator == false) Response.Redirect("Default.aspx");
            if (!IsPostBack) ClearResults();
        }

        protected void QuarantineCalendar_SelectionChanged(object sender, EventArgs e)
        {
            NewQuarantineDate.Text = QuarantineCalendar.SelectedDate.ToShortDateString();
            QuarantineCalendar.Visible = false;
        }

        protected void QuarantineCalendarImage_Click(object sender, ImageClickEventArgs e)
        {
            QuarantineCalendar.Visible = !QuarantineCalendar.Visible;
        }

        protected void PaidDateCalendar_SelectionChanged(object sender, EventArgs e)
        {
            NewPaidDate.Text = PaidDateCalender.SelectedDate.ToShortDateString();
            PaidDateCalender.Visible = false;
        }

        protected void FinalizedDateCalendarImage_Click(object sender, ImageClickEventArgs e)
        {
            FinalizedDateCalender.Visible = !FinalizedDateCalender.Visible;
        }

        protected void FinalizedDateCalendar_SelectionChanged(object sender, EventArgs e)
        {
            newFinalizedDate.Text = FinalizedDateCalender.SelectedDate.ToShortDateString();
            FinalizedDateCalender.Visible = false;
        }

        protected void PaidDateCalendarImage_Click(object sender, ImageClickEventArgs e)
        {
            PaidDateCalender.Visible = !PaidDateCalender.Visible;
        }


        protected void ReceiveCalendar_SelectionChanged(object sender, EventArgs e)
        {
            NewReceiveDate.Text = ReceiveCalendar.SelectedDate.ToShortDateString();
            ReceiveCalendar.Visible = false;
        }

        protected void ReceiveCalendarImage_Click(object sender, ImageClickEventArgs e)
        {
            ReceiveCalendar.Visible = !ReceiveCalendar.Visible;
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            _searchValue = SearchTextBox.Text.Trim().Replace("'", "''");
            ClearResults();
            if (string.IsNullOrEmpty(_searchValue))
                SearchTextBox.Focus();
            else
                SearchMilkKit();
        }

        protected void Update_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!UpdateMilkKit()) return;
                UpdateLabKit();
                ClearResults();
                ResultMessage.Visible = true;
                ResultMessage.Text = "Milk Kit Updated";
            }
            catch (Exception exception)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "An Error occured updating this milk kit.";
            }

        }

        private void ClearResults()
        {
            ManageMilkKitControls.Visible = false;
            MilkKitDisplay.Visible = false;

            Update.Visible = false;

            SearchTextBox.Text = string.Empty;
            SearchTextBox.Enabled = true;
            SearchBtn.Visible = true;
            ClearSearchBtn.Visible = false;
            ResultMessage.Visible = false;

            ReceiveCalendar.Visible = false;
            QuarantineCalendar.Visible = false;
            PaidDateCalender.Visible = false;
            FinalizedDateCalender.Visible = false;

            SelectedLabKit.Value = string.Empty;
            SelectedDonorId.Value = string.Empty;
            SelectedMilkKit.Value = string.Empty;
            SearchTextBox.Focus();
        }

        private void SearchMilkKit()
        {
            MilkKit milkKit = MilkKitRepository.GetWithLot(_searchValue);

            if (milkKit == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            LabKit labKit = LabKitRepository.GetByMilkKit(milkKit.Id) ?? LabKitRepository.Add(new LabKit
                {
                    DonorId = milkKit.DonorId,
                    MilkKitId = milkKit.Id
                });

            SetSelectedMilkKit(milkKit, labKit);
        }

        private void SetSelectedMilkKit(MilkKit milkKit, LabKit labKit)
        {
            Donor donor = DonorRepository.Get(milkKit.DonorId);
            Barcode.Text = milkKit.Barcode;
            DonorId.Text = milkKit.DonorId;
            DonorName.Text = donor != null ? $"{donor.LastName}, {donor.FirstName}" : string.Empty;
            Volume.Text = milkKit.Volume;
            ReceiveDate.Text = milkKit.ReceiveDate?.ToString("d") ?? string.Empty;
            QuarantineDate.Text = milkKit.QuarantineDate?.ToString("d") ?? string.Empty;
           
            MicrobialStatus.Text =
                labKit.MicrobialOrdered.HasValue
                    ? milkKit.MicrobialTest.HasValue ? milkKit.MicrobialTest.Value ? "Passed" : "Failed" :
                    $"Ordered {labKit.MicrobialOrdered.Value:d}"
                    : "Not Ordered";
            ToxicologyStatus.Text = labKit.ToxicologyOrdered.HasValue
                ? milkKit.DrugAlcoholTest.HasValue ? milkKit.DrugAlcoholTest.Value ? "Passed" : "Failed" :
                $"Ordered {labKit.ToxicologyOrdered.Value:d}"
                : "Not Ordered";
            GeneticsStatus.Text = labKit.GeneticOrdered.HasValue
                ? milkKit.Dnatest.HasValue ? milkKit.Dnatest.Value ? "Passed" : "Failed" :
                $"Ordered {labKit.GeneticOrdered.Value:d}"
                : "Not Ordered";

            SelectedLabKit.Value = labKit.Barcode;
            SelectedMilkKit.Value = milkKit.Barcode;
            SelectedDonorId.Value = labKit.DonorId;
            SearchTextBox.Text = milkKit.Barcode;
            SearchTextBox.Enabled = false;
            SearchBtn.Visible = false;
            ClearSearchBtn.Visible = true;
            MilkKitDisplay.Visible = true;

            ManageMilkKitControls.Visible = true;
            Active.Checked = milkKit.Active;
            NewVolume.Text = milkKit.Volume;
            NewDonorId.Text = milkKit.DonorId;
            ReceiveCalendar.SelectedDate = milkKit.ReceiveDate ?? DateTime.Now;
            NewReceiveDate.Text = milkKit.ReceiveDate?.ToShortDateString() ?? string.Empty;
            ShippingService.SelectedValue = milkKit.ShippingService;
            TrackingNumber.Text = milkKit.TrackingNumber;
            QuarantineCalendar.SelectedDate = milkKit.QuarantineDate ?? DateTime.Now;
            NewQuarantineDate.Text = milkKit.QuarantineDate?.ToShortDateString() ?? string.Empty;
            PaidDateCalender.SelectedDate = milkKit.DatePaid ?? DateTime.Now;
            NewPaidDate.Text = milkKit.DatePaid?.ToShortDateString() ?? string.Empty;
            FinalizedDateCalender.SelectedDate = milkKit.Finalized ?? DateTime.Now;
            newFinalizedDate.Text = milkKit.Finalized?.ToShortDateString() ?? string.Empty;
            MicrobialOrdered.Checked = labKit.MicrobialOrdered.HasValue;
            MicrobialOrderDate.Text = labKit.MicrobialOrdered.HasValue
                ? labKit.MicrobialOrdered.Value.ToShortDateString()
                : string.Empty;

            if (milkKit.MicrobialTest.HasValue)
            {
                if (milkKit.MicrobialTest.Value)
                {
                    MicrobialPass.Checked = true;
                    MicrobialFail.Checked = false;
                }
                else
                {
                    MicrobialPass.Checked = false;
                    MicrobialFail.Checked = true;
                }
            }

            ToxicologyOrdered.Checked = labKit.ToxicologyOrdered.HasValue;
            ToxicologyOrderDate.Text = labKit.ToxicologyOrdered.HasValue
                ? labKit.ToxicologyOrdered.Value.ToShortDateString()
                : string.Empty;

            if (milkKit.DrugAlcoholTest.HasValue)
            {
                if (milkKit.DrugAlcoholTest.Value)
                {
                    ToxicologyPass.Checked = true;
                    ToxicologyFail.Checked = false;
                }
                else
                {
                    ToxicologyPass.Checked = false;
                    ToxicologyFail.Checked = true;
                }

            }

            GeneticsOrdered.Checked = labKit.GeneticOrdered.HasValue;
            GeneticsOrderDate.Text = labKit.GeneticOrdered.HasValue
                ? labKit.GeneticOrdered.Value.ToShortDateString()
                : string.Empty;

            if (milkKit.Dnatest.HasValue)
            {
                if (milkKit.Dnatest.Value)
                {
                    GeneticsPass.Checked = true;
                    GeneticsFail.Checked = false;
                }
                else
                {
                    GeneticsPass.Checked = false;
                    GeneticsFail.Checked = true;
                }
            }

            Lot.Text = milkKit.Lot?.Barcode ?? string.Empty;
            Update.Visible = true;
        }

        private void UpdateLabKit()
        {
            LabKit labKit = LabKitRepository.Get(SelectedLabKit.Value);
            Dictionary<string, string> changed = new Dictionary<string, string>();
            bool updated = false;

            if (!MicrobialOrdered.Checked && labKit.MicrobialOrdered.HasValue)
            {
                changed.Add("Previous Microbial Order Date", labKit.MicrobialOrdered.Value.ToShortDateString());
                changed.Add("New Microbial order date", "None");
                labKit.MicrobialOrdered = null;
                updated = true;
            }

            if (!ToxicologyOrdered.Checked && labKit.ToxicologyOrdered.HasValue)
            {
                changed.Add("Previous Toxicology Order Date", labKit.ToxicologyOrdered.Value.ToShortDateString());
                changed.Add("New Toxicology order date", "None");
                labKit.ToxicologyOrdered = null;
                updated = true;
            }

            if (!GeneticsOrdered.Checked && labKit.GeneticOrdered.HasValue)
            {
                changed.Add("Previous Genetics Order Date", labKit.GeneticOrdered.Value.ToShortDateString());
                changed.Add("New Genetics order date", "None");
                labKit.GeneticOrdered = null;
                updated = true;
            }

            if (MicrobialOrdered.Checked && (labKit.MicrobialOrdered.HasValue &&
                labKit.MicrobialOrdered.Value.ToShortDateString() != MicrobialOrderDate.Text ||
                !labKit.MicrobialOrdered.HasValue && !string.IsNullOrEmpty(MicrobialOrderDate.Text)))
            {
                changed.Add("Previous Microbial Order Date", labKit.MicrobialOrdered?.ToShortDateString() ?? "None");
                changed.Add("New Microbial order date", MicrobialOrderDate.Text);
                labKit.MicrobialOrdered = DateTime.Parse(MicrobialOrderDate.Text);
                updated = true;
            }

            if (ToxicologyOrdered.Checked && (labKit.ToxicologyOrdered.HasValue &&
                labKit.ToxicologyOrdered.Value.ToShortDateString() != ToxicologyOrderDate.Text ||
                !labKit.ToxicologyOrdered.HasValue && !string.IsNullOrEmpty(ToxicologyOrderDate.Text)))
            {
                changed.Add("Previous Toxicology Order Date", labKit.ToxicologyOrdered?.ToShortDateString() ?? "None");
                changed.Add("New Toxicology order date", ToxicologyOrderDate.Text);
                labKit.ToxicologyOrdered = DateTime.Parse(ToxicologyOrderDate.Text);
                updated = true;
            }

            if (GeneticsOrdered.Checked && (labKit.GeneticOrdered.HasValue &&
                labKit.GeneticOrdered.Value.ToShortDateString() != GeneticsOrderDate.Text ||
                !labKit.GeneticOrdered.HasValue && !string.IsNullOrEmpty(GeneticsOrderDate.Text)))
            {
                changed.Add("Previous Genetics Order Date", labKit.GeneticOrdered?.ToShortDateString() ?? "None");
                changed.Add("New Genetics order date", GeneticsOrderDate.Text);
                labKit.GeneticOrdered = DateTime.Parse(GeneticsOrderDate.Text);
                updated = true;
            }

            if (!updated) return;
            LabKitRepository.Update(labKit);
            Logger.Add(TransactionType.ChangeLabKit, _userInfo.ID.ToString(), ItemType.LabKit, labKit.Id, changed);
        }

        private bool UpdateMilkKit()
        {
            MilkKit milkKit = MilkKitRepository.GetWithLot(SelectedMilkKit.Value);
            Dictionary<string, string> changed = new Dictionary<string, string>();
            bool updated = false;

            if (Active.Checked != milkKit.Active)
            {
                changed.Add("Previous Active Status", milkKit.Active.ToString());
                changed.Add("New Active Status", Active.Checked.ToString());
                milkKit.Active = Active.Checked;
                updated = true;
            }

            if (NewVolume.Text != milkKit.Volume)
            {
                changed.Add("Previous Volume", milkKit.Volume);
                changed.Add("New Volume", NewVolume.Text);
                milkKit.Volume = NewVolume.Text;
                updated = true;
            }

            if (NewDonorId.Text != milkKit.DonorId)
            {
                var donor = DonorRepository.Get(NewDonorId.Text);

                if (donor == null)
                {
                    ResultMessage.Visible = true;
                    ResultMessage.Text = "New Donor not found.";

                    return false;
                }

                changed.Add("Previous Donor ID", milkKit.DonorId);
                changed.Add("New Donor ID", NewDonorId.Text);
                milkKit.DonorId = NewDonorId.Text;
                updated = true;
            }

            if (!string.IsNullOrEmpty(NewReceiveDate.Text) && !milkKit.ReceiveDate.HasValue ||
                milkKit.ReceiveDate.HasValue && milkKit.ReceiveDate.Value.ToShortDateString() != NewReceiveDate.Text)
            {
                changed.Add("Previous Receive Date",
                    milkKit.ReceiveDate.HasValue ? milkKit.ReceiveDate.Value.ToShortDateString() : "No Date");
                changed.Add("New Receive Date",
                    string.IsNullOrEmpty(NewReceiveDate.Text) ? "No Date" : NewReceiveDate.Text);
                milkKit.ReceiveDate = string.IsNullOrEmpty(NewReceiveDate.Text)
                    ? (DateTime?) null
                    : DateTime.Parse(NewReceiveDate.Text);
                updated = true;
            }

            if (ShippingService.SelectedValue != milkKit.ShippingService)
            {
                changed.Add("Previous ShippingService", milkKit.ShippingService);
                changed.Add("New ShippingService", ShippingService.SelectedValue);
                milkKit.ShippingService = ShippingService.SelectedValue;
                updated = true;
            }

            if (TrackingNumber.Text != milkKit.TrackingNumber)
            {
                changed.Add("Previous TrackingNumber", milkKit.TrackingNumber);
                changed.Add("New TrackingNumber", TrackingNumber.Text);
                milkKit.TrackingNumber = TrackingNumber.Text;
                updated = true;
            }

            if (!string.IsNullOrEmpty(NewQuarantineDate.Text) && !milkKit.QuarantineDate.HasValue ||
                milkKit.QuarantineDate.HasValue &&
                milkKit.QuarantineDate.Value.ToShortDateString() != NewQuarantineDate.Text)
            {
                changed.Add("Previous quarantine Date",
                    milkKit.QuarantineDate.HasValue ? milkKit.QuarantineDate.Value.ToShortDateString() : "No Date");
                changed.Add("New quarantine Date",
                    string.IsNullOrEmpty(NewQuarantineDate.Text) ? "No Date" : NewQuarantineDate.Text);
                milkKit.QuarantineDate = string.IsNullOrEmpty(NewQuarantineDate.Text)
                    ? (DateTime?) null
                    : DateTime.Parse(NewQuarantineDate.Text);
                updated = true;
            }

            if (!string.IsNullOrEmpty(NewPaidDate.Text) && !milkKit.DatePaid.HasValue ||
               milkKit.DatePaid.HasValue &&
               milkKit.DatePaid.Value.ToShortDateString() != NewPaidDate.Text)
            {
                changed.Add("Previous Paid Date",
                    milkKit.DatePaid.HasValue ? milkKit.DatePaid.Value.ToShortDateString() : "No Date");
                changed.Add("New Paid Date",
                    string.IsNullOrEmpty(NewPaidDate.Text) ? "No Date" : NewPaidDate.Text);
                milkKit.DatePaid = string.IsNullOrEmpty(NewPaidDate.Text)
                    ? (DateTime?)null
                    : DateTime.Parse(NewPaidDate.Text);
                updated = true;
            }

            if (!string.IsNullOrEmpty(newFinalizedDate.Text) && !milkKit.Finalized.HasValue ||
               milkKit.Finalized.HasValue &&
               milkKit.Finalized.Value.ToShortDateString() != newFinalizedDate.Text)
            {
                changed.Add("Previous Finalized Date",
                    milkKit.Finalized.HasValue ? milkKit.Finalized.Value.ToShortDateString() : "No Date");
                changed.Add("New Finalized Date",
                    string.IsNullOrEmpty(newFinalizedDate.Text) ? "No Date" : newFinalizedDate.Text);
                milkKit.Finalized = string.IsNullOrEmpty(newFinalizedDate.Text)
                    ? (DateTime?)null
                    : DateTime.Parse(newFinalizedDate.Text);
                updated = true;
            }

            if (!string.IsNullOrEmpty(Lot.Text) && !milkKit.LotId.HasValue ||
                string.IsNullOrEmpty(Lot.Text) && milkKit.LotId.HasValue ||
                !string.IsNullOrEmpty(Lot.Text) && milkKit.Lot != null && Lot.Text != milkKit.Lot.Barcode)
            {
                Lot newLot = LotRepository.Get(Lot.Text);
                if (newLot == null)
                {
                    ResultMessage.Visible = true;
                    ResultMessage.Text = "New lot not found.";

                    return false;
                }

                changed.Add("Previous Lot", milkKit.Lot?.Barcode ?? "None");
                changed.Add("New Lot", string.IsNullOrEmpty(Lot.Text) ? "None" : Lot.Text);
                milkKit.LotId = newLot.Id;
                updated = true;
            }

            bool? microbialResult =
                MicrobialPass.Checked || MicrobialFail.Checked ? MicrobialPass.Checked : (bool?) null;

            if (milkKit.MicrobialTest != microbialResult)
            {
                changed.Add("Previous Microbial Result", milkKit.MicrobialTest?.ToString() ?? "None");
                changed.Add("New Microbial Result", microbialResult.Value.ToString() ?? "None");
                milkKit.MicrobialTest = microbialResult;
                updated = true;
            }

            bool? toxicologyResult =
                ToxicologyPass.Checked || ToxicologyFail.Checked ? ToxicologyPass.Checked : (bool?) null;

            if (milkKit.DrugAlcoholTest != toxicologyResult)
            {
                changed.Add("Previous Toxicology Result", milkKit.DrugAlcoholTest?.ToString() ?? "None");
                changed.Add("New Toxicology Result", toxicologyResult.Value.ToString() ?? "None");
                milkKit.DrugAlcoholTest = toxicologyResult;
                updated = true;
            }

            bool? geneticResult = GeneticsPass.Checked || GeneticsFail.Checked ? GeneticsPass.Checked : (bool?) null;

            if (milkKit.Dnatest != geneticResult)
            {
                changed.Add("Previous Genetics Result", milkKit.Dnatest?.ToString() ?? "None");
                changed.Add("New Genetics Result", geneticResult.Value.ToString() ?? "None");
                milkKit.Dnatest = geneticResult;
                updated = true;
            }

            if (!updated) return true;
            MilkKitRepository.Update(milkKit);
            Logger.Add(TransactionType.ChangeMilkKit, _userInfo.ID.ToString(), ItemType.MilkKit, milkKit.Id, changed);

            return true;
        }
    }
}