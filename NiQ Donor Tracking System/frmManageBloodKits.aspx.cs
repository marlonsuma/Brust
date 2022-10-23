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
    public partial class frmManageBloodKits : System.Web.UI.Page
    {
        static String strReceiveDate = "";
        static String strDonorID = "";
        static String strShippingService = "";
        static String strTrackingNumber = "";
        static String strStatus = "";
        static String strActive = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                if (ui.Administrator == false)
                {
                    Response.Redirect("Default.aspx");
                }
                txtDIN.Focus();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bool blnFound = false;

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

            #region Check if Blood Kit exists
            // Read Blood Kit information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblBloodKits WHERE [DIN]='" + txtDIN.Text.Trim().Replace("'","''") + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    strDonorID = dr["DonorID"].ToString();
                    strShippingService = dr["ShippingService"].ToString();
                    strTrackingNumber = dr["TrackingNumber"].ToString();
                    strStatus = dr["Status"].ToString();
                    strReceiveDate = dr["ReceiveDate"].ToString();
                    strActive = dr["Active"].ToString();
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
            #endregion

            txtDonorNumber.Text = strDonorID;
            txtDonorNumber.Enabled = true;
            txtTrackingNumber.Text = strTrackingNumber;
            txtTrackingNumber.Enabled = true;
            ddlShipping.SelectedValue = strShippingService;
            ddlShipping.Enabled = true;
            chkActive.Enabled = true;
            chkActive.Checked = (strActive == "True");

            if (strReceiveDate.Length > 0)
            {
                radPass.Checked = (strStatus == "True");
                radFail.Checked = (strStatus == "False");
                radPass.Enabled = true;
                radFail.Enabled = true;
            }
            else
            {
                radPass.Checked = false;
                radFail.Checked = false;
            }

            btnSearch.Enabled = false;
            txtDIN.Enabled = false;
            btnSubmit.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            String strInputBarcode = txtDIN.Text.Trim().Replace("'", "''");
            String strNewDonorID = txtDonorNumber.Text.Trim().Replace("'","''");
            String strNewTrackingNumber = txtTrackingNumber.Text.Trim().Replace("'","''");
            String strNewShippingService = ddlShipping.Text;
            String strNewStatus = radPass.Checked.ToString();
            String strNewActive = chkActive.Checked.ToString();
            String strSQL = "UPDATE tblBloodKits SET ";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            String strReceiveDate = DateTime.Now.ToString();
            int intTransactionID = -1;
            int intBloodKitID = -1;

            #region Validate Input
            // Make sure tracking number is entered
            if (strNewTrackingNumber.Length <= 0)
            {
                lblMessage.Text = "Please enter a valid tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            // Check Donor Number Length
            if (strNewDonorID.Length <= 0)
            {
                lblMessage.Text = "Invalid entry [" + txtDonorNumber.Text + "]. Please enter a valid Donor Number.";
                txtDonorNumber.Text = "";
                txtDonorNumber.Focus();
                return;
            }
            #endregion

            #region Check what fields have changed
            // Check what fields have been changed
            if(strDonorID != strNewDonorID)
            {
                strFields.Add("Donor ID");
                strValues.Add(strNewDonorID);
                strSQL += "[DonorID]='" + strNewDonorID + "',";
            }
            if (strShippingService != strNewShippingService)
            {
                strFields.Add("Shipping Service");
                strValues.Add(strNewShippingService);
                strSQL += "[ShippingService]='" + strNewShippingService + "',";
            }
            if (strTrackingNumber != strNewTrackingNumber)
            {
                strFields.Add("Tracking Number");
                strValues.Add(strNewTrackingNumber);
                strSQL += "[TrackingNumber]='" + strNewTrackingNumber + "',";
            }
            if (radPass.Enabled == true)
            {
                if (strStatus != strNewStatus)
                {
                    strFields.Add("Status");
                    strValues.Add(strNewStatus);
                    strSQL += "[Status]='" + strNewStatus + "',";
                }
            }
            if (strActive != strNewActive)
            {
                strFields.Add("Active");
                strValues.Add(strNewActive);
                if (strNewActive == "True")
                {
                    strSQL += "[Active]=1,";
                }
                else
                {
                    strSQL += "[Active]=0,";
                }
            }
            strSQL = strSQL.TrimEnd(',');
            strSQL += " output INSERTED.ID WHERE [DIN]='" + strInputBarcode + "'";
            if (strFields.Count <= 0)
            {
                lblMessage.Text = "No changes detected.";
                return;
            }
            #endregion

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
                sqlCommand.CommandText = strSQL;
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

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Manage Blood Kit" + "','" +
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
            lblMessage.Text = "Blood Kit successfully updated.";
            txtDonorNumber.Text = "";
            txtDonorNumber.Enabled = false;
            txtTrackingNumber.Text = "";
            txtTrackingNumber.Enabled = false;
            chkActive.Checked = false;
            chkActive.Enabled = false;
            ddlShipping.SelectedIndex = 0;
            ddlShipping.Enabled = false;
            radPass.Checked = true;
            radFail.Checked = false;
            radPass.Enabled = false;
            radFail.Enabled = false;
            btnSubmit.Visible = false;
            btnSearch.Enabled = true;
            txtDIN.Enabled = true;
            txtDIN.Text = "";
            txtDIN.Focus();
        }
    }
}