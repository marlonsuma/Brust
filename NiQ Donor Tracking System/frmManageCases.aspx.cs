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
    public partial class frmManageCases : System.Web.UI.Page
    {
        static String strPONumber = "";
        static String strShippingService = "";
        static String strTrackingNumber = "";
        static String strCaseQuantity = "";
        static String strActive = "";
        static int intLotID = 0;
        static int intLotCasesRemaining = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                if (ui.Administrator == false)
                {
                    Response.Redirect("Default.aspx");
                }
                txtCaseID.Focus();
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
            if (txtCaseID.Text.Length != 9 || txtCaseID.Text.StartsWith("CA") == false)
            {
                lblMessage.Text = "Please enter a valid Case ID.";
                txtCaseID.Text = "";
                txtCaseID.Focus();
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

            #region Check if Case exists
            // Read Case information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblCases WHERE [tblCases].[Barcode]='" + txtCaseID.Text.Trim().Replace("'", "''") + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    strActive = dr["Active"].ToString();
                    strPONumber = dr["PONumber"].ToString();
                    strShippingService = dr["ShippingService"].ToString();
                    strTrackingNumber = dr["TrackingNumber"].ToString();
                    strCaseQuantity = dr["CaseQuantity"].ToString();
                    intLotID = int.Parse(dr["LotID"].ToString());
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

            // Check if Case is found
            if (!blnFound)
            {
                lblMessage.Text = "Case [" + txtCaseID.Text + "] not found.";
                txtCaseID.Text = "";
                txtCaseID.Focus();
                return;
            }
            #endregion

            #region GetCaseLot
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblLots WHERE [tblLots].[ID]="+ intLotID;
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    intLotCasesRemaining = int.Parse(dr["CasesRemaining"].ToString());
                   
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
            #endregion

            txtPONumber.Text = strPONumber;
            txtPONumber.Enabled = true;
            txtTrackingNumber.Text = strTrackingNumber;
            txtTrackingNumber.Enabled = true;
            ddlShipping.SelectedValue = strShippingService;
            ddlShipping.Enabled = true;
            txtCaseQuantity.Text = strCaseQuantity;
            txtCaseQuantity.Enabled = true;
            chkActive.Enabled = true;
            chkActive.Checked = (strActive == "True");

            btnSearch.Enabled = false;
            txtCaseID.Enabled = false;
            btnSubmit.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            Int32 intNumberofCases = 0;
            String strInputBarcode = txtCaseID.Text.Trim().Replace("'", "''");
            String strNewPONumber = txtPONumber.Text.Trim().Replace("'", "''");
            String strNewCaseQuantity = txtCaseQuantity.Text.Trim().Replace("'","''");
            String strNewTrackingNumber = txtTrackingNumber.Text.Trim().Replace("'", "''");
            String strNewShippingService = ddlShipping.Text;
            String strNewActive = chkActive.Checked.ToString();
            String strSQL = "UPDATE tblCases SET ";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            String strReceiveDate = DateTime.Now.ToString();
            int intTransactionID = -1;
            int intCaseID = -1;
          

            #region Validate Input

            // Make sure PO number is entered
            if (strNewPONumber.Length <= 0)
            {
                lblMessage.Text = "Please enter a valid PO number.";
                txtPONumber.Focus();
                return;
            }

            // Make sure tracking number is entered
            if (strNewTrackingNumber.Length <= 0)
            {
                lblMessage.Text = "Please enter a valid tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            // Make sure Case Quantity is valid
            if (Int32.TryParse(strNewCaseQuantity, out intNumberofCases) == false)
            {
                lblMessage.Text = "Please enter a valid number of cases.";
                txtCaseQuantity.Text = "";
                txtCaseQuantity.Focus();
                return;
            }
            if (intNumberofCases <= 0)
            {
                lblMessage.Text = "Please enter a valid number of cases.";
                txtCaseQuantity.Text = "";
                txtCaseQuantity.Focus();
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

            #region Check what fields have changed
            // Check what fields have been changed
            if (strPONumber != strNewPONumber)
            {
                strFields.Add("PO Number");
                strValues.Add(strNewPONumber);
                strSQL += "[PONumber]='" + strNewPONumber + "',";
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

            int intCaseQty = int.Parse(strCaseQuantity);
            int intNewCaseQty = int.Parse(strNewCaseQuantity);

            if (strCaseQuantity != strNewCaseQuantity)
            {
              
                strFields.Add("Case Quantity");
                strValues.Add(strNewCaseQuantity);
                strSQL += "[CaseQuantity]='" + strNewCaseQuantity + "',";
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
            strSQL += " output INSERTED.ID WHERE [Barcode]='" + strInputBarcode + "'";
            if (strFields.Count <= 0)
            {
                lblMessage.Text = "No changes detected.";
                return;
            }
            #endregion

            #region Update Case
            try
            {
                conn.Open();
                sqlCommand.CommandText = strSQL;
                intCaseID = (int)sqlCommand.ExecuteScalar();
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

            #region UpdateLot
          
                conn.Open();

                if (intNewCaseQty > intCaseQty)
                {
                    int casesRemaining = System.Math.Abs(intNewCaseQty - intLotCasesRemaining);

                    if(casesRemaining >= 1)
                    {
                        sqlCommand.CommandText = "UPDATE tblLots SET [CasesRemaining] = " + casesRemaining + " WHERE ID = " + intLotID;
                        int affected = (int)sqlCommand.ExecuteNonQuery();
                    }

                    else if (casesRemaining == 0)
                    {
                        sqlCommand.CommandText = "UPDATE tblLots SET [CasesRemaining] = " + casesRemaining + " , [Closed]=1 WHERE ID = " + intLotID;
                        int affected = (int)sqlCommand.ExecuteNonQuery();
                    }

                }

                else if(intNewCaseQty < intCaseQty)
                {
                    int casesRemaining = intNewCaseQty + intLotCasesRemaining;

                    if (casesRemaining >= 1)
                    {
                        sqlCommand.CommandText = "UPDATE tblLots SET [CasesRemaining] = " + casesRemaining + " WHERE [ID] = " + intLotID;
                        int _lotID = (int)sqlCommand.ExecuteNonQuery();
                    }

                    else if (casesRemaining == 0)
                    {
                        sqlCommand.CommandText = "UPDATE tblLots SET [CasesRemaining] = " + casesRemaining + " , [Closed]=1 WHERE [ID] = " + intLotID;
                        int _lotID = (int)sqlCommand.ExecuteNonQuery();
                    }

                }


            conn.Close();
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Manage Case" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Case" + "'," +
                                            intCaseID.ToString() + ")";
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
            lblMessage.Text = "Case successfully updated.";
            txtPONumber.Text = "";
            txtPONumber.Enabled = false;
            txtTrackingNumber.Text = "";
            txtTrackingNumber.Enabled = false;
            ddlShipping.SelectedIndex = 0;
            ddlShipping.Enabled = false;
            txtCaseQuantity.Text = "";
            txtCaseQuantity.Enabled = false;
            chkActive.Checked = false;
            chkActive.Enabled = false;
            btnSubmit.Visible = false;
            btnSearch.Enabled = true;
            txtCaseID.Enabled = true;
            txtCaseID.Text = "";
            txtCaseID.Focus();
        }
    }
}