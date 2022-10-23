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
    public partial class frmManageLots : System.Web.UI.Page
    {
        static String strTotalCases = "";
        static String strCasesRemaining = "";
        static String strSamplePouches = "";
        static String strClosed = "";
        static String strTransferred = "";
        static String strBestByDate = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                if (ui.Administrator == false)
                {
                    Response.Redirect("Default.aspx");
                }
                txtLotNumber.Focus();
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
            if (txtLotNumber.Text.Length != 9 || txtLotNumber.Text.StartsWith("LT") == false)
            {
                lblMessage.Text = "Please enter a valid Lot Number.";
                txtLotNumber.Text = "";
                txtLotNumber.Focus();
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

            #region Check if Lot exists
            // Read Lot information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblLots WHERE [Barcode]='" + txtLotNumber.Text.Trim().Replace("'", "''") + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    strTotalCases = dr["TotalCases"].ToString();
                    strCasesRemaining = dr["CasesRemaining"].ToString();
                    strSamplePouches = dr["SamplePouches"].ToString();
                    strClosed = dr["Closed"].ToString();
                    strTransferred = dr["Transferred"].ToString();
                    strBestByDate = dr["BestByDate"].ToString();
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

            // Check if Lot is found
            if (!blnFound)
            {
                lblMessage.Text = "Lot [" + txtLotNumber.Text + "] not found.";
                txtLotNumber.Text = "";
                txtLotNumber.Focus();
                return;
            }
            #endregion

            chkClosed.Checked = (strClosed == "True");
            chkClosed.Enabled = true;

            if (strTransferred == "True")
            {
                txtTotalCases.Enabled = true;
                txtTotalCases.Text = strTotalCases;
                txtCasesRemaining.Enabled = true;
                txtCasesRemaining.Text = strCasesRemaining;
                txtSamplePouches.Enabled = true;
                txtSamplePouches.Text = strSamplePouches;
                lotDate.Enabled = true;
                lotDate.Text = strBestByDate;
            }
            else
            {
                txtTotalCases.Text = "";
                txtTotalCases.Enabled = false;
                txtCasesRemaining.Text = "";
                txtCasesRemaining.Enabled = false;
                txtSamplePouches.Text = "";
                txtSamplePouches.Enabled = false;
                lotDate.Enabled = false;
                lotDate.Text = "";
            }

            btnSearch.Enabled = false;
            txtLotNumber.Enabled = false;
            btnSubmit.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            Int32 intTotalCases = 0;
            Int32 intSamplePouches = 0;
            Int32 intCasesRemaining = 0;
            String strInputBarcode = txtLotNumber.Text.Trim().Replace("'", "''");
            String strNewTotalCases = txtTotalCases.Text.Trim().Replace("'", "''");
            String strNewCasesRemaining = txtCasesRemaining.Text.Trim().Replace("'", "''");
            String strNewSamplePouches = txtSamplePouches.Text.Trim().Replace("'", "''");
            String strNewClosed = chkClosed.Checked.ToString();
            String strNewBestByDatee = lotDate.Text.Trim().Replace("'", "''");
            String strSQL = "UPDATE tblLots SET ";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            int intTransactionID = -1;
            int intLotID = -1;

            #region Validate Input
            if (strTransferred == "True")
            {
                if (Int32.TryParse(txtTotalCases.Text.Trim(), out intTotalCases) == false || intTotalCases <= 0)
                {
                    lblMessage.Text = "Please enter a valid number of total cases.";
                    txtTotalCases.Text = "";
                    txtTotalCases.Focus();
                    return;
                }
                if (Int32.TryParse(txtCasesRemaining.Text.Trim(), out intCasesRemaining) == false || intCasesRemaining < 0)
                {
                    lblMessage.Text = "Please enter a valid number of cases remaining.";
                    txtCasesRemaining.Text = "";
                    txtCasesRemaining.Focus();
                    return;
                }
                if (Int32.TryParse(txtSamplePouches.Text.Trim(), out intSamplePouches) == false || intSamplePouches < 0)
                {
                    lblMessage.Text = "Please enter a valid number of sample pouches.";
                    txtSamplePouches.Text = "";
                    txtSamplePouches.Focus();
                    return;
                }
                if (intCasesRemaining > intTotalCases)
                {
                    lblMessage.Text = "The number of cases remaining cannot exceed the total number of cases.";
                    txtCasesRemaining.Text = "";
                    txtCasesRemaining.Focus();
                    return;
                }
                if (intSamplePouches > 47)
                {
                    lblMessage.Text = "The number of sample pouches cannot equal or exceed 48.";
                    txtSamplePouches.Text = "";
                    txtSamplePouches.Focus();
                    return;
                }

                // If lot is not closed, make sure remaining cases > 0
                if (strNewClosed == "False" && intCasesRemaining <= 0)
                {
                    lblMessage.Text = "Please enter a valid number of cases remaining.  Cannot have an 'Open' lot with no cases remaining.";
                    txtCasesRemaining.Focus();
                    return;
                }

               
            }
            #endregion

            #region Check what fields have changed
            // Check what fields have been changed
            if (strTransferred == "True")
            {
                if (strTotalCases != strNewTotalCases)
                {
                    strFields.Add("Total Cases");
                    strValues.Add(strNewTotalCases);
                    strSQL += "[TotalCases]='" + strNewTotalCases + "',";
                }
                if (strCasesRemaining != strNewCasesRemaining)
                {
                    strFields.Add("Cases Remaining");
                    strValues.Add(strNewCasesRemaining);
                    strSQL += "[CasesRemaining]='" + strNewCasesRemaining + "',";
                }
                if (strSamplePouches != strNewSamplePouches)
                {
                    strFields.Add("Sample Pouches");
                    strValues.Add(strNewSamplePouches);
                    strSQL += "[SamplePouches]='" + strNewSamplePouches + "',";
                }

               

                if (strBestByDate != strNewBestByDatee)
                {
                    strFields.Add("Best By Date");
                    strValues.Add(strNewBestByDatee);
                    strSQL += "[BestByDate]='" + DateTime.Parse(strNewBestByDatee) + "',";
                }
            }

            if (strClosed != strNewClosed)
            {
                strFields.Add("Closed");
                strValues.Add(strNewClosed);
                if (strNewClosed == "True")
                {
                    strSQL += "[Closed]=1,";
                }
                else
                {
                    strSQL += "[Closed]=0,";
                }
            }
            strSQL = strSQL.TrimEnd(',');
            strSQL += " output INSERTED.ID WHERE [Barcode]='" + strInputBarcode + "'";
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

            #region Update Lot
            try
            {
                conn.Open();
                sqlCommand.CommandText = strSQL;
                intLotID = (int)sqlCommand.ExecuteScalar();
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
                                            "Manage Lot" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Lot" + "'," +
                                            intLotID.ToString() + ")";
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

            // Clear form and move to next Lot
            lblMessage.Text = "Lot successfully updated.";
            txtTotalCases.Text = "";
            txtTotalCases.Enabled = false;
            txtCasesRemaining.Text = "";
            txtCasesRemaining.Enabled = false;
            txtSamplePouches.Text = "";
            txtSamplePouches.Enabled = false;
            chkClosed.Checked = false;
            chkClosed.Enabled = false;
            btnSubmit.Visible = false;
            btnSearch.Enabled = true;
            txtLotNumber.Enabled = true;
            txtLotNumber.Text = "";
            txtLotNumber.Focus();
        }
    }
}