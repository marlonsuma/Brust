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
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class frmCreateBloodKit : System.Web.UI.Page
    {
        
        public IDonorRepository DonorRepository { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                LoadPrinters();
                txtDonorNumber.Focus();

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
            lblMessage.Text = "";
            int intInsertedSequence = -1;
            int intTransactionID = -1;
            int intBloodKitID = -1;
            String strSequence = "";
            String strDIN = "";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];

            string donorId = txtDonorNumber.Text.Trim();
            string trackingNo = txtTrackingNumber.Text.Trim();

            #region Validate Input
            // Make sure tracking number is entered
            if (string.IsNullOrEmpty(trackingNo))
            {
                lblMessage.Text = "Please enter a tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            // Check Donor Number Length
            if (string.IsNullOrEmpty(donorId))
            {
                lblMessage.Text = "Please enter a Donor Number.";
                txtDonorNumber.Text = "";
                txtDonorNumber.Focus();
                return;
            }

            // check if donor forms received
            var donor = DonorRepository.Get(donorId);

            if (donor == null)
            {
                lblMessage.Text = "Donor not found.";
                txtDonorNumber.Focus();
                return;
            }

            if (!donor.ReceiveConsentForm || !donor.ReceiveFinancialForm)
            {
                lblMessage.Text = "Donor consent and/or financial forms have not been received.";
                txtDonorNumber.Focus();
                return;
            }

            if (!donor.Active)
            {
                lblMessage.Text = "Donor inactive.";
                txtDonorNumber.Focus();
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

            #region Get DIN Sequence
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblSequences WHERE [Year]='" + DateTime.Now.Year.ToString() + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strSequence = dr["Sequence"].ToString();
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

            #region Create Sequence if none exist for current year
            if (strSequence == "")
            {
                try
                {
                    conn.Open();
                    sqlCommand.CommandText = "INSERT INTO tblSequences ([Year]) output INSERTED.Sequence VALUES('" + DateTime.Now.Year.ToString() + "')";
                    intInsertedSequence = (int)sqlCommand.ExecuteScalar();
                    if (intInsertedSequence < 0)
                    {
                        lblMessage.Text = "Error creating new sequence number for current year.  Cannot proceed.";
                        conn.Close();
                        conn.Dispose();
                        return;
                    }
                    else
                    {
                        strSequence = intInsertedSequence.ToString();
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
            }
            #endregion

            // Check if sequence has gone over 6 digits (ie: > 999999)
            if (strSequence.Length > 6)
            {
                lblMessage.Text = "Sequence for the current year has exceeded 6 digits.  Unable to create new kits.";
                return;
            }

            strDIN = "=W4237" + DateTime.Now.Year.ToString().Substring(2, 2) + strSequence.PadLeft(6,'0') + "00";

            #region Increment Sequence
            try
            {
                conn.Open();
                sqlCommand.CommandText = "UPDATE tblSequences SET [Sequence]='" + (Int32.Parse(strSequence)+1).ToString() + "' WHERE [Year]='" + DateTime.Now.Year.ToString() + "'";
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

            #region Create Blood Kit
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblBloodKits ([DonorID],[DIN],[ShippingService],[TrackingNumber]) output INSERTED.ID VALUES('" +
                               txtDonorNumber.Text.Trim().Replace("'", "''") + "','" +
                               strDIN + "','" +
                               ddlShipping.Text + "','" +
                               txtTrackingNumber.Text.Trim().Replace("'", "''") + "')";
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
            strFields.Add("Donor ID");
            strValues.Add(txtDonorNumber.Text.Trim().Replace("'", "''"));
            strFields.Add("DIN");
            strValues.Add(strDIN);
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
                                            "Create Blood Kit" + "','" +
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

            lblMessage.Text = "Blood Kit successfully created. Select a printer and click 'Print Label' to generate labels.";

            // Display new label information
            lblLabelInfo.Visible = true;
            lblISBTDIN.Visible = true;
            lblISBTDINTitle.Visible = true;
            lblISBTDIN.Text = strDIN;

            // Display print interface
            lblSelectPrinter.Visible = true;
            ddlPrinters.Visible = true;
            btnPrint.Visible = true;
            btnCreate.Enabled = false;
            txtDonorNumber.Enabled = false;
            ddlShipping.Enabled = false;
            txtTrackingNumber.Enabled = false;
            btnNext.Visible = true;
        }

        void ClearForm()
        {
            lblMessage.Text = "";
            lblLabelInfo.Visible = false;
            lblISBTDIN.Visible = false;
            lblISBTDINTitle.Visible = false;

            txtDonorNumber.Enabled = true;
            ddlShipping.Enabled = true;
            txtTrackingNumber.Enabled = true;
            txtDonorNumber.Text = "";
            txtTrackingNumber.Text = "";
            ddlShipping.SelectedIndex = 0;
            btnCreate.Enabled = true;
            lblSelectPrinter.Visible = false;
            ddlPrinters.Visible = false;
            btnPrint.Visible = false;
            btnNext.Visible = false;
            txtDonorNumber.Focus();
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
            lblMessage.Text = "";

            String strPrinterIP = "";
            String strLabelFormat = "";

            strLabelFormat = "^XA" + Environment.NewLine;
            strLabelFormat += "^MD15" + Environment.NewLine;
            strLabelFormat += "^LH0,0" + Environment.NewLine;
            strLabelFormat += "^PR2" + Environment.NewLine;
            strLabelFormat += "^BY3,2" + Environment.NewLine;
            strLabelFormat += "^PW525" + Environment.NewLine;
            strLabelFormat += "^FO50,50^BCN,110,N,N,N,A^FD" + lblISBTDIN.Text + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO105,175^A0N,30,30^FD" + lblISBTDIN.Text.Substring(1,5) + "  " + lblISBTDIN.Text.Substring(6,2) + "  " + lblISBTDIN.Text.Substring(8,6) + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO355,175^A0B,25,25^FD" + lblISBTDIN.Text.Substring(14,2) + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO405,175^A0N,28,28^FD" + CalculateCheckDigit(lblISBTDIN.Text.Substring(1,15)) + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO397,170^GB35,30,2,B,0^FS" + Environment.NewLine;
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