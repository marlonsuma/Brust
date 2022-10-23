using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class Search : Page
    {
        private string _searchValue;
        public IBloodKitRepository BloodKitRepository { get; set; }
        public ICaseRepository CaseRepository { get; set; }
        public IDonorRepository DonorRepository { get; set; }
        public ILabKitRepository LabKitRepository { get; set; }
        public ILotRepository LotRepository { get; set; }
        public IMilkKitRepository MilkKitRepository { get; set; }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClearResults();
            searchTextBox.Focus();
            if(!IsPostBack)
            ddrstatus.Attributes.Add("onchange", "statuschanged()");
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();

            if(!string.IsNullOrEmpty(TextBox1.Text) && TextBox1.Text!="")
            {
                _searchValue = TextBox1.Text.Trim().Replace("'","''");
            }
            else
            _searchValue = searchTextBox.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(_searchValue) && ddrstatus.Text== "Select Status")
                searchTextBox.Focus();
            else if (_searchValue.StartsWith("MK") && _searchValue.Length == 9)
                SearchMilkKit();
            else if (_searchValue.StartsWith("DK") && _searchValue.Length == 9)
                SearchLabKit();
            else if (_searchValue.StartsWith("LT") && _searchValue.Length == 9)
                SearchLot();
            else if (_searchValue.StartsWith("CA") && _searchValue.Length == 9)
                SearchCase();
            else if (_searchValue.StartsWith("=") && _searchValue.Length == 16)
                SearchBloodKit();
            else
                SearchDonor();
        }

        private void BloodKitBuilder(GridView gridView, List<BloodKit> bloodKits)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Donor Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Barcode", typeof(string)));
            dt.Columns.Add(new DataColumn("Shipping Service", typeof(string)));
            dt.Columns.Add(new DataColumn("Tracking Number", typeof(string)));
            dt.Columns.Add(new DataColumn("Received", typeof(string)));
            dt.Columns.Add(new DataColumn("Passed", typeof(bool)));

            bloodKits.ForEach(b =>
                {
                    dr = dt.NewRow();
                    dr[0] = b.DonorId;
                    dr[1] = b.Din;
                    dr[2] = b.ShippingService;
                    dr[3] = b.TrackingNumber;
                    dr[4] = b.ReceiveDate.HasValue ? b.ReceiveDate.Value.ToString("d") : string.Empty;
                    dr[5] = b.Status ?? false;
                    dt.Rows.Add(dr);
                });

            gridView.DataSource = dt;
            gridView.DataBind();
        }

        private void CaseBuilder(GridView gridView, List<Case> cases)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Location", typeof(string)));
            dt.Columns.Add(new DataColumn("Lot", typeof(string)));
            dt.Columns.Add(new DataColumn("Ship Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Shipping Service", typeof(string)));
            dt.Columns.Add(new DataColumn("Tracking Number", typeof(string)));
            dt.Columns.Add(new DataColumn("P.O. Number", typeof(string)));
            dt.Columns.Add(new DataColumn("Case Quantity", typeof(int)));
            dt.Columns.Add(new DataColumn("Active", typeof(bool)));

            cases.ForEach(c =>
                {
                    dr = dt.NewRow();
                    dr[0] = c.Location;
                    dr[1] = c.LotBarcode;
                    dr[2] = c.ShipDate;
                    dr[3] = c.ShippingService;
                    dr[4] = c.TrackingNumber;
                    dr[5] = c.PoNumber;
                    dr[6] = c.CaseQuantity;
                    dr[7] = c.Active;
                    dt.Rows.Add(dr);
                });

            gridView.DataSource = dt;
            gridView.DataBind();
        }

        private void ClearResults()
        {
            firstResults.Visible = false;
            secondResults.Visible = false;
            thirdResults.Visible = false;
            fourthResults.Visible = false;
            ResultMessage.Visible = false;
        }

        private void DonorBuilder(GridView gridView, List<Donor> donors)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Donor Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Birth Date", typeof(string)));
            dt.Columns.Add(new DataColumn("E-Mail", typeof(string)));
            dt.Columns.Add(new DataColumn("Shipping", typeof(string)));
            dt.Columns.Add(new DataColumn("Mailing", typeof(string)));

            donors.ForEach(d =>
                {
                    dr = dt.NewRow();
                    dr[0] = d.DonorId;
                    dr[1] = $"{d.LastName}, {d.FirstName}";
                    dr[2] = d.DateOfBirth.ToString("d");
                    dr[3] = d.Email;
                    if (d.ShippingAddress != null)
                        dr[4] = $"{d.ShippingAddress.Address1} " +
                                $"{d.ShippingAddress.Address2} " +
                                $"{d.ShippingAddress.City} {d.ShippingAddress.State}, {d.ShippingAddress.Zipcode}";

                    if (d.MailingAddress != null)
                        dr[5] = $"{d.MailingAddress.Address1} " +
                                $"{d.MailingAddress.Address2} " +
                                $"{d.MailingAddress.City} {d.MailingAddress.State}, {d.MailingAddress.Zipcode}";
                    dt.Rows.Add(dr);
                });

            gridView.DataSource = dt;
            gridView.DataBind();
        }

        private void FillGrid<T>(GridView gridView, string title, Action<GridView, T> dataHandler, T data)
        {
            if (gridView == firstResultGrid)
            {
                firstResultLabel.Text = title;
                firstResults.Visible = true;
            }

            if (gridView == secondResultGrid)
            {
                secondResultLabel.Text = title;
                secondResults.Visible = true;
            }

            if (gridView == thirdResultGrid)
            {
                thirdResultLabel.Text = title;
                thirdResults.Visible = true;
            }

            if (gridView == fourthResultGrid)
            {
                fourthResultLabel.Text = title;
                fourthResults.Visible = true;
            }

            dataHandler(gridView, data);
        }

        private void LabKitBuilder(GridView gridView, List<LabKit> labKits)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Donor Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Barcode", typeof(string)));
            dt.Columns.Add(new DataColumn("Shipping Service", typeof(string)));
            dt.Columns.Add(new DataColumn("Tracking Number", typeof(string)));
            dt.Columns.Add(new DataColumn("Microbial Ordered", typeof(string)));
            dt.Columns.Add(new DataColumn("Toxicology Ordered", typeof(string)));
            dt.Columns.Add(new DataColumn("Genetics Ordered", typeof(string)));

            labKits.ForEach(l =>
                {
                    dr = dt.NewRow();
                    dr[0] = l.DonorId;
                    dr[1] = l.Barcode;
                    dr[2] = l.ShippingService;
                    dr[3] = l.TrackingNumber;
                    dr[4] = l.MicrobialOrdered.HasValue ? $"Ordered {l.MicrobialOrdered.Value:d}" : "Not Ordered";
                    dr[5] = l.ToxicologyOrdered.HasValue ? $"Ordered {l.ToxicologyOrdered.Value:d}" : "Not Ordered";
                    dr[6] = l.GeneticOrdered.HasValue? $"Ordered {l.GeneticOrdered.Value:d}" : "Not Ordered";

                    dt.Rows.Add(dr);
                });

            gridView.DataSource = dt;
            gridView.DataBind();
        }

        private void LotBuilder(GridView gridView, List<Lot> lots)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Best By Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Closed", typeof(bool)));
            dt.Columns.Add(new DataColumn("Transfered", typeof(bool)));
            dt.Columns.Add(new DataColumn("Total Case", typeof(int)));
            dt.Columns.Add(new DataColumn("Cases Remaining", typeof(int)));
            dt.Columns.Add(new DataColumn("Sample Pouches", typeof(int)));

            lots.ForEach(l =>
                {
                    dr = dt.NewRow();
                    dr[0] = l.BestByDate;
                    dr[1] = l.Closed;
                    dr[2] = l.Transferred;
                    dr[3] = l.TotalCases ?? 0;
                    dr[4] = l.CasesRemaining ?? 0;
                    dr[5] = l.SamplePouches ?? 0;
                    dt.Rows.Add(dr);
                });
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        private void MilkKitBuilder(GridView gridView, List<MilkKit> milkKits)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Donor Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Barcode", typeof(string)));
            dt.Columns.Add(new DataColumn("Lot", typeof(string)));
            dt.Columns.Add(new DataColumn("Shipping Service", typeof(string)));
            dt.Columns.Add(new DataColumn("Tracking Number", typeof(string)));
            dt.Columns.Add(new DataColumn("Received", typeof(string)));
            dt.Columns.Add(new DataColumn("Quarantined", typeof(string)));
            dt.Columns.Add(new DataColumn("Volume", typeof(string)));
            dt.Columns.Add(new DataColumn("Genetic", typeof(bool)));
            dt.Columns.Add(new DataColumn("Microbial", typeof(bool)));
            dt.Columns.Add(new DataColumn("Toxicology", typeof(bool)));
            dt.Columns.Add(new DataColumn("Paid", typeof(string)));
            dt.Columns.Add(new DataColumn("Finalized", typeof(string)));

            milkKits.ForEach(m =>
                {
                    dr = dt.NewRow();
                    dr[0] = m.DonorId;
                    dr[1] = m.Barcode;
                    dr[2] = m.Lot != null ? m.Lot.Barcode : m.LotBarcode;
                    dr[3] = m.ShippingService;
                    dr[4] = m.TrackingNumber;
                    dr[5] = m.ReceiveDate.HasValue ? m.ReceiveDate.Value.ToString("d") : string.Empty;
                    dr[6] = m.QuarantineDate.HasValue ? m.QuarantineDate.Value.ToString("d") : string.Empty;
                    dr[7] = m.Volume;
                    dr[8] = m.Dnatest ?? false;
                    dr[9] = m.MicrobialTest ?? false;
                    dr[10] = m.DrugAlcoholTest ?? false;
                    dr[11] = m.DatePaid.HasValue ? m.DatePaid.Value.ToString("d") : string.Empty;
                    dr[12] = m.Finalized.HasValue ? m.Finalized.Value.ToString("d") : string.Empty;
                    dt.Rows.Add(dr);
                });

            gridView.DataSource = dt;
            gridView.DataBind();
        }

        private void SearchBloodKit()
        {
            BloodKit bloodKit = BloodKitRepository.Get(_searchValue);

            if (bloodKit == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            FillGrid(firstResultGrid, "Blood Kit", BloodKitBuilder, new List<BloodKit> { bloodKit });
            Donor donor = DonorRepository.GetWithAddresses(bloodKit.DonorId);
            if (donor != null) FillGrid(secondResultGrid, "Donor", DonorBuilder, new List<Donor> { donor });
        }

        private void SearchCase()
        {
            Case caseResult = CaseRepository.Get(_searchValue);

            if (caseResult == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            FillGrid(firstResultGrid, "Case", CaseBuilder, new List<Case> { caseResult });
        }

        private void SearchDonor()
        {
            Donor donor = DonorRepository.GetWithKits(_searchValue);

            if (donor == null)
            {
                SearchDonors();

                return;
            }

            FillGrid(firstResultGrid, "Donor", DonorBuilder, new List<Donor> { donor });
            if (donor.MilkKits.Any())
                FillGrid(secondResultGrid,
                    $"Milk Kits - Total Ounces: {donor.MilkKits.Sum(k => decimal.TryParse(k.Volume, out decimal volume) ? volume : 0m)}",
                    MilkKitBuilder,
                    donor.MilkKits);

            if (donor.BloodKits.Any()) FillGrid(thirdResultGrid, "Blood Kits", BloodKitBuilder, donor.BloodKits);

            if (donor.LabKits.Any()) FillGrid(fourthResultGrid, "Lab Kits", LabKitBuilder, donor.LabKits);
        }

        private void SearchDonorLess()
        {
            List<MilkKit> milkKits = MilkKitRepository.GetByDonor(_searchValue);
            List<BloodKit> bloodKits = BloodKitRepository.GetByDonor(_searchValue);
            List<LabKit> labKits = LabKitRepository.GetByDonor(_searchValue);

            if (!milkKits.Any() && !bloodKits.Any() && !labKits.Any())
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            if (milkKits.Any())
                FillGrid(
                    secondResultGrid,
                    $"Milk Kits - Total Ounces: {milkKits.Sum(k => decimal.TryParse(k.Volume, out decimal volume) ? volume : 0m)}",
                    MilkKitBuilder,
                    milkKits);

            if (bloodKits.Any()) FillGrid(thirdResultGrid, "Blood Kits", BloodKitBuilder, bloodKits);

            if (labKits.Any()) FillGrid(fourthResultGrid, "Lab Kits", LabKitBuilder, labKits);
        }

        private void SearchDonors()
        {
            List<Donor> donors = new List<Donor>();
            if (ddrstatus.Text != "Select Status")
            {
                var status = ddrstatus.Text == "Active" ? true : false;
                donors = DonorRepository.GetDonorWithStatus(status);
            }
            else
            {
                donors = DonorRepository.FindByName(_searchValue);
            }

            if ((donors != null  && donors.Count==0)|| !donors.Any())
            {
                SearchDonorLess();

                return;
            }

            FillGrid(firstResultGrid, "Donors", DonorBuilder, donors);
        }

        protected void statuschanged(object sender,EventArgs e)
        {
            TextBox1.Text = "";
            searchTextBox.Text = "";

        }
        private void SearchLabKit()
        {
            LabKit labKit = LabKitRepository.Get(_searchValue);

            if (labKit == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            FillGrid(firstResultGrid, "Lab Kit", LabKitBuilder, new List<LabKit> { labKit });
            Donor donor = DonorRepository.GetWithAddresses(labKit.DonorId);
            if (donor != null) FillGrid(secondResultGrid, "Donor", DonorBuilder, new List<Donor> { donor });
        }

        private void SearchLot()
        {
            Lot lot = LotRepository.Get(_searchValue);

            if (lot == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            var milkKits = MilkKitRepository.GetByLot(lot.Id);
            var cases = CaseRepository.GetByLot(lot.Id);

            FillGrid(firstResultGrid, "Lot", LotBuilder, new List<Lot> { lot });

            if (milkKits != null && milkKits.Any())
            {
                milkKits.ForEach(m => m.Lot = lot);
                FillGrid(secondResultGrid, "Milk Kits", MilkKitBuilder, milkKits);
            }

            if (cases != null && cases.Any()) FillGrid(thirdResultGrid, "Cases", CaseBuilder, cases);
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

            FillGrid(firstResultGrid, "Milk Kit", MilkKitBuilder, new List<MilkKit> { milkKit });

            Donor donor = DonorRepository.GetWithAddresses(milkKit.DonorId);
            if (donor != null) FillGrid(secondResultGrid, "Donor", DonorBuilder, new List<Donor> { donor });
        }

        protected void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            ddrstatus.SelectedValue = "Select Status";
        }
    }
}