﻿using System;
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
    public partial class frmManagePallet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            txtMilkKitID.Focus();


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bool blnFound = false;
            bool blnActive = false;
            bool blnDNATest = false;
            bool blnDrugAlcoholTest = false;
            bool blnMicrobialTest = false;
            String strVolume = "";
            String strExistingLotID = "";
            string strGrade = "";

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

            // See if kit is already in list
            if (lstMilkKits.Items.FindByText(txtMilkKitID.Text) != null)
            {
                lblMessage.Text = "Milk Collection Kit [" + txtMilkKitID.Text + "] has already been added to the Pallet.";
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

            #region Check if Milk Kit exists and has passed all tests and is active
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
                    blnDNATest = (dr["DNATest"].ToString() == "True");
                    blnDrugAlcoholTest = (dr["DrugAlcoholTest"].ToString() == "True");
                    blnMicrobialTest = (dr["MicrobialTest"].ToString() == "True");
                    strExistingLotID = dr["PalletID"].ToString();
                    strVolume = dr["Volume"].ToString();
                    strGrade = dr["Grade"].ToString();
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
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] is not active.  Cannot add to Pallet.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            // Check Receive Date
            if (!blnDNATest || !blnDrugAlcoholTest || !blnMicrobialTest)
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] has not passed all tests and cannot be added to Pallet.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            // Check if Milk Kit is already in a Lot
            if (strExistingLotID != "")
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] is already in a Pallet.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            if (grade1.Checked && int.Parse(strGrade) != 1)
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] does not meet the grade threshold";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            if (lstMilkKits.Items.Count >= 36)
            {
                lblMessage.Text = "Can only have 36 milk kits on a pallet";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            #endregion

            // Add volume to current total
            lblTotalVolume.Text = (decimal.Parse(lblTotalVolume.Text) + decimal.Parse(strVolume)).ToString();



            // Activate status controls and submit button
            //lstMilkKits.Items.Add(txtMilkKitID.Text);

            lstMilkKits.Items.Insert(0, txtMilkKitID.Text);
            totalMilkKitsValue.Text = lstMilkKits.Items.Count.ToString();
            lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] added to Pallet.  Click 'Submit' to create Pallet.";
            btnSubmit.Visible = true;
            txtMilkKitID.Text = "";
            txtMilkKitID.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            string strNewBarcode = "";
            int intInsertedID = -1;
            int intTransactionID = -1;
            int palletID = 0;
            int Grade = 2;
            String strBestByDate = DateTime.Now.AddDays(365).ToString();
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            #endregion

            #region Create Lot
            try
            {


              
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblPallets WHERE [Barcode] = '" + palletNumber.Text + "'";
                dr = sqlCommand.ExecuteReader();


                while (dr.Read())
                {
                    palletID = int.Parse(dr["ID"].ToString());
                    Grade = int.Parse(dr["Grade"].ToString());
                }
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

            #region Update all Milk Kits
            try
            {
                conn.Open();
                sqlCommand.CommandText = "UPDATE tblMilkKits SET [PalletID]=" + palletID.ToString() + " WHERE [Barcode]='" + lstMilkKits.Items[0].Value + "'";
                for (int i = 1; i < lstMilkKits.Items.Count; i++)
                {
                    sqlCommand.CommandText += " OR [Barcode]='" + lstMilkKits.Items[i].Value + "'";
                }
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
                conn.Close();
            }
            #endregion

            #region Setup Transaction Fields/Values
            strFields.Add("Barcode");
            strValues.Add(strNewBarcode);
            strFields.Add("Best By Date");
            strValues.Add(strBestByDate);
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Create Pallet" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Lot" + "'," +
                                            intInsertedID.ToString() + ")";
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

            lblMessage.Text = "Pallet [" + strNewBarcode + "] successfully created with Best By Date of [" + strBestByDate + "].";
            lstMilkKits.Items.Clear();
            btnSubmit.Enabled = false;
            lblTotalVolume.Text = "0";
            totalMilkKitsValue.Text = "0";
            palletNumber.Text = "";
            palletNumber.Focus();
            txtMilkKitID.Text = "";
            gradeLabel.Visible = false;
            gradeValue.Visible = false;

        }

        protected void btnLoadPallet_Click(object sender, EventArgs e)
        {
            lstMilkKits.Items.Clear();
            lblTotalVolume.Text = "0";
            lblMessage.Text = "";
           
            

            int palletID = 0;
            int _grade = 2;
            bool exists = false;

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            #endregion

            #region Check if Milk Kit exists and has passed all tests and is active
            // Read Milk Kit information from database
            try
            {

                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblPallets WHERE [Barcode] = '" + palletNumber.Text + "'";
                dr = sqlCommand.ExecuteReader();
              

                while (dr.Read())
                {
                    exists = true;
                    palletID = int.Parse(dr["ID"].ToString());
                    palletNumber.Text = dr["Barcode"].ToString();
                    _grade = dr["Grade"].ToString() == "1" ? 1:2;
                   
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

            if(lstMilkKits.Items.Count >= 36)
            {
                lblMessage.Text = "Cannot add anymore milk kits to pallet max kits of 36 reached: " + palletNumber;
                palletNumber.Text = "";
                palletNumber.Focus();
                return;
            }

            if (exists && palletID != 0)
            {
                if (_grade == 1)
                {
                    grade1.Checked = true;
                    grade2.Checked = false;
                    gradeValue.Text = "1";
                }

                else
                {
                    grade1.Checked = false;
                    grade2.Checked = true;
                    gradeValue.Text = "2";
                }


                palletGradeLabel.Visible = true;
                grade1.Visible = true;
                grade2.Visible = true;
                milkKitCollectionLabel.Visible = true;
                txtMilkKitID.Visible = true;
                btnSearch.Visible = true;
                lstMilkKits.Visible = true;
                lblTotalVolumeTitle.Visible = true;
                lblTotalVolume.Visible = true;
                totalMilkKitsLabel.Visible = true;
                totalMilkKitsValue.Visible = true;
               
                gradeLabel.Visible = true;
                gradeValue.Visible = true;
            }

            else
            {
                lblMessage.Text = "Could not find pallet: " + palletNumber;
                palletNumber.Text = "";
                palletNumber.Focus();
                return;

            }

            #endregion


            #region LoadPalletMilkKits
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblMilkKits WHERE PalletID = " + palletID;
                dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    lstMilkKits.Items.Add(dr["Barcode"].ToString());
                    lblTotalVolume.Text = (decimal.Parse(lblTotalVolume.Text) + decimal.Parse(dr["Volume"].ToString())).ToString();
                   
                }


                totalMilkKitsValue.Text = lstMilkKits.Items.Count.ToString();
                dr.Dispose();
                return;
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
        }
    }
}