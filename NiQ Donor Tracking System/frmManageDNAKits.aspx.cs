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
    public partial class frmManageDNAKits : System.Web.UI.Page
    {
        static String strMilkKitID = "";
        static String strShippingService = "";
        static String strTrackingNumber = "";
        static String strDonorID = "";
        static String strActive = "";
        static String strMicrobialTest = "";
        static String strGeneticTest = "";
        static String strToxicologyTest = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                if (ui.Administrator == false)
                {
                    Response.Redirect("Default.aspx");
                }
                txtDNAKit.Focus();
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
            if (txtDNAKit.Text.Length != 9 || txtDNAKit.Text.StartsWith("DK") == false)
            {
                lblMessage.Text = "Please enter a valid DNA Kit ID.";
                txtDNAKit.Text = "";
                txtDNAKit.Focus();
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

            #region Check if DNA Kit exists
            // Read DNA Kit information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT *,[tblMilkKits].[Barcode] AS [MilkKitBarcode] FROM tblDNAKits INNER JOIN [tblMilkKits] ON [tblMilkKits].[ID]=[tblDNAKits].[MilkKitID] WHERE [tblDNAKits].[Barcode]='" + txtDNAKit.Text.Trim().Replace("'","''") + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    strActive = dr["Active"].ToString();
                    strMilkKitID = dr["MilkKitBarcode"].ToString();
                    strShippingService = dr["ShippingService"].ToString();
                    strTrackingNumber = dr["TrackingNumber"].ToString();
                    strDonorID = dr["DonorID"].ToString();
                    strMicrobialTest = dr["Microbial"].ToString();
                    strGeneticTest = dr["Genetic"].ToString();
                    strToxicologyTest = dr["Toxicology"].ToString();

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

            // Check if DNA Kit is found
            if (!blnFound)
            {
                lblMessage.Text = "DNA Kit [" + txtDNAKit.Text + "] not found.";
                txtDNAKit.Text = "";
                txtDNAKit.Focus();
                return;
            }
            #endregion

            txtMilkKitID.Text = strMilkKitID;
            txtMilkKitID.Enabled = true;
            txtTrackingNumber.Text = strTrackingNumber;
            txtTrackingNumber.Enabled = true;
            ddlShipping.SelectedValue = strShippingService;
            ddlShipping.Enabled = true;
            chkActive.Enabled = true;
            chkActive.Checked = (strActive == "True");

            chkMicrobial.Enabled = true;
            chkMicrobial.Checked = (strMicrobialTest == "True");

            chkGenetic.Enabled = true;
            chkGenetic.Checked = (strGeneticTest == "True");

            chkToxicology.Enabled = true;
            chkToxicology.Checked = (strToxicologyTest == "True");

            btnSearch.Enabled = false;
            txtDNAKit.Enabled = false;
            btnSubmit.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            String strInputBarcode = txtDNAKit.Text.Trim().Replace("'", "''");
            String strNewMilkKitID = "";
            String strNewDonorID = "";
            String strNewTrackingNumber = txtTrackingNumber.Text.Trim().Replace("'", "''");
            String strNewShippingService = ddlShipping.Text;
            String strNewMilkKitBarcode = txtMilkKitID.Text.Trim().Replace("'", "''");
            String strNewActive = chkActive.Checked.ToString();
            String strNewMicrobialTest = chkMicrobial.Checked.ToString();
            String strNewGeneticTest = chkGenetic.Checked.ToString();
            String strNewToxicologyTest = chkToxicology.Checked.ToString();
            String strSQL = "UPDATE tblDNAKits SET ";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            String strReceiveDate = DateTime.Now.ToString();
            int intTransactionID = -1;
            int intDNAKitID = -1;
            bool blnFound = false;

            #region Validate Input
            // Make sure tracking number is entered
            if (strNewTrackingNumber.Length <= 0)
            {
                lblMessage.Text = "Please enter a valid tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            // Check Milk Kit Length
            if (strNewMilkKitBarcode.Length != 9 || strNewMilkKitBarcode.StartsWith("MK") == false)
            {
                lblMessage.Text = "Invalid entry [" + strNewMilkKitBarcode + "]. Please enter a valid Milk Kit ID.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
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

            #region Check what fields have changed
            // Check what fields have been changed

            // If Milk Kit ID changed, we have to update Donor ID as well and get actual ID value
            if (strMilkKitID != strNewMilkKitBarcode)
            {
                try
                {
                    conn.Open();
                    sqlCommand.CommandText = "SELECT * FROM tblMilkKits WHERE [Barcode]='" + strNewMilkKitBarcode + "'";
                    dr = sqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        blnFound = true;
                        strNewMilkKitID = dr["ID"].ToString();
                        strNewDonorID = dr["DonorID"].ToString();
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

                if (blnFound == false)
                {
                    lblMessage.Text = "Cannot find Milk Collection Kit ID [" + strNewMilkKitBarcode + "].";
                    txtMilkKitID.Focus();
                    return;
                }

                strFields.Add("Milk Kit ID");
                strValues.Add(strNewMilkKitID);
                strSQL += "[MilkKitID]=" + strNewMilkKitID + ",";

                if (strDonorID != strNewDonorID)
                {
                    strFields.Add("Donor ID");
                    strValues.Add(strNewDonorID);
                    strSQL += "[DonorID]='" + strNewDonorID + "',";
                }
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

            if (strMicrobialTest != strNewMicrobialTest)
            {
                strFields.Add("Microbial");
                strValues.Add(strNewMicrobialTest);
                if (strNewMicrobialTest == "True")
                {
                    strSQL += "[Microbial]=1,";
                }
                else
                {
                    strSQL += "[Microbial]=0,";
                }
            }

            if (strGeneticTest != strNewGeneticTest)
            {
                strFields.Add("Genetic");
                strValues.Add(strNewGeneticTest);
                if (strNewGeneticTest == "True")
                {
                    strSQL += "[Genetic]=1,";
                }
                else
                {
                    strSQL += "[Genetic]=0,";
                }
            }

            if (strToxicologyTest != strNewToxicologyTest)
            {
                strFields.Add("Toxicology");
                strValues.Add(strNewToxicologyTest);
                if (strNewToxicologyTest == "True")
                {
                    strSQL += "[Toxicology]=1,";
                }
                else
                {
                    strSQL += "[Toxicology]=0,";
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

            #region Update DNA Kit
            try
            {
                conn.Open();
                sqlCommand.CommandText = strSQL;
                intDNAKitID = (int)sqlCommand.ExecuteScalar();
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
                                            "Manage DNA Kit" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "DNA Kit" + "'," +
                                            intDNAKitID.ToString() + ")";
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
            lblMessage.Text = "DNA Kit successfully updated.";
            txtMilkKitID.Text = "";
            txtMilkKitID.Enabled = false;
            txtTrackingNumber.Text = "";
            txtTrackingNumber.Enabled = false;
            ddlShipping.SelectedIndex = 0;
            ddlShipping.Enabled = false;
            chkActive.Checked = false;
            chkActive.Enabled = false;
            chkMicrobial.Checked = false;
            chkMicrobial.Enabled = false;
            chkGenetic.Checked = false;
            chkGenetic.Enabled = false;
            chkToxicology.Checked = false;
            chkToxicology.Enabled = false;
            btnSubmit.Visible = false;
            btnSearch.Enabled = true;
            txtDNAKit.Enabled = true;
            txtDNAKit.Text = "";
            txtDNAKit.Focus();
        }
    }
}