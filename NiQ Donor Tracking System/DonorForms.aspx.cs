using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class DonorForms : Page
    {
        private string _searchValue;
        public IDonorRepository DonorRepository { get; set; }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void donorGrid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _searchValue = donorGrid.Rows[donorGrid.SelectedIndex].Cells[1].Text;
                ClearResults();
                searchTextBox.Text = _searchValue;
                SearchDonor();
            }
            catch
            {
                // ignored
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearResults();
                ddrstatus.Attributes.Add("onchange", "statuschanged()");
            }

        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            ClearResults();

            try
            {
                searchTextBox.Text = string.Empty;

                var donor = DonorRepository.Get(SelectedDonorId.Value);

                donor.ReceiveConsentForm = receivedConsent.Checked;
                donor.ReceiveFinancialForm = receivedFinancial.Checked;
                DonorRepository.Update(donor);

                ResultMessage.Visible = true;
                ResultMessage.Text = "Donor updated";
            }
            catch 
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "An error occured updating the donors record.";
            }
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
            if(!string.IsNullOrEmpty(TextBox1.Text) && TextBox1.Text!="")
            {
                _searchValue = TextBox1.Text.Trim().Replace("'", "''");
            }
            else
            _searchValue = searchTextBox.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(_searchValue))
                searchTextBox.Focus();
            else
                SearchDonor();
        }

        private void ClearResults()
        {
            donorDisplay.Visible = false;
            formFields.Visible = false;
            ResultMessage.Visible = false;
            donorResults.Visible = false;
            SaveButton.Visible = false;
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
            donorResultLabel.Text = title;
            donorResults.Visible = true;
            dataHandler(gridView, data);
        }

        private void SearchDonor()
        {

                
            Donor donor = DonorRepository.GetWithAddresses(_searchValue);

            if (donor == null)
                SearchDonors();
            else
                SetSelectedDonor(donor);
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
            if (donors == null || !donors.Any())
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "No Records Found";

                return;
            }

            if (donors.Count == 1)
            {
                SetSelectedDonor(donors[0]);

                return;
            }

            FillGrid(donorGrid, "Donors", DonorBuilder, donors);
        }

        private void SetSelectedDonor(Donor donor)
        {
            SelectedDonorId.Value = donor.DonorId;
            DonorIdValue.Text = donor.DonorId;
            DonorNameValue.Text = $"{donor.LastName}, {donor.FirstName}";
            DonorDobValue.Text = donor.DateOfBirth.ToString("d");
            DonorEmailValue.Text = donor.Email;
            DonorMailing1Value.Text = donor.MailingAddress?.Address1 ?? string.Empty;
            DonorMailing2Value.Text = donor.MailingAddress?.Address2 ?? string.Empty;
            DonorMailing3Value.Text = donor.MailingAddress != null
                ? $"{donor.MailingAddress.City} {donor.MailingAddress.State}, {donor.MailingAddress.Zipcode}"
                : string.Empty;
            DonorShipping1Value.Text = donor.ShippingAddress?.Address1 ?? string.Empty;
            DonorShipping2Value.Text = donor.ShippingAddress?.Address2 ?? string.Empty;
            DonorShipping3Value.Text = donor.ShippingAddress != null
                ? $"{donor.ShippingAddress.City} {donor.ShippingAddress.State}, {donor.ShippingAddress.Zipcode}"
                : string.Empty;

            receivedConsent.Checked = donor.ReceiveConsentForm;
            receivedFinancial.Checked = donor.ReceiveFinancialForm;

            donorDisplay.Visible = true;
            formFields.Visible = true;
            SaveButton.Visible = true;
        }
    }
}