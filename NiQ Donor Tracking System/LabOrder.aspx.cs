using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Web.UI;
using System.Web.UI.WebControls;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class LabOrder : Page
    {
        private int _donorLabels;
        private int _milkLabels;
        private List<Printer> _printers;
        private string _searchValue;
        public IDonorRepository DonorRepository { get; set; }
        public ILabKitRepository LabKitRepository { get; set; }
        public IMilkKitRepository MilkKitRepository { get; set; }
        public IPrinterRepository PrinterRepository { get; set; }

        protected void Cancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void ClearSearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) ClearResults();

            string g = Request.QueryString["milkkit"];

            if (!string.IsNullOrEmpty(g))
            {
                _searchValue = g;
                SearchMilkKit();
            }
        }

        protected void Print_OnClick(object sender, EventArgs e)
        {
            var counts = PrintCount.Value.Split(',');
            string printerIp = Printers.SelectedValue;

            string labelRequest = LabelHelper.GetMilkKitLabel(SelectedMilkKit.Value, counts[0]);

            /*
            if (counts[1] != "0")
            {
                labelRequest += Environment.NewLine;
                labelRequest += LabelHelper.GetDonorLabel(SelectedDonorId.Value, counts[1]);
            } */

            TcpClient tcpSocket = new TcpClient();

            try
            {
                // Connect to the printer
                tcpSocket.Connect(printerIp, Global.PrinterInfo.PORT);
                NetworkStream nsPrint = tcpSocket.GetStream();
                StreamWriter swPrint = new StreamWriter(nsPrint);

                // Start sending
                swPrint.WriteLine(labelRequest);

                swPrint.Flush();
                swPrint.Close();
                nsPrint.Close();
                tcpSocket.Close();
            }
            catch (Exception ex)
            {
                ResultMessage.Text = "Label Print Error: " + ex.Message;

                return;
            }

            ClearResults();
            ResultMessage.Visible = true;
            ResultMessage.Text = "Label printed successfully.";
        }

        protected void SaveOrder_Clicked(object sender, EventArgs e)
        {
            try
            {
                LabKit labKit = LabKitRepository.Get(SelectedLabKit.Value);

                if (ToxicologyOrder.Enabled && ToxicologyOrder.Checked)
                {
                    labKit.ToxicologyOrdered = DateTime.Now;
                    _milkLabels++;
                }

                if (MicrobialOrder.Enabled && MicrobialOrder.Checked)
                {
                    labKit.MicrobialOrdered = DateTime.Now;
                    _milkLabels++;
                }

                if (GeneticsOrder.Enabled && GeneticsOrder.Checked)
                {
                    labKit.GeneticOrdered = DateTime.Now;
                    _milkLabels++;
                    _donorLabels = 2;
                }

                if (_milkLabels == 0 && _donorLabels == 0)
                {
                    ResultMessage.Visible = true;
                    ResultMessage.Text = "No tests ordered";

                    return;
                }
                else
                {
                    MilkKit milkKit1 = MilkKitRepository.Get(SelectedMilkKit.Value);
                    string toemail, body, Subject;
                    Subject = "Lab Ordered";
                    toemail = DonorRepository.Get(milkKit1.DonorId).Email;
                    body = "Ni-Q has started the testing process on your milk kit. This process may take up to 7-10 business days to complete. Ni-Q will notify once the milk kit is finalized in testing. You will be notified if your milk kit passes or fails.";
                    if(!string.IsNullOrEmpty(toemail) && toemail!="")
                    EMailHelper.SendEmail(toemail, body, Subject);
                }

                _milkLabels = 1;

                LabKitRepository.Update(labKit);
                MilkKit milkKit = MilkKitRepository.Get(SelectedMilkKit.Value);
                SetSelectedMilkKit(milkKit, labKit);
                LabOrderControls.Visible = false;
                LoadPrinters();
                PrintControls.Visible = true;
                PrintCount.Value = $"{_milkLabels},{_donorLabels}";
                ResultMessage.Visible = true;
                string resultMessage = $"Select printer to print {_milkLabels} Milk Kit Label(s)";

                if (_donorLabels != 0) resultMessage += $" and {_donorLabels} Donor Labels.";

                ResultMessage.Text = resultMessage;
            }
            catch //(Exception exception)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "An error occured ordering lab kit.";
            }
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            _searchValue = SearchTextBox.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(_searchValue))
                SearchTextBox.Focus();
            else
                SearchMilkKit();
        }

        private void ClearResults()
        {
            LabOrderControls.Visible = false;
            PrintControls.Visible = false;
            MilkKitDisplay.Visible = false;
            SearchTextBox.Enabled = true;
            SearchBtn.Visible = true;
            ClearSearchBtn.Visible = false;
            MicrobialOrder.Enabled = true;
            MicrobialOrder.Checked = false;
            ToxicologyOrder.Enabled = true;
            ToxicologyOrder.Checked = false;
            GeneticsOrder.Enabled = true;
            GeneticsOrder.Checked = false;
            ResultMessage.Visible = false;
            _milkLabels = 0;
            _donorLabels = 0;
            SearchTextBox.Text = string.Empty;
            SelectedLabKit.Value = string.Empty;
            SelectedDonorId.Value = string.Empty;
            SelectedMilkKit.Value = string.Empty;
            SearchTextBox.Focus();
        }

        private void LoadPrinters()
        {
            _printers = PrinterRepository.Get();
            Printers.Items.Clear();
            _printers.ForEach(p => Printers.Items.Add(new ListItem(p.PrinterName, p.PrinterIp)));
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

            else if(milkKit.Volume == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "This milkkit has no volume entered.";
            }

            else if (milkKit.QuarantineDate == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "This milkkit is not quarantined";
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
            if (labKit.MicrobialOrdered.HasValue && labKit.ToxicologyOrdered.HasValue && labKit.GeneticOrdered.HasValue)
            {
                SearchTextBox.Text = string.Empty;
                ResultMessage.Visible = true;
                ResultMessage.Text = "All tests ordered for entered Milk Kit";

                return;
            }

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
            LabOrderControls.Visible = true;

            if (labKit.MicrobialOrdered.HasValue)
            {
                MicrobialOrder.Checked = true;
                MicrobialOrder.Enabled = false;
            }

            if (labKit.ToxicologyOrdered.HasValue)
            {
                ToxicologyOrder.Checked = true;
                ToxicologyOrder.Enabled = false;
            }

            if (labKit.GeneticOrdered.HasValue)
            {
                GeneticsOrder.Checked = true;
                GeneticsOrder.Enabled = false;
            }
        }
    }
}