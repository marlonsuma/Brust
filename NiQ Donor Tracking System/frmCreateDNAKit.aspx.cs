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
    public partial class frmCreateDNAKit : System.Web.UI.Page
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
                    btnCreate.Enabled = false;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            bool blnFound = false;
            bool blnActive = false;
            lblMessage.Text = "";
            int intInsertedID = -1;
            int intTransactionID = -1;
            String strInputBarcode = txtMilkKitID.Text.Trim().Replace("'", "''");
            String strBarcode = "";
            String strDonorID = "";
            String strMilkKitID = "-1";
            String strQuarantineDate = "";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];

            #region Validate Input
            // Make sure tracking number is entered
            if (txtTrackingNumber.Text.Trim().Length <= 0)
            {
                lblMessage.Text = "Please enter a valid tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            // Check Milk Kit ID
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

            #region Get Milk Kit information and check if it has been Quarantined and Active
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblMilkKits WHERE [Barcode]='" + strInputBarcode + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    blnActive = (dr["Active"].ToString() == "True");
                    strDonorID = dr["DonorID"].ToString();
                    strQuarantineDate = dr["QuarantineDate"].ToString();
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
                lblMessage.Text = "Milk Collection Kit ID [" + txtMilkKitID.Text + "] is not active.  Cannot Create DNA Kit.";
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

            // Display new label information
            lblLabelInfo.Visible = true;
            lblDonorID.Visible = true;
            lblDonorIDTitle.Visible = true;
            lblDonorID.Text = strDonorID;

            #endregion

            #region Create DNA Kit
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblDNAKits ([DonorID],[MilkKitID],[DateOrdered],[ShippingService],[TrackingNumber]) output INSERTED.ID VALUES('" +
                                            lblDonorID.Text.Trim().Replace("'","''") + "'," +
                                            strMilkKitID + ",'" +
                                            DateTime.Now + "','" +
                                            ddlShipping.Text + "','" +
                                            txtTrackingNumber.Text.Trim().Replace("'", "''") + "')";
                intInsertedID = (int)sqlCommand.ExecuteScalar();
                strBarcode = "DK" + intInsertedID.ToString().PadLeft(7, '0');
                sqlCommand.CommandText = "UPDATE tblDNAKits SET [Barcode]='" + strBarcode + "' WHERE [ID]=" + intInsertedID;
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
            strFields.Add("Donor ID");
            strValues.Add(strDonorID);
            strFields.Add("Barcode");
            strValues.Add(strBarcode);
            strFields.Add("Milk Kit ID");
            strValues.Add(strMilkKitID);
            strFields.Add("Shipping Service");
            strValues.Add(ddlShipping.Text);
            strFields.Add("Tracking Number");
            strValues.Add(txtTrackingNumber.Text.Trim().Replace("'", "''"));
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Create DNA Kit" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "DNA Kit" + "'," +
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

            lblMessage.Text = "DNA Kit successfully created. Select a printer and click 'Print Label' to generate labels.";

            // Display print interface
            lblSelectPrinter.Visible = true;
            ddlPrinters.Visible = true;
            btnPrint.Visible = true;
            btnCreate.Enabled = false;
            txtMilkKitID.Enabled = false;
            ddlShipping.Enabled = false;
            txtTrackingNumber.Enabled = false;
            btnNext.Visible = true;
        }

        void ClearForm()
        {
            lblMessage.Text = "";
            lblLabelInfo.Visible = false;
            lblDonorID.Visible = false;
            lblDonorIDTitle.Visible = false;

            txtMilkKitID.Enabled = true;
            ddlShipping.Enabled = true;
            txtTrackingNumber.Enabled = true;
            txtMilkKitID.Text = "";
            txtTrackingNumber.Text = "";
            ddlShipping.SelectedIndex = 0;
            btnCreate.Enabled = true;
            lblSelectPrinter.Visible = false;
            ddlPrinters.Visible = false;
            btnPrint.Visible = false;
            btnNext.Visible = false;
            txtMilkKitID.Focus();
        }

        string CalculateCheckDigit(string barcode)
        {
            int i, intLen, intCheck, sum;
            string strTable;
            char strChar;

            i = intLen = intCheck = sum = 0;

            intLen = barcode.Length;
            strTable = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ*";

            // Perform the algorithm
            for (i = 0; i < intLen - 2; i++)
            {
                strChar = barcode[i];
                if (strChar <= 57 & strChar >= 48)
                {
                    intCheck = strChar - 48;
                }
                else if (strChar >= 65 & strChar <= 90)
                {
                    intCheck = (strChar - 65) + 10;
                }
                sum = (sum + intCheck) * 2;
                while (sum >= 37)
                {
                    sum = sum - 37;
                }
            }
            intCheck = 38 - sum;
            while (intCheck >= 37)
            {
                intCheck = intCheck - 37;
            }
            strChar = strTable[intCheck];
            return strChar.ToString();
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //Prints out 2 donor ID barcodes
            lblMessage.Text = "";

            String strPrinterIP = "";
            String strLabelFormat = "";

            strLabelFormat = "^XA" + Environment.NewLine;
            strLabelFormat += "^MD15" + Environment.NewLine;
            strLabelFormat += "^LH0,0" + Environment.NewLine;
            strLabelFormat += "^PR2" + Environment.NewLine;
            strLabelFormat += "^BY2,2" + Environment.NewLine;
            strLabelFormat += "^PW525" + Environment.NewLine;
            strLabelFormat += "^FO50,50^BCN,110,N,N,N,A^FD" + lblDonorID.Text + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO50,175^A0N,30,30^FD" + lblDonorID.Text + "^FS" + Environment.NewLine;
            strLabelFormat += "^PQ2" + Environment.NewLine;
            strLabelFormat += "^XZ" + Environment.NewLine;

            //Prints out 3 milk kit barcodes
            strLabelFormat = "^XA" + Environment.NewLine;
            strLabelFormat += "^MD15" + Environment.NewLine;
            strLabelFormat += "^LH0,0" + Environment.NewLine;
            strLabelFormat += "^PR2" + Environment.NewLine;
            strLabelFormat += "^BY3,2" + Environment.NewLine;
            strLabelFormat += "^PW525" + Environment.NewLine;
            strLabelFormat += "^FO100,50^BCN,110,N,N,N,A^FD" + txtMilkKitID.Text + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO200,175^A0N,30,30^FD" + txtMilkKitID.Text + "^FS" + Environment.NewLine;
            strLabelFormat += "^PQ3" + Environment.NewLine;
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

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}