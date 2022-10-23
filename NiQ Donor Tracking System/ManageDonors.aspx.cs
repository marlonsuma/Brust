using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class ManageDonors : System.Web.UI.Page
    {
        private string _searchValue;
        public IDonorRepository DonorRepository { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            if (ui.Administrator == false)
            {
                Response.Redirect("Default.aspx");
            }
            ClearResults();
        }

        protected void DonorGrid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _searchValue = DonorGrid.Rows[DonorGrid.SelectedIndex].Cells[1].Text;
                ClearResults();
                SearchTextBox.Text = _searchValue;
                SearchDonor();
            }
            catch
            {
                // ignored
            }
        }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Donor donor = DonorRepository.Get(SelectedDonorId.Value);
                bool donorChanged = false;
                if (Inactive.Checked && !donor.InactiveDate.HasValue)
                {
                    donor.InactiveDate = DateTime.Now.Date;
                    donor.InactiveReason = InactiveReasonText.Text;
                    donorChanged = true;
                }

                if (!Inactive.Checked && donor.InactiveDate.HasValue)
                {
                    donor.InactiveDate = null;
                    donor.InactiveReason = string.Empty;
                    donorChanged = true;
                }

                ClearResults();
                SearchTextBox.Text = string.Empty;
                SelectedDonorId.Value = string.Empty;

                if (!donorChanged) return;

                DonorRepository.Update(donor);
                ResultMessage.Visible = true;
                ResultMessage.Text = "Donor status updated.";
            }
            catch //(Exception exception)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "An error occured updating the donor record.";
            }

        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
            _searchValue = SearchTextBox.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(_searchValue))
                SearchTextBox.Focus();
            else
                SearchDonor();
        }

        protected void ClearSearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
            SelectedDonorId.Value = string.Empty;
            SearchTextBox.Text = string.Empty;
        }

        private void ClearResults()
        {
            DonorDisplay.Visible = false;
            formFields.Visible = false;
            ResultMessage.Visible = false;
            donorResults.Visible = false;
            SaveButton.Visible = false;
            SearchTextBox.Enabled = true;
            SearchBtn.Visible = true;
            ClearSearchBtn.Visible = false;
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
            List<Donor> donors = DonorRepository.FindByName(_searchValue);

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

            FillGrid(DonorGrid, "Donors", DonorBuilder, donors);
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

            Inactive.Checked = !donor.Active;
            InactiveReasonText.Text = donor.InactiveReason;

            DonorDisplay.Visible = true;
            formFields.Visible = true;
            SaveButton.Visible = true;
            SearchBtn.Visible = false;
            ClearSearchBtn.Visible = true;
            SearchTextBox.Text = donor.DonorId;
            SearchTextBox.Enabled = false;
        }
    }
}