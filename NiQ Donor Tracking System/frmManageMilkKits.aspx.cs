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
    public partial class frmManageMilkKits : System.Web.UI.Page
    {
        static String strDonorID = "";
        static String strLotBarcode = "";
        static String strShippingService = "";
        static String strTrackingNumber = "";
        static String strVolume = "";
        static String strPallet = "";
        static String strDNA = "";
        static String strDrug = "";
        static String strMicrobial = "";
        static String strActive = "";
        static String strQuarantineDate = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                if (ui.Administrator == false)
                {
                    Response.Redirect("Default.aspx");
                }
                txtMilkKitID.Focus();
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
            if (txtMilkKitID.Text.Length != 9 || txtMilkKitID.Text.StartsWith("MK") == false)
            {
                lblMessage.Text = "Please enter a valid Milk Collection Kit ID.";
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

            #region Check if Milk Collection Kit exists
            // Read DNA Kit information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT *,[tblLots].[Barcode] AS [LotBarcode] FROM tblMilkKits LEFT JOIN [tblLots] ON [tblMilkKits].[LotID]=[tblLots].[ID] WHERE [tblMilkKits].[Barcode]='" + txtMilkKitID.Text.Trim().Replace("'", "''") + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    strActive = dr["Active"].ToString();
                    strDonorID = dr["DonorID"].ToString();
                    strLotBarcode = dr["LotBarcode"].ToString();
                    strShippingService = dr["ShippingService"].ToString();
                    strTrackingNumber = dr["TrackingNumber"].ToString();
                    strVolume = dr["Volume"].ToString();
                    strPallet = dr["Pallet"].ToString();
                    strDNA = dr["DNATest"].ToString();
                    strDrug = dr["DrugAlcoholTest"].ToString();
                    strMicrobial = dr["MicrobialTest"].ToString();
                    strQuarantineDate = dr["QuarantineDate"].ToString();
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
                lblMessage.Text = "Milk Collection Kit [" + txtMilkKitID.Text + "] not found.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }
            #endregion

            txtDonorID.Text = strDonorID;
            txtDonorID.Enabled = true;
            txtTrackingNumber.Text = strTrackingNumber;
            txtTrackingNumber.Enabled = true;
            ddlShipping.SelectedValue = strShippingService;
            ddlShipping.Enabled = true;

            if (strQuarantineDate.Length > 0)
            {
                txtVolume.Text = strVolume;
                txtVolume.Enabled = true;
                txtPallet.Text = strPallet;
                txtPallet.Enabled = true;
            }

            if (strDNA.Length > 0 || strDrug.Length > 0 || strMicrobial.Length > 0)
            {
                radDNAFail.Checked = (strDNA == "False");
                radDNAPass.Checked = (strDNA == "True");
                radDNAPass.Enabled = true;
                radDNAFail.Enabled = true;
                radDrugFail.Checked = (strDrug == "False");
                radDrugPass.Checked = (strDrug == "True");
                radDrugFail.Enabled = true;
                radDrugPass.Enabled = true;
                radMicrobialFail.Checked = (strMicrobial == "False");
                radMicrobialPass.Checked = (strMicrobial == "True");
                radMicrobialFail.Enabled = true;
                radMicrobialPass.Enabled = true;
            }

            if (strLotBarcode.Length > 0)
            {
                txtLotNumber.Text = strLotBarcode;
                txtLotNumber.Enabled = true;
            }

            chkActive.Enabled = true;
            chkActive.Checked = (strActive == "True");

            btnSearch.Enabled = false;
            txtMilkKitID.Enabled = false;
            btnSubmit.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            String strInputBarcode = txtMilkKitID.Text.Trim().Replace("'", "''");
            String strNewDonorID = txtDonorID.Text.Trim().Replace("'","''");
            String strNewTrackingNumber = txtTrackingNumber.Text.Trim().Replace("'", "''");
            String strNewShippingService = ddlShipping.Text;
            String strNewVolume = txtVolume.Text.Trim().Replace("'", "''");
            String strNewPallet = txtPallet.Text.Trim().Replace("'", "''");
            String strNewLotNumber = txtLotNumber.Text.Trim().Replace("'", "''");
            String strNewDNA = radDNAPass.Checked.ToString();
            String strNewDrug = radDrugPass.Checked.ToString();
            String strNewMicrobial = radMicrobialPass.Checked.ToString();
            String strNewActive = chkActive.Checked.ToString();
            String strNewLotID = "";
            decimal decVolume = 0;
            String strSQL = "UPDATE tblMilkKits SET ";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            String strReceiveDate = DateTime.Now.ToString();
            int intTransactionID = -1;
            int intMilkKitID = -1;
            bool blnFound = false;

            #region Validate Input

            // Make sure Donor ID is entered
            if (strNewDonorID.Length <= 0)
            {
                lblMessage.Text = "Please enter a valid Donor ID.";
                txtDonorID.Focus();
                return;
            }

            // Make sure tracking number is entered
            if (strNewTrackingNumber.Length <= 0)
            {
                lblMessage.Text = "Please enter a valid tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            if (txtVolume.Enabled == true)
            {
                // Make sure volume is entered
                if (txtVolume.Text.Trim().Length <= 0)
                {
                    lblMessage.Text = "Please enter a valid volume.";
                    txtVolume.Focus();
                    return;
                }

                // Make sure volume is numeric and greater than 0
                if (decimal.TryParse(txtVolume.Text.Trim(), out decVolume) == false)
                {
                    lblMessage.Text = "Please enter a valid volume.";
                    txtVolume.Focus();
                    return;
                }
                if (decVolume <= 0)
                {
                    lblMessage.Text = "Please enter a valid volume.  It must be greater than 0.";
                    txtVolume.Focus();
                    return;
                }
            }

            // Make sure pallet is entered, only if one already existed
            if (txtPallet.Enabled == true && strPallet.Length > 0)
            {
                if (txtPallet.Text.Trim().Length <= 0)
                {
                    lblMessage.Text = "Please enter a valid pallet number.";
                    txtPallet.Focus();
                    return;
                }
            }

            // Check Lot Length
            if (txtLotNumber.Enabled == true)
            {
                if (strNewLotNumber.Length != 9 || strNewLotNumber.StartsWith("LT") == false)
                {
                    lblMessage.Text = "Invalid entry [" + strNewLotNumber + "]. Please enter a valid Lot Number.";
                    txtMilkKitID.Text = "";
                    txtMilkKitID.Focus();
                    return;
                }
            }

            // Clear tests if they are disabled
            if (radDNAPass.Enabled == false)
            {
                strNewDNA = "";
                strNewDrug = "";
                strNewMicrobial = "";
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

            // If Donor ID changed, we have to make sure new Donor ID exists
            if (strDonorID != strNewDonorID)
            {
                try
                {
                    conn.Open();
                    sqlCommand.CommandText = "SELECT * FROM tblBloodKits WHERE [DonorID]='" + strNewDonorID + "'";
                    dr = sqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        blnFound = true;
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
                    lblMessage.Text = "Cannot find Donor ID [" + strNewDonorID + "].";
                    txtDonorID.Focus();
                    return;
                }

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
            if (strVolume != strNewVolume)
            {
                strFields.Add("Volume");
                strValues.Add(strNewVolume);
                strSQL += "[Volume]='" + strNewVolume + "',";
            }
            if (strPallet != strNewPallet)
            {
                strFields.Add("Pallet Number");
                strValues.Add(strNewPallet);
                strSQL += "[Pallet]='" + strNewPallet + "',";
            }
            if (strDNA != strNewDNA)
            {
                strFields.Add("DNA Test");
                strValues.Add(strNewDNA);
                strSQL += "[DNATest]='" + strNewDNA + "',";
            }
            if (strDrug != strNewDrug)
            {
                strFields.Add("Drug Alcohol Test");
                strValues.Add(strNewDrug);
                strSQL += "[DrugAlcoholTest]='" + strNewDrug + "',";
            }
            if (strMicrobial != strNewMicrobial)
            {
                strFields.Add("Microbial Test");
                strValues.Add(strNewMicrobial);
                strSQL += "[MicrobialTest]='" + strNewMicrobial + "',";
            }

            // If Lot Number changed, we have to make sure new Lot exists
            if (strLotBarcode != strNewLotNumber)
            {
                try
                {
                    conn.Open();
                    sqlCommand.CommandText = "SELECT * FROM tblLots WHERE [Barcode]='" + strNewLotNumber + "'";
                    dr = sqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        strNewLotID = dr["ID"].ToString();
                        blnFound = true;
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
                    lblMessage.Text = "Cannot find Lot Number [" + strNewLotNumber + "].";
                    txtLotNumber.Focus();
                    return;
                }

                strFields.Add("Lot ID");
                strValues.Add(strNewLotID);
                strSQL += "[LotID]=" + strNewLotID + ",";
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

            #region Update Milk Collection Kit
            try
            {
                conn.Open();
                sqlCommand.CommandText = strSQL;
                intMilkKitID = (int)sqlCommand.ExecuteScalar();
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
                                            "Manage Milk Kit" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Milk Kit" + "'," +
                                            intMilkKitID.ToString() + ")";
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
            lblMessage.Text = "Milk Collection Kit successfully updated.";
            txtTrackingNumber.Text = "";
            txtTrackingNumber.Enabled = false;
            ddlShipping.SelectedIndex = 0;
            ddlShipping.Enabled = false;
            txtDonorID.Text = "";
            txtDonorID.Enabled = false;
            txtVolume.Text = "";
            txtVolume.Enabled = false;
            txtPallet.Text = "";
            txtPallet.Enabled = false;
            txtLotNumber.Text = "";
            txtLotNumber.Enabled = false;
            radDNAFail.Checked = false;
            radDNAPass.Checked = false;
            radDNAFail.Enabled = false;
            radDNAPass.Enabled = false;
            radDrugFail.Checked = false;
            radDrugPass.Checked = false;
            radDrugFail.Enabled = false;
            radDrugPass.Enabled = false;
            radMicrobialPass.Checked = false;
            radMicrobialFail.Checked = false;
            radMicrobialPass.Enabled = false;
            radMicrobialFail.Enabled = false;
            chkActive.Checked = false;
            chkActive.Enabled = false;
            btnSubmit.Visible = false;
            btnSearch.Enabled = true;
            txtMilkKitID.Enabled = true;
            txtMilkKitID.Text = "";
            txtMilkKitID.Focus();
        }
    }
}