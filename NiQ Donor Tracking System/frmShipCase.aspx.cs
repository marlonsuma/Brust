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
    public partial class frmShipCase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                LoadRootLocations();
                LoadLots();
                LoadPrinters();

                if (treeLocations.Nodes.Count <= 0)
                {
                    lblMessage.Text = "No locations currently configured.  Please add locations via the Administrator options.";
                    btnShip.Enabled = false;
                }
                else
                {
                    // Default to top selection
                    treeLocations.Nodes[0].Select();
                }
            }
        }

        private void LoadPrinters()
        {
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

        private void LoadLots()
        {
            ddlLotNumber.Items.Clear();

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            #endregion

            #region Get all Lots that are transferred but not closed
            // Read Lot information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblLots WHERE [Transferred]='True' AND [Closed]='False'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    ddlLotNumber.Items.Add(dr["Barcode"].ToString());
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

            if (ddlLotNumber.Items.Count < 1)
            {
                btnShip.Enabled = false;
                ddlLotNumber.Enabled = false;
                lblMessage.Text = "No lots available for shipping.  Please transfer a lot prior to shipping cases.";
            }
        }

        private void LoadRootLocations()
        {
            treeLocations.Nodes.Clear();
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
            SqlCommand objCommand = new SqlCommand(@"SELECT * FROM tblLocations WHERE [ParentID]=0 AND [Active]=1", objConn);
            SqlDataAdapter da = new SqlDataAdapter(objCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            PopulateNodes(dt, treeLocations.Nodes);
        }

        private void PopulateSubLevel(int intParentID, TreeNode parentNode)
        {
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
            SqlCommand objCommand = new SqlCommand(@"SELECT * FROM tblLocations WHERE [ParentID]=" + intParentID.ToString() + " AND [Active]=1", objConn);
            SqlDataAdapter da = new SqlDataAdapter(objCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            PopulateNodes(dt, parentNode.ChildNodes);
        }

        protected void treeLocations_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            PopulateSubLevel(Int32.Parse(e.Node.Value.ToString()), e.Node);
        }

        private void PopulateNodes(DataTable dt, TreeNodeCollection nodes)
        {
            foreach (DataRow dr in dt.Rows)
            {
                TreeNode tn = new TreeNode();
                tn.Text = dr["Name"].ToString();
                tn.Value = dr["ID"].ToString();
                nodes.Add(tn);

                //If node has child nodes, then enable on-demand populating
                tn.PopulateOnDemand = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnShip_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            Int32 intNumberofCases = 0;
            Int32 intRemainingCases = 0;
            Int32 intInsertedID = -1;
            int intTransactionID = -1;
            String strNewBarcode = "";
            String strShipDate = DateTime.Now.ToString();
            String strLotID = "";
            StringCollection strFields = new StringCollection();
            StringCollection strValues = new StringCollection();
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];

            #region Validate Input
            if (treeLocations.SelectedNode == null || treeLocations.SelectedValue == null)
            {
                lblMessage.Text = "Please select a location to ship to.";
                return;
            }

            if (ddlLotNumber.SelectedIndex < 0)
            {
                lblMessage.Text = "Please select a lot for the case.";
                return;
            }

            if (txtTrackingNumber.Text.Trim() == "")
            {
                lblMessage.Text = "Please enter a valid tracking number.";
                txtTrackingNumber.Focus();
                return;
            }

            if (txtPONumber.Text.Trim() == "")
            {
                lblMessage.Text = "Please enter a valid PO Number.";
                txtPONumber.Focus();
                return;
            }

            if (Int32.TryParse(txtNumberofCases.Text.Trim(), out intNumberofCases) == false)
            {
                lblMessage.Text = "Please enter a valid number of cases.";
                txtNumberofCases.Text = "";
                txtNumberofCases.Focus();
                return;
            }
            if (intNumberofCases <= 0)
            {
                lblMessage.Text = "Please enter a valid number of cases.";
                txtNumberofCases.Text = "";
                txtNumberofCases.Focus();
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

            #region Get Lot Information from Database
            // Read Lot information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblLots WHERE [Barcode]='" + ddlLotNumber.Text + "'";
                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strLotID = dr["ID"].ToString();
                    intRemainingCases = Int32.Parse(dr["CasesRemaining"].ToString());
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

            // Make sure there are enough remaining cases for shipment
            if (intNumberofCases > intRemainingCases)
            {
                lblMessage.Text = "Unable to ship " + intNumberofCases.ToString() + " cases from lot.  Only " + intRemainingCases + " left in lot.";
                conn.Dispose();
                txtNumberofCases.Focus();
                return;
            }

            #region Create Case
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblCases ([LocationID],[LotID],[ShipDate],[ShippingService],[TrackingNumber],[PONumber],[CaseQuantity]) output INSERTED.ID VALUES('" +
                                            treeLocations.SelectedValue.ToString() + "'," +
                                            strLotID + ",'" +
                                            strShipDate + "','" +
                                            ddlShipping.Text + "','" +
                                            txtTrackingNumber.Text.Trim().Replace("'","''") + "','" +
                                            txtPONumber.Text.Trim().Replace("'", "''") + "'," +
                                            intNumberofCases.ToString() + ")";
                intInsertedID = (int)sqlCommand.ExecuteScalar();
                strNewBarcode = "CA" + intInsertedID.ToString().PadLeft(7, '0');
                sqlCommand.CommandText = "UPDATE tblCases SET [Barcode]='" + strNewBarcode + "' WHERE [ID]=" + intInsertedID;
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

            #region Update Lot (Close if no cases remain to be shipped)
            try
            {
                conn.Open();
                if ((intRemainingCases - intNumberofCases) == 0)
                {
                    sqlCommand.CommandText = "UPDATE tblLots SET [CasesRemaining]='" + (intRemainingCases - intNumberofCases).ToString() + "',[Closed]=1 WHERE [ID]=" + strLotID;
                }

                else
                {
                    sqlCommand.CommandText = "UPDATE tblLots SET [CasesRemaining]='" + (intRemainingCases - intNumberofCases).ToString() + "' WHERE [ID]=" + strLotID;
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
            strFields.Add("Lot ID");
            strValues.Add(strLotID);
            strFields.Add("Ship Date");
            strValues.Add(strShipDate);
            strFields.Add("Shipping Service");
            strValues.Add(ddlShipping.Text);
            strFields.Add("Tracking Number");
            strValues.Add(txtTrackingNumber.Text.Trim().Replace("'", "''"));
            strFields.Add("PO Number");
            strValues.Add(txtPONumber.Text.Trim().Replace("'", "''"));
            strFields.Add("Cases Shipped");
            strValues.Add(intNumberofCases.ToString());
            strFields.Add("Cases Remaining");
            strValues.Add((intRemainingCases - intNumberofCases).ToString());
            if ((intRemainingCases - intNumberofCases) == 0)
            {
                strFields.Add("Closed");
                strValues.Add("True");
            }
            #endregion

            #region Create Transaction
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblTransactions ([TransactionType],[TransactionDate],[TransactionUser],[ItemType],[ItemID]) output INSERTED.ID VALUES('" +
                                            "Create Case" + "','" +
                                            DateTime.Now + "','" +
                                            ui.ID + "','" +
                                            "Case" + "'," +
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

            // Disable entry
            lblMessage.Text = "Case(s) successfully created and shipped.  Select a printer and click 'Print Label' to print.";
            btnShip.Enabled = false;
            ddlShipping.SelectedIndex = 0;
            ddlShipping.Enabled = false;
            txtTrackingNumber.Enabled = false;
            txtPONumber.Enabled = false;
            ddlLotNumber.Enabled = false;
            treeLocations.Enabled = false;
            txtNumberofCases.Enabled = false;

            // Show Print Section
            lblCaseID.Text = strNewBarcode;
            lblLabelInfo.Visible = true;
            lblCaseID.Visible = true;
            lblCaseIDTitle.Visible = true;
            lblSelectPrinter.Visible = true;
            ddlPrinters.Visible = true;
            btnNext.Visible = true;
            btnPrint.Visible = true;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        void ClearForm()
        {
            lblMessage.Text = "";
            lblLabelInfo.Visible = false;
            lblCaseID.Visible = false;
            lblCaseIDTitle.Visible = false;
            lblSelectPrinter.Visible = false;
            ddlPrinters.Visible = false;
            btnNext.Visible = false;
            btnPrint.Visible = false;

            treeLocations.Enabled = true;
            ddlLotNumber.Enabled = true;
            ddlLotNumber.SelectedIndex = 0;
            txtPONumber.Enabled = true;
            txtPONumber.Text = "";
            txtTrackingNumber.Enabled = true;
            txtTrackingNumber.Text = "";
            ddlShipping.Enabled = true;
            ddlShipping.SelectedIndex = 0;
            btnShip.Enabled = true;
            txtNumberofCases.Text = "";
            txtNumberofCases.Enabled = true;
            LoadLots();
            ddlLotNumber.Focus();
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
            strLabelFormat += "^FO100,50^BCN,110,N,N,N,A^FD" + lblCaseID.Text + "^FS" + Environment.NewLine;
            strLabelFormat += "^FO200,175^A0N,30,30^FD" + lblCaseID.Text + "^FS" + Environment.NewLine;
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