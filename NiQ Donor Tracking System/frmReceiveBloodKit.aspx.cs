using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;

namespace NiQ_Donor_Tracking_System
{
    public partial class frmReceiveBloodKit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                txtDIN.Focus();
            }
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
            if (txtDIN.Text.Length != 16 || txtDIN.Text.StartsWith("=") == false)
            {
                lblMessage.Text = "Please enter a valid Blood Kit ISBT 128 DIN.";
                txtDIN.Text = "";
                txtDIN.Focus();
                return;
            }
            #endregion

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            #endregion

            #region Check if Blood Kit exists, is Active and hasn't been received previously
            // Read Blood Kit information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblBloodKits WHERE [DIN]='" + txtDIN.Text.Trim() + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    strReceiveDate = dr["ReceiveDate"].ToString();
                    blnActive = (dr["Active"].ToString() == "True");
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

            // Check if Blood Kit is found
            if (!blnFound)
            {
                lblMessage.Text = "Blood Kit [" + txtDIN.Text + "] not found.";
                txtDIN.Text = "";
                txtDIN.Focus();
                return;
            }

            // Check if Blood Kit is Inactive
            if (!blnActive)
            {
                lblMessage.Text = "Blood Kit [" + txtDIN.Text + "] is inactive. Cannot receive.";
                txtDIN.Text = "";
                txtDIN.Focus();
                return;
            }
            
            // Check Receive Date
            if (strReceiveDate != "")
            {
                lblMessage.Text = "Blood Kit [" + txtDIN.Text + "] has already been received.";
                txtDIN.Text = "";
                txtDIN.Focus();
                return;
            }
            #endregion

            // Activate status controls and submit button
            lblMessage.Text = "Blood Kit [" + txtDIN.Text + "] found.  Select test status and click 'Submit' to save.";
            radPass.Enabled = true;
            radFail.Enabled = true;
            btnSubmit.Visible = true;
            btnSearch.Visible = false;
            txtDIN.Enabled = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            String strInputBarcode = txtDIN.Text.Trim().Replace("'", "''");
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            String strReceiveDate = DateTime.Now.ToString();
            int intTransactionID = -1;
            int intBloodKitID = -1;

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            #endregion

            #region Update Blood Kit
            try
            {
                conn.Open();
                sqlCommand.CommandText = "UPDATE tblBloodKits SET [Status]='" + radPass.Checked.ToString() + "',[ReceiveDate]='" + strReceiveDate + "' output INSERTED.ID WHERE [DIN]='" + strInputBarcode + "'";
                intBloodKitID = (int)sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
                conn.Close();
                conn.Dispose();
                return;
            }
            finally
            {
                conn.Close();
            }
            #endregion

            #region Setup Transaction Fields/Values
            strFields.Add("Status");
            strValues.Add(radPass.Checked.ToString());
            strFields.Add("Receive Date");
            strValues.Add(strReceiveDate);
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Receive Blood Kit" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Blood Kit" + "'," +
                                            intBloodKitID.ToString() + ")";
                intTransactionID = (int)sqlCommand.ExecuteScalar();

                sqlCommand.CommandText = "INSERT INTO tblTransactionDetails ([TransactionID],[Field],[Value]) VALUES";
                for (int intChangeCount = 0; intChangeCount < strFields.Count; intChangeCount++)
                {
                    sqlCommand.CommandText += "(" + intTransactionID.ToString() + ",'" + strFields[intChangeCount] + "','" + strValues[intChangeCount] + "'),";
                }
                sqlCommand.CommandText = sqlCommand.CommandText.TrimEnd(',');
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
                conn.Close();
                conn.Dispose();
                return;
            }
            finally
            {
                conn.Dispose();
            }
            #endregion

            // Clear form and move to next kit
            lblMessage.Text = "Blood Kit successfully received.";
            radPass.Checked = true;
            radFail.Checked = false;
            radPass.Enabled = false;
            radFail.Enabled = false;
            btnSubmit.Visible = false;
            btnSearch.Visible = true;
            txtDIN.Enabled = true;
            txtDIN.Text = "";
            txtDIN.Focus();
        }
    }
}