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
    public partial class frmCreateMilkKit : System.Web.UI.Page
    {
        public IDonorRepository DonorRepository { get; set; }
        public IMilkKitRepository milkKitRepository { get; set; }

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

        void ClearForm()
        {
            lblMessage.Text = "";
            lblLabelInfo.Visible = false;
            lblMilkKitID.Visible = false;
            lblMilkKitIDTitle.Visible = false;
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            string strNewBarcode = "";
            bool blnFound = false;
            int intInsertedID = -1;
            int intTransactionID = -1;
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            string donorId = txtDonorNumber.Text.Trim();
            string trackingNo = txtTrackingNumber.Text.Trim();

            #region Validate Input
            // Make sure tracking number is entered
            if (txtTrackingNumber.Text.Trim().Length <= 0)
            {
                lblMessage.Text = "Please enter a valid tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            // Check Donor Number Length
            if (txtDonorNumber.Text.Trim().Length <= 0)
            {
                lblMessage.Text = "Invalid entry [" + txtDonorNumber.Text + "]. Please enter a valid Donor Number.";
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

            #region Check if DIN has passed blood tests within 6 months
            try
            {
                conn.Open();
                sqlCommand.CommandText = "DECLARE @testDate DateTime";
                sqlCommand.CommandText += " SET @testDate='" + DateTime.Now.Subtract(TimeSpan.FromDays(180)).ToShortDateString() + " 00:00:00' ";
                sqlCommand.CommandText += "SELECT * FROM tblBloodKits WHERE [DonorID]='" + txtDonorNumber.Text.Trim() + "' AND [Status]='True' AND [Active]='True' AND [ReceiveDate] >= @testDate";
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

            if (!blnFound)
            {
                lblMessage.Text = "Donor has not passed blood tests within 180 days.  Cannot proceed.";
                txtDonorNumber.Text = "";
                txtDonorNumber.Focus();
                return;
            }
            #endregion

            #region Create Milk Kit
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblMilkKits ([DonorID],[ShippingService],[TrackingNumber]) output INSERTED.ID VALUES('" +
                                            txtDonorNumber.Text.Trim().Replace("'","''") + "','" +
                                            ddlShipping.Text + "','" +
                                            txtTrackingNumber.Text.Trim().Replace("'", "''") + "')";
                intInsertedID = (int)sqlCommand.ExecuteScalar();
                strNewBarcode = "MK" + intInsertedID.ToString().PadLeft(7, '0');
                sqlCommand.CommandText = "UPDATE tblMilkKits SET [Barcode]='" + strNewBarcode + "' WHERE [ID]=" + intInsertedID;
                sqlCommand.ExecuteNonQuery();

                //Have no clue what this was intended for.
               // sqlCommand.CommandText = "INSERT INTO tblDonors (DonorId) VALUES ('" + txtDonorNumber.Text + "');";
                //sqlCommand.ExecuteNonQuery();


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
            strFields.Add("Barcode");
            strValues.Add(strNewBarcode);
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
                                            "Create Milk Kit" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Milk Kit" + "'," +
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
            if(intInsertedID!=0)
            {
                var donorid = milkKitRepository.Get(strNewBarcode).DonorId;
                string toemail, body, Subject;
                Subject = "Milk Kit Created";
                toemail = DonorRepository.Get(donorid).Email;
                body = " Ni-Q has shipped your milk kit. It may take up to 7 days for FedEx to deliver the milk kit.";
                if (!string.IsNullOrEmpty(toemail) && toemail != "")
                    EMailHelper.SendEmail(toemail, body, Subject);
            }
            lblMessage.Text = "Milk Kit successfully created.  Select a printer and click 'Print Label' to generate labels.";

            lblLabelInfo.Visible = true;
            lblMilkKitID.Visible = true;
            lblMilkKitIDTitle.Visible = true;
            lblMilkKitID.Text = strNewBarcode;

            lblSelectPrinter.Visible = true;
            ddlPrinters.Visible = true;
            btnPrint.Visible = true;
            btnCreate.Enabled = false;
            txtDonorNumber.Enabled = false;
            ddlShipping.Enabled = false;
            txtTrackingNumber.Enabled = false;
            btnNext.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ClearForm();
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
            strLabelFormat += "^FO100,50^BCN,110,N,N,N,A^FD" + lblMilkKitID.Text + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO200,175^A0N,30,30^FD" + lblMilkKitID.Text + "^FS" + Environment.NewLine;
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

            #region Print Label 1
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

            #region Print Label 2
            // Print to IP Address
            TcpClient tcpSocket2 = new TcpClient();
            try
            {
                // Connect to the printer
                tcpSocket2.Connect(strPrinterIP, Global.PrinterInfo.PORT);
                NetworkStream nsPrint2 = tcpSocket2.GetStream();
                StreamWriter swPrint2 = new StreamWriter(nsPrint2);

                // Start sending
                swPrint2.WriteLine(strLabelFormat);

                swPrint2.Flush();
                swPrint2.Close();
                nsPrint2.Close();
                tcpSocket2.Close();
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