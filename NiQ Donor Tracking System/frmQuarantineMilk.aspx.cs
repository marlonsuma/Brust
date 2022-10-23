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
    public partial class frmQuarantineMilk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                LoadPrinters();
                txtMilkKitID.Focus();

                if (ddlPrinters.Items.Count < 1)
                {
                    lblMessage.Text = "No printers currently configured.  Please configure a printer before proceeding.";
                    btnSearch.Enabled = false;
                }
            }
        }

        private void LoadPrinters()
        {
            lblMessage.Text = "";

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            #endregion

            #region Get Printers
            try
            {
                ddlPrinters.Items.Clear();
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblPrinters";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    ddlPrinters.Items.Add(dr["PrinterName"].ToString());
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
                conn.Dispose();
            }
            #endregion

            if (ddlPrinters.Items.Count < 1)
            {
                ddlPrinters.Enabled = false;
                btnPrint.Enabled = false;
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
            String strQuarantineDate = "";

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
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] not found.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            // Check if Milk Kit is active
            if (!blnActive)
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] is not active.  Cannot quarantine.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            // Check Receive Date
            if (strReceiveDate == "")
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] has not been received.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }

            // Check Quarantine Date
            if (strQuarantineDate != "")
            {
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] has already been quarantined.";
                txtMilkKitID.Text = "";
                txtMilkKitID.Focus();
                return;
            }
            #endregion

            // Activate status controls and submit button
            lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] found.  Enter a volume and click 'Submit' to quarantine.";
            lblVolume.Visible = true;
            txtVolume.Visible = true;
            txtVolume.Enabled = true;
         
            btnSubmit.Visible = true;
            btnSearch.Visible = false;
            txtMilkKitID.Enabled = false;
            txtVolume.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            decimal decVolume = -1;
            String strInputBarcode = txtMilkKitID.Text.Trim().Replace("'", "''");
            String strQuarantineDate = DateTime.Now.ToString();
            String strMilkKitID = "-1";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            int intTransactionID = -1;

            #region Validate Input
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

            // Make sure pallet is entered
            /*
            if (txtPallet.Text.Trim().Length <= 0)
            {
                lblMessage.Text = "Please enter a valid pallet number.";
                txtPallet.Focus();
                return;
            } */

            // Round volume to hundreths
            txtVolume.Text = Math.Round(decVolume, 2).ToString();
            #endregion

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
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

            #region Update Milk Kit
            try
            {
                conn.Open();
                sqlCommand.CommandText = "UPDATE tblMilkKits SET [QuarantineDate]='" + strQuarantineDate + "',[Volume]='" + txtVolume.Text.Trim().Replace("'", "''") + "' WHERE [Barcode]='" + txtMilkKitID.Text.Trim() + "'";
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
            strFields.Add("Quarantine Date");
            strValues.Add(strQuarantineDate);
            strFields.Add("Volume");
            strValues.Add(txtVolume.Text.Trim().Replace("'", "''"));
          
          
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Quarantine Milk Bag" + "','" +
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

            lblMessage.Text = "Milk Kit [" + txtMilkKitID.Text.Trim() + "] successfully quarantined.  Select a printer and click 'Print Label' to generate bag labels.";
            lblSelectPrinter.Visible = true;
            ddlPrinters.Visible = true;
            btnPrint.Visible = true;
            btnSubmit.Visible = false;
            txtVolume.Enabled = false;
          
            btnNext.Visible = true;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        void ClearForm()
        {
            lblMessage.Text = "";
            txtVolume.Visible = false;
            lblVolume.Visible = false;
        
            lblSelectPrinter.Visible = false;
            ddlPrinters.Visible = false;
            btnPrint.Visible = false;
            btnNext.Visible = false;
            btnSubmit.Visible = false;
            btnSearch.Visible = true;
            txtMilkKitID.Enabled = true;
            btnSearch.Enabled = true;
            txtMilkKitID.Text = "";
            txtVolume.Text = "";
           
            txtMilkKitID.Focus();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            String strPrinterIP = "";
            String strLabelFormat = "";

            strLabelFormat = "^XA" + Environment.NewLine;
            strLabelFormat += "^MD15" + Environment.NewLine;
            strLabelFormat += "^LH0,0" + Environment.NewLine;
            strLabelFormat += "^PR2" + Environment.NewLine;
            strLabelFormat += "^BY3,2" + Environment.NewLine;
            strLabelFormat += "^PW525" + Environment.NewLine;
            strLabelFormat += "^FO100,50^BCN,110,N,N,N,A^FD" + txtMilkKitID.Text.Trim() + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO200,175^A0N,30,30^FD" + txtMilkKitID.Text.Trim() + "^FS" + Environment.NewLine;
            strLabelFormat += "^PQ1" + Environment.NewLine;
            strLabelFormat += "^XZ" + Environment.NewLine;

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            #endregion

            #region Get Printer Information
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblPrinters WHERE [PrinterName]='" + ddlPrinters.Text + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strPrinterIP = dr["PrinterIP"].ToString();
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

            #region Print Label
            // Print to IP Address
            TcpClient tcpSocket = new TcpClient();
            try
            {
                // Connect to the printer
                tcpSocket.Connect(strPrinterIP, Global.PrinterInfo.PORT);
                NetworkStream nsPrint = tcpSocket.GetStream();
                StreamWriter swPrint = new StreamWriter(nsPrint);

                // Start sending
                swPrint.WriteLine(strLabelFormat);

                swPrint.Flush();
                swPrint.Close();
                nsPrint.Close();
                tcpSocket.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Label Print Error: " + ex.Message;
                return;
            }
            #endregion

            lblMessage.Text = "Label printed successfully.";
        }
    }
}