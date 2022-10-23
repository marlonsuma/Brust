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
    public partial class frmLotTransfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                LoadPrinters();
                txtLotNumber.Focus();

                if (ddlPrinters.Items.Count < 1)
                {
                    lblMessage.Text = "No printers currently configured.  Please configure a printer before proceeding.";
                    btnSearch.Enabled = false;
                }
                ddlGtin.Items.Clear();
                ddlGtin.Items.AddRange(new []{ new ListItem("HDMP004", "10000001200048"), new ListItem("HDMB006", "10000002240067"), new ListItem("HMFE6", "10000003000066") });
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bool blnFound = false;
            bool blnClosed = false;
            bool blnTransferred = false;
            string strTotalCases = "";
            string strSamplePouches = "";

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

            #region Check if Lot Number exists and isn't closed
            // Read Lot information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblLots WHERE [Barcode]='" + txtLotNumber.Text.Trim() + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    blnFound = true;
                    blnTransferred = (dr["Transferred"].ToString()=="True");
                    blnClosed = (dr["Closed"].ToString() == "True");
                    strTotalCases = dr["TotalCases"].ToString();
                    strSamplePouches = dr["SamplePouches"].ToString();
                    lblExpDate.Text = DateTime.Parse(dr["BestByDate"].ToString()).ToString("yyMMdd");
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
                lblMessage.Text = "Lot Number [" + txtLotNumber.Text + "] not found.";
                txtLotNumber.Text = "";
                txtLotNumber.Focus();
                return;
            }

            // Check if Lot is closed
            if (blnClosed == true)
            {
                lblMessage.Text = "Lot Number [" + txtLotNumber.Text + "] is closed.  Cannot transfer.";
                txtLotNumber.Text = "";
                txtLotNumber.Focus();
                return;
            }
            #endregion

            // Activate controls and submit button
            btnSearch.Visible = false;
            txtLotNumber.Enabled = false;
            if (blnTransferred == false)
            {
                lblMessage.Text = "Lot Number [" + txtLotNumber.Text + "] found.  Click 'Transfer' to transfer lot.";
                btnSubmit.Visible = true;
                txtTotalCases.Enabled = true;
                txtSamplePouches.Enabled = true;
                txtTotalCases.Focus();
            }
            else
            {
                lblMessage.Text = "Lot Number [" + txtLotNumber.Text + "] already transferred.  Click 'Print' to print pouch labels.";
                txtTotalCases.Text = strTotalCases;
                txtSamplePouches.Text = strSamplePouches;
                lblSelectPrinter.Visible = true;
                ddlPrinters.Visible = true;
                btnPrint.Visible = true;
                btnNext.Visible = true;
                lblExpDate.Visible = true;
                lblExpDateTitle.Visible = true;
                lblStartingPouchNumber.Visible = true;
                lblQuantity.Visible = true;
                txtStartingPouchNumber.Visible = true;
                txtQuantity.Visible = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Int32 intTotalCases = 0;
            Int32 intSamplePouches = 0;
            Int32 intLotID = -1;
            Int32 intTransactionID = -1;
            String strInputBarcode = txtLotNumber.Text.Trim().Replace("'", "''");
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];

            lblMessage.Text = "";
            
            #region Validate Input
            if (Int32.TryParse(txtTotalCases.Text.Trim(), out intTotalCases) == false || intTotalCases <= 0)
            {
                lblMessage.Text = "Please enter a valid number of total cases.";
                txtTotalCases.Text = "";
                txtTotalCases.Focus();
                return;
            }
            if (Int32.TryParse(txtSamplePouches.Text.Trim(), out intSamplePouches) == false || intSamplePouches < 0)
            {
                lblMessage.Text = "Please enter a valid number of sample pouches.";
                txtSamplePouches.Text = "";
                txtSamplePouches.Focus();
                return;
            }
            if (intSamplePouches > 47)
            {
                lblMessage.Text = "The number of sample pouches cannot equal or exceed 48.";
                txtSamplePouches.Text = "";
                txtSamplePouches.Focus();
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
                sqlCommand.CommandText = "UPDATE tblLots SET [Transferred]=1,[TotalCases]=" + intTotalCases.ToString() + ",[CasesRemaining]=" + intTotalCases.ToString() + ",[SamplePouches]=" + intSamplePouches.ToString() + " output INSERTED.ID WHERE [Barcode]='" + strInputBarcode + "'";
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

            #region Setup Transaction Fields/Values
            strFields.Add("Transferred");
            strValues.Add("True");
            strFields.Add("Total Cases");
            strValues.Add(intTotalCases.ToString());
            strFields.Add("Cases Remaining");
            strValues.Add(intTotalCases.ToString());
            strFields.Add("Sample Pouches");
            strValues.Add(intSamplePouches.ToString());
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Transfer Lot" + "','" +
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

            // Enable pouch printing
            lblMessage.Text = "Lot successfully transferred. Verify selected product then click 'Print' to print pouch labels.";
            txtTotalCases.Enabled = false;
            txtSamplePouches.Enabled = false;
            btnSubmit.Visible = false;
            lblSelectPrinter.Visible = true;
            ddlPrinters.Visible = true;
            btnPrint.Visible = true;
            btnNext.Visible = true;
            lblExpDate.Visible = true;
            lblExpDateTitle.Visible = true;
            lblStartingPouchNumber.Visible = true;
            lblQuantity.Visible = true;
            txtStartingPouchNumber.Visible = true;
            txtQuantity.Visible = true;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        void ClearForm()
        {
            lblMessage.Text = "";
            txtLotNumber.Text = "";
            txtSamplePouches.Text = "";
            txtTotalCases.Text = "";
            lblExpDate.Text = "";
            txtQuantity.Text = "1";
            txtStartingPouchNumber.Text = "1";
            btnSearch.Visible = true;
            lblSelectPrinter.Visible = false;
            ddlPrinters.Visible = false;
            btnPrint.Visible = false;
            btnNext.Visible = false;
            lblExpDate.Visible = false;
            lblExpDateTitle.Visible = false;
            lblStartingPouchNumber.Visible = false;
            lblQuantity.Visible = false;
            txtStartingPouchNumber.Visible = false;
            txtQuantity.Visible = false;
            txtLotNumber.Enabled = true;
            txtLotNumber.Focus();
            ddlGtin.SelectedIndex = 0;
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            #region Validate Quantity and Starting Pouch Number
            // Check quantity
            bool quantitySet = int.TryParse(txtQuantity.Text, out int printQuantity);
            if (!quantitySet)
            {
                lblMessage.Text = "Invalid quantity entered.";
                txtQuantity.Focus();
                return;
            }

            if (printQuantity <= 0)
            {
                lblMessage.Text = "Quantity must be greater than 0.";
                txtQuantity.Focus();
                return;
            }

            // Check Starting Pouch Number
            bool pouchStartSet = int.TryParse(txtStartingPouchNumber.Text, out int pouchStart);
            if (!pouchStartSet)
            {
                lblMessage.Text = "Invalid Starting Pouch Number entered.";
                txtStartingPouchNumber.Focus();
                return;
            }

            if (pouchStart <= 0)
            {
                lblMessage.Text = "Starting Pouch Number must be greater than 0.";
                txtStartingPouchNumber.Focus();
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

            #region Get Printer Information
            string strPrinterIP = null;
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

            if (string.IsNullOrEmpty(strPrinterIP))
            {
                lblMessage.Text = "Select valid printer.";
                ddlPrinters.Focus();
                return;
            }
            #endregion

            for (int i = 0; i < printQuantity; i++)
            {
                string strLabelFormat = "^XA" + Environment.NewLine;
                strLabelFormat += "^MD10" + Environment.NewLine;
                strLabelFormat += "^LH0,0" + Environment.NewLine;
                strLabelFormat += "^PR2" + Environment.NewLine;
                strLabelFormat += "^PW525" + Environment.NewLine;
                strLabelFormat += "^FO180,40^BXN,6,200,26,26,6,_,1^FD_101" + ddlGtin.SelectedValue + "17" + lblExpDate.Text + "10" + txtLotNumber.Text.Trim() + "_191" + pouchStart + "^FS" + Environment.NewLine;
                strLabelFormat += "^PQ1" + Environment.NewLine;
                strLabelFormat += "^XZ" + Environment.NewLine;

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

                pouchStart += 1;
            }
            lblMessage.Text = "Label(s) printed successfully.";
        }

    }
}