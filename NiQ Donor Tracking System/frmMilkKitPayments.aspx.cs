using System;
using System.Web.UI;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class frmMilkKitPayments : Page
    {
        private string _searchValue;
        public IMilkKitRepository MilkKitRepository { get; set; }
        public IDonorRepository donorRepository { get; set; }
        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void ClearSearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
            SelectedMilkKit.Value = string.Empty;
        }

        protected void imgCalendar_Click(object sender, ImageClickEventArgs e)
        {
            paymentCalendar.Visible = paymentCalendar.Visible == false;
        }

        protected void Page_Load(object sender, EventArgs e)

        {
            if (!IsPostBack) ClearResults();
        }

        protected void paymentCalendar_SelectionChanged(object sender, EventArgs e)
        {
            paymentDate.Text = paymentCalendar.SelectedDate.ToShortDateString();
            paymentCalendar.Visible = false;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            ClearResults();

            try
            {
                searchTextBox.Text = string.Empty;

                MilkKit kit = MilkKitRepository.Get(SelectedMilkKit.Value);
                kit.DatePaid = DateTime.Parse(paymentDate.Text);
               var milk= MilkKitRepository.Update(kit);
                if(milk!=null)
                {
                    string toemail,body,Subject;
                    Subject = "Milk Kit Payment";
                    toemail = donorRepository.Get(kit.DonorId).Email;
                    body = "Ni-Q has submitted you a payment. First payment through Ni-Q comes in the form of a check, and all subsequent payments will be deposited directly into the account provided. Thank you again for all that you do in helping, Ni-Q to provide the highest safety and nutritional standards in the industry. We do appreciate your donation so very much!";
                    if (!string.IsNullOrEmpty(toemail) && toemail != "")
                        EMailHelper.SendEmail(toemail,body,Subject);
                }

                ResultMessage.Visible = true;
                ResultMessage.Text = "Payment date entered.";
            }
            catch // (Exception ex)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "An error occured updating the milk kit record.";
            }
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            ClearResults();
            _searchValue = searchTextBox.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(_searchValue))
            {
                searchTextBox.Focus();

                return;
            }

            if (_searchValue.Length != 9 || _searchValue.StartsWith("MK") == false)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "Please enter a valid Milk Collection Kit ID.";
                searchTextBox.Text = "";
                searchTextBox.Focus();

                return;
            }

            FindMilkKit();
        }

        private void ClearResults()
        {
            calendarGroup.Visible = false;
            paymentCalendar.Visible = false;
            ResultMessage.Visible = false;
            SaveButton.Visible = false;
            searchTextBox.Enabled = true;
            SearchBtn.Visible = true;
            ClearSearchBtn.Visible = false;
        }

        private void FindMilkKit()
        {
            MilkKit milkKit = MilkKitRepository.Get(_searchValue);

            if (milkKit == null)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = $"Milk Collection Kit barcode {_searchValue} not found.";
                searchTextBox.Text = "";
                searchTextBox.Focus();

                return;
            }

            ManagePaymentDate(milkKit);
        }

        private void ManagePaymentDate(MilkKit milkKit)
        {
            if (milkKit.DatePaid.HasValue)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = $"Milk Collection Kit barcode {_searchValue} paid on {milkKit.DatePaid.Value:d}.";
                searchTextBox.Text = "";
                searchTextBox.Focus();

                return;
            }

            ResultMessage.Visible = true;
            ResultMessage.Text = "Select Payment Date";
            SelectedMilkKit.Value = milkKit.Barcode;
            searchTextBox.Text = milkKit.Barcode;
            searchTextBox.Enabled = false;
            SearchBtn.Visible = false;
            ClearSearchBtn.Visible = true;
            calendarGroup.Visible = true;
            paymentDate.Text = DateTime.Now.Date.ToString("d");
            SaveButton.Visible = true;
        }
    }
}