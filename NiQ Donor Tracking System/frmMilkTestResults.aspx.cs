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
    public partial class frmMilkTestResults : System.Web.UI.Page
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
            String blnFinalized = "";
            String strQuarantineDate = "";
            String strGeneticTest = "";
            String strToxiTest = "";
            String strMicrobialTest = "";

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

            #region Check if Milk Kit exists and has been quarantined and is Active
            // Read Milk Collection Kit information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblMilkKits WHERE [Barcode]='" + txtMilkKitID.Text.Trim() + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    blnActive = (dr["Active"].ToString() == "True");
                    strQuarantineDate = dr["QuarantineDate"].ToString();
                    strGeneticTest = dr["DNATest"].ToString();
                    strToxiTest = dr["DrugAlcoholTest"].ToString();
                    strMicrobialTest = dr["MicrobialTest"].ToString();
                    blnFinalized = dr["DateFinalized"].ToString();
                }
                dr.Dispose();

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
                    lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] is not active.  Cannot record test results.";
                    txtMilkKitID.Text = "";
                    txtMilkKitID.Focus();
                    return;
                }

                // Check if Milk Kit has been finalized
                if (blnFinalized != "")
                {
                    lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] found.  All test results have been previously recorded and finalized";
                    txtMilkKitID.Text = "";
                    txtMilkKitID.Focus();
                    return;
                }

                // Check Quarantine Date
                if (strQuarantineDate == "")
                {
                    lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] has not been quarantined.";
                    txtMilkKitID.Text = "";
                    txtMilkKitID.Focus();
                    return;
                }
                #endregion

                if (blnFinalized == "")
                {
                    // Activate status controls and submit button
                    if (strMicrobialTest != "")
                    {
                        radMicrobialPass.Enabled = true;
                        radMicrobialFail.Enabled = true;
                        radMicrobialPass.Checked = (strMicrobialTest == "True");
                        radMicrobialFail.Checked = (strMicrobialTest == "False");
                    }

                    if (strToxiTest != "")
                    {
                        radToxiPass.Enabled = true;
                        radToxiFail.Enabled = true;
                        radToxiPass.Checked = (strToxiTest == "True");
                        radToxiFail.Checked = (strToxiTest == "False");
                    }

                    if (strGeneticTest != "NULL")
                    {
                        radGeneticPass.Enabled = true;
                        radGeneticFail.Enabled = true;
                        radGeneticNA.Enabled = true;
                        radGeneticPass.Checked = (strGeneticTest == "True");
                        radGeneticFail.Checked = (strGeneticTest == "False");
                        radGeneticNA.Checked = (strGeneticTest == "");
                    }
                }
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

            lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] found.  Select test results and click 'Save'.";
            btnSubmit.Visible = true;
            checkBoxFinalize.Enabled = true;
            radMicrobialPass.Enabled = true;
            radMicrobialFail.Enabled = true;
            radToxiPass.Enabled = true;
            radToxiFail.Enabled = true;
            radGeneticPass.Enabled = true;
            radGeneticFail.Enabled = true;
            radGeneticNA.Enabled = true;
            btnSearch.Visible = false;
            txtMilkKitID.Enabled = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            String strGenetic = "NULL";
            String strToxi = "NULL";
            String strMicrobial = "NULL";
            DateTime? dateFinalized = null;
            String strMilkKitID = "-1";
            String strInputBarcode = txtMilkKitID.Text.Trim().Replace("'", "''");
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            int intTransactionID = -1;

            #region Validate Input
            if (radMicrobialFail.Checked == true)
            {
                strMicrobial = "0";
            }
            else if (radMicrobialPass.Checked == true)
            {
                strMicrobial = "1";
            }
            if (radToxiFail.Checked == true)
            {
                strToxi = "0";
            }
            else if (radToxiPass.Checked == true)
            {
                strToxi = "1";
            }
            if (radGeneticFail.Checked == true)
            {
                strGenetic = "0";
            }
            else if (radGeneticPass.Checked == true)
            {
                if (strMicrobial == "0")
                {
                    lblMessage.Text = "Genetic Testing cannot be selected as Pass if Microbial or Toxicology Test has failed. Please make sure the test results are correct and try agian.";
                    btnSubmit.Visible = true;
                    checkBoxFinalize.Enabled = true;
                    btnSearch.Visible = false;
                    txtMilkKitID.Enabled = false;
                    return;
                }
                else if (strToxi == "0")
                {
                    lblMessage.Text = "Genetic Testing cannot be selected as Pass if Microbial or Toxicology Test has failed. Please make sure the test results are correct and try agian.";
                    btnSubmit.Visible = true;
                    checkBoxFinalize.Enabled = true;
                    btnSearch.Visible = false;
                    txtMilkKitID.Enabled = false;
                    return;
                }
                strGenetic = "1";
            }
            else if (radGeneticNA.Checked == true)
            {
                strGenetic = "NULL";
            }
            if (checkBoxFinalize.Checked == true)
            {
                dateFinalized = DateTime.Now;
            }
            #endregion

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

            #region Update Milk Collection Kit
            try
            {
                if (dateFinalized == null)
                {
                    conn.Open();
                    sqlCommand.CommandText = "UPDATE tblMilkKits SET " +
                                             "[DNATest]=" + strGenetic +
                                             ",[DrugAlcoholTest]=" + strToxi +
                                             ",[MicrobialTest]=" + strMicrobial +
                                             " WHERE [Barcode]='" + strInputBarcode + "'";
                    sqlCommand.ExecuteNonQuery();
                }
                // If DatTime does not equal to null, add in the date finalized
                else
                {

                    conn.Open();
                    sqlCommand.CommandText = "UPDATE tblMilkKits SET " +
                                             "[DNATest]=" + strGenetic +
                                             ",[DrugAlcoholTest]=" + strToxi +
                                             ",[MicrobialTest]=" + strMicrobial +
                                             ",[DateFinalized]='" + dateFinalized +
                                             "' WHERE [Barcode]='" + strInputBarcode + "'";
                    sqlCommand.ExecuteNonQuery();
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

            #region Setup Transaction Fields/Values
            if (strMicrobial != "NULL")
            {
                strFields.Add("Microbial Test");
                if (strMicrobial == "1")
                {
                    strValues.Add("Pass");
                }
                else
                {
                    strValues.Add("Fail");
                }
            }
            if (strToxi != "NULL")
            {
                strFields.Add("Drug Alcohol Test");
                if (strToxi == "1")
                {
                    strValues.Add("Pass");
                }
                else
                {
                    strValues.Add("Fail");
                }
            }
            if (strGenetic != "3")
            {
                strFields.Add("DNA Test");
                if (strGenetic == "1")
                {
                    strValues.Add("Pass");
                }
                else if (strGenetic == "0")
                {
                    strValues.Add("Fail");
                }
                else
                {
                    strValues.Add("NULL");
                }
            }
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Lab Results" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Milk Kit" + "'," +
                                            strMilkKitID + ")";
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
            checkBoxFinalize.Checked = false;
            checkBoxFinalize.Enabled = false;
            radGeneticPass.Checked = false;
            radGeneticFail.Checked = false;
            radGeneticNA.Checked = false;
            radGeneticPass.Enabled = false;
            radGeneticFail.Enabled = false;
            radGeneticNA.Enabled = false;
            radToxiPass.Checked = false;
            radToxiFail.Checked = false;
            radToxiPass.Enabled = false;
            radToxiFail.Enabled = false;
            radMicrobialPass.Checked = false;
            radMicrobialFail.Checked = false;
            radMicrobialPass.Enabled = false;
            radMicrobialFail.Enabled = false;
            btnSubmit.Visible = false;
            btnSearch.Visible = true;
            txtMilkKitID.Enabled = true;
            txtMilkKitID.Text = "";
            txtMilkKitID.Focus();
        }

        protected void radGeneticPass_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
        }

        protected void radGeneticFail_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
        }

        protected void radToxiPass_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
        }

        protected void radToxiFail_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
        }

        protected void radMicrobialPass_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
        }

        protected void radMicrobialFail_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
        }

        protected void radGeneticsNA_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
        }
    }
}