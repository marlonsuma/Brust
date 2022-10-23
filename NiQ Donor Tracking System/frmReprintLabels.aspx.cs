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
    public partial class frmReprintLabels : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                LoadPrinters();
                txtBarcode.Focus();

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

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        void ClearForm()
        {
            lblMessage.Text = "";
            lblLabelInfo.Visible = false;
            lblPrintedBarcode.Visible = false;
            lblBarcodeTitle.Visible = false;
            txtBarcode.Enabled = true;
            lblSelectPrinter.Visible = false;
            ddlPrinters.Visible = false;
            btnSearch.Enabled = true;
            btnPrint.Visible = false;
            btnNext.Visible = false;
            txtBarcode.Focus();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Trim().Replace("'", "''").Length <= 0)
            {
                txtBarcode.Focus();
                return;
            }

            // Display new label information
            lblLabelInfo.Visible = true;
            lblBarcodeTitle.Visible = true;
            lblPrintedBarcode.Visible = true;
            lblPrintedBarcode.Text = txtBarcode.Text.Trim().Replace("'", "''");

            // Display print interface
            lblSelectPrinter.Visible = true;
            ddlPrinters.Visible = true;
            btnPrint.Visible = true;
            btnNext.Visible = true;

            // Disable submitting next barcode
            btnSearch.Enabled = false;
            txtBarcode.Text = "";
            txtBarcode.Enabled = false;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            String strPrinterIP = "";
            String strLabelFormat = "";
            String strLabelType = "Generic";

            #region Determine Label Type
            if (lblPrintedBarcode.Text.StartsWith("=") == true && lblPrintedBarcode.Text.Length == 16)
            {
                strLabelType = "ISBT 128 DIN";
            }
            else if (lblPrintedBarcode.Text.Length == 9)
            {
                if (lblPrintedBarcode.Text.Substring(0, 2) == "BK" || lblPrintedBarcode.Text.Substring(0, 2) == "MK" || lblPrintedBarcode.Text.Substring(0, 2) == "DK" || lblPrintedBarcode.Text.Substring(0, 2) == "CA" || lblPrintedBarcode.Text.Substring(0, 2) == "LT")
                {
                    strLabelType = "Standard";
                }
            }
            #endregion

            #region Create Label Format
            if (strLabelType == "ISBT 128 DIN")
            {
                strLabelFormat = "^XA" + Environment.NewLine;
                strLabelFormat += "^MD15" + Environment.NewLine;
                strLabelFormat += "^LH0,0" + Environment.NewLine;
                strLabelFormat += "^PR2" + Environment.NewLine;
                strLabelFormat += "^BY3,2" + Environment.NewLine;
                strLabelFormat += "^PW525" + Environment.NewLine;
                strLabelFormat += "^FO50,50^BCN,110,N,N,N,A^FD" + lblPrintedBarcode.Text + "^FS" + Environment.NewLine;
                strLabelFormat += "^FO105,175^A0N,30,30^FD" + lblPrintedBarcode.Text.Substring(1, 5) + "  " + lblPrintedBarcode.Text.Substring(6, 2) + "  " + lblPrintedBarcode.Text.Substring(8, 6) + "^FS" + Environment.NewLine;
                strLabelFormat += "^FO355,175^A0B,25,25^FD" + lblPrintedBarcode.Text.Substring(14, 2) + "^FS" + Environment.NewLine;
                strLabelFormat += "^FO405,175^A0N,28,28^FD" + CalculateCheckDigit(lblPrintedBarcode.Text.Substring(1, 15)) + "^FS" + Environment.NewLine;
                strLabelFormat += "^FO397,170^GB35,30,2,B,0^FS" + Environment.NewLine;
                strLabelFormat += "^PQ1" + Environment.NewLine;
                strLabelFormat += "^XZ" + Environment.NewLine;
            }
            else if (strLabelType == "Standard")
            {
                strLabelFormat = "^XA" + Environment.NewLine;
                strLabelFormat += "^MD15" + Environment.NewLine;
                strLabelFormat += "^LH0,0" + Environment.NewLine;
                strLabelFormat += "^PR2" + Environment.NewLine;
                strLabelFormat += "^BY3,2" + Environment.NewLine;
                strLabelFormat += "^PW525" + Environment.NewLine;
                strLabelFormat += "^FO100,50^BCN,110,N,N,N,A^FD" + lblPrintedBarcode.Text + "^FS" + Environment.NewLine;
                strLabelFormat += "^FO200,175^A0N,30,30^FD" + lblPrintedBarcode.Text + "^FS" + Environment.NewLine;
                strLabelFormat += "^PQ1" + Environment.NewLine;
                strLabelFormat += "^XZ" + Environment.NewLine;
            }
            else if (strLabelType == "Generic")
            {
                strLabelFormat = "^XA" + Environment.NewLine;
                strLabelFormat += "^MD15" + Environment.NewLine;
                strLabelFormat += "^LH0,0" + Environment.NewLine;
                strLabelFormat += "^PR2" + Environment.NewLine;
                strLabelFormat += "^BY2,2" + Environment.NewLine;
                strLabelFormat += "^PW525" + Environment.NewLine;
                strLabelFormat += "^FO50,50^BCN,110,N,N,N,A^FD" + lblPrintedBarcode.Text + "^FS" + Environment.NewLine;
                strLabelFormat += "^FO50,175^A0N,30,30^FD" + lblPrintedBarcode.Text + "^FS" + Environment.NewLine;
                strLabelFormat += "^PQ1" + Environment.NewLine;
                strLabelFormat += "^XZ" + Environment.NewLine;
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
    }
}