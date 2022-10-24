using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Data.SqlClient;
using System.Collections.Specialized;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class frmReceiveMilkKit : System.Web.UI.Page
    {
        public IMilkKitRepository milkKitRepository { get; set; }
        public IDonorRepository donorRepository { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReceiveCalendar.Visible = false;
                txtCalendarReceiveDate.Enabled = false;
            }

            Global.UserInfo ui = (Global.UserInfo) Session["ui"];
            txtMilkKitID.Focus();
        }

        protected void imgReceiveCalendar_Click(object sender, ImageClickEventArgs e)
        {
            if (ReceiveCalendar.Visible == false)
            {
                ReceiveCalendar.Visible = true;
            }
            else
            {
                ReceiveCalendar.Visible = false;
            }
        }

        protected void receiveCalendar_SelectionChanged(object sender, EventArgs e)
        {
            txtCalendarReceiveDate.Text = ReceiveCalendar.SelectedDate.ToShortDateString();
            var date = DateTime.Parse(txtCalendarReceiveDate.Text);
            var sqlFormattedDate = date.ToString("yyyy-MM-dd");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bool blnFound = false;
            bool blnActive = false;
            String strReceiveDate = "";

            // Clear the message
            lblMessage.Text = "";

            #region Validate Input

            // See if user provided all the info
            if (txtMilkKitID.Text.StartsWith("MK") == false || txtMilkKitID.Text.Length != 9)
            {
                lblMessage.Text = "Please enter a valid Milk Collection Kit ID.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();

                return;
            }

            #endregion

            #region Setup Database Connection

            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            SqlConnection conn = new SqlConnection(strConnection);
            SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            SqlDataReader dr;

            #endregion

            #region Check if Milk Kit exists, is active and hasn't been received previously

            // Read Milk Kit information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblMilkKits WHERE [Barcode]='" + txtMilkKitID.Text.Trim() + "'";
                dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    blnFound = true;
                    blnActive = (dr["Active"].ToString() == "True");
                    strReceiveDate = dr["ReceiveDate"].ToString();
                }

                dr.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
                conn.Dispose();

                return;
            }
            finally
            {
                conn.Close();
            }

            // Check if Milk Kit is found
            if (!blnFound)
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] not found.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();

                return;
            }

            // Check if Milk Kit is active
            if (!blnActive)
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] is not active.  Cannot receive.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();

                return;
            }

            #endregion

            // Activate status controls and submit button
            lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text +
                              "] found. Select Received date to continue.";
            txtCalendarReceiveDate.Enabled = true;

            if (!string.IsNullOrEmpty(strReceiveDate))
            {
                var date = DateTime.Parse(strReceiveDate);
                var txtFormat = date.ToString("MM/dd/yyyy");
                txtCalendarReceiveDate.Text = txtFormat;
            }
            else
            {
                txtCalendarReceiveDate.Text = DateTime.Now.ToShortDateString();
            }

            btnSubmit.Visible = true;
            btnSearch.Visible = false;
            txtMilkKitID.Enabled = false;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            String strInputBarcode = txtMilkKitID.Text.Trim().Replace("'", "''");
            String strMilkKitID = "-1";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            String strReceiveDate = txtCalendarReceiveDate.Text;
            String strDbReceiveDate = "";
            int intTransactionID = -1;

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            SqlConnection conn = new SqlConnection(strConnection);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            SqlDataReader dr;
            #endregion

            #region Get Milk Kit ID
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblMilkKits WHERE [Barcode]='" + strInputBarcode + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strMilkKitID = dr["ID"].ToString();
                    strDbReceiveDate = dr["ReceiveDate"].ToString();

                }
                dr.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
                conn.Close();
                conn.Dispose();
                return;
            }
            #endregion

            #region Add or Update Milk Collection Kit
            try
            {
                DateTime pDate = DateTime.Today;
                if (DateTime.Parse(strReceiveDate) > pDate)
                {
                    lblMessage.Text = "Date [" + txtCalendarReceiveDate.Text + "] is not a valid date. It cannot be a date after today's date.";
                    txtMilkKitID.Enabled = true;
                    btnSubmit.Visible = true;
                    btnSubmit.Enabled = true;
                    btnSearch.Visible = true;
                    ReceiveCalendar.Visible = false;
                    btnSearch.Enabled = false;
                    txtCalendarReceiveDate.Text = txtCalendarReceiveDate.Text;
                    txtMilkKitID.Focus();
                    return;
                }
                conn.Open();
                if (string.IsNullOrEmpty(strDbReceiveDate))
                {
                    sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                                "Receive Milk Kit" + "','" +
                                                strReceiveDate + "','" +
                                                ui.ID + "','" +
                                                "Milk Kit" + "'," +
                                                strMilkKitID + ")";
                    intTransactionID = (int)sqlCommand.ExecuteScalar();

                    sqlCommand.CommandText = "INSERT INTO tblTransactionDetails ([TransactionID],[Field],[Value]) VALUES ('" + intTransactionID + "','Receive Date', '" + strReceiveDate + "')";
                    sqlCommand.ExecuteNonQuery();

                    lblMessage.Text = "Milk Collection Kit successfully received.";
                }
                else
                {
                    sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                             "Update Milk Kit" + "','" +
                                             strReceiveDate + "','" +
                                             ui.ID + "','" +
                                             "Milk Kit" + "'," +
                                             strMilkKitID + ")";
                    intTransactionID = (int)sqlCommand.ExecuteScalar();

                    sqlCommand.CommandText = "INSERT INTO tblTransactionDetails ([TransactionID],[Field],[Value]) VALUES ('" + intTransactionID + "','Old Receive Date', '" + strDbReceiveDate + "')";
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand.CommandText = "INSERT INTO tblTransactionDetails ([TransactionID],[Field],[Value]) VALUES ('" + intTransactionID + "','New Receive Date', '" + strReceiveDate + "')";
                    sqlCommand.ExecuteNonQuery();

                    lblMessage.Text = "Milk Collection Kit successfully updated.";
                }

                sqlCommand.CommandText = "UPDATE tblMilkKits SET [ReceiveDate]='" + strReceiveDate + "' WHERE [Barcode]='" + strInputBarcode + "'";
                sqlCommand.ExecuteNonQuery();

                var donorid = milkKitRepository.Get(strInputBarcode).DonorId;
                if(!string.IsNullOrEmpty(donorid))
                {
                    string toemail, body, Subject;
                    Subject = "Milk Kit Received";
                    toemail = donorRepository.Get(donorid).Email;
                    body = "Ni-Q has moved your milk kit further into the process of testing, we have pooled your milk kit. A sample is being collected. You will be notified once testing begins";
                    if (!string.IsNullOrEmpty(toemail) && toemail != "")
                        EMailHelper.SendEmail(toemail, body, Subject);
                }

                btnSubmit.Visible = false;
                btnSearch.Visible = true;
                ReceiveCalendar.Visible = false;
                txtMilkKitID.Enabled = true;
                txtMilkKitID.Text = "";
                txtCalendarReceiveDate.Text = "";
                txtCalendarReceiveDate.Enabled = false;
                txtMilkKitID.Focus();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            #endregion

        }
    }
}