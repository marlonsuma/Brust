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
    public partial class frmInventoryProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                divFirst.Visible = false;
                divSecond.Visible = false;
                divThird.Visible = false;
                lblFirstTitle.Visible = false;
                lblSecondTitle.Visible = false;
                lblThirdTitle.Visible = false;
                txtBarcode.Attributes.Add("onfocus", "this.select()");
                txtBarcode.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String strBarcode = txtBarcode.Text.Trim().Replace("'", "''");
            String strItemType = "";
            decimal decTotalOunces = 0;

            // Clear the message
            lblMessage.Text = "";

            ClearForm();

            #region Validate Input
            // Validate Input and Set Scan Type
            if (strBarcode.Length <= 0)
            {
                txtBarcode.Focus();
                return;
            }
            else if (strBarcode.StartsWith("MK") == true && strBarcode.Length == 9)
            {
                strItemType = "Milk Kit";
            }
            else if (strBarcode.StartsWith("DK") == true && strBarcode.Length == 9)
            {
                strItemType = "DNA Kit";
            }
            else if (strBarcode.StartsWith("LT") == true && strBarcode.Length == 9)
            {
                strItemType = "Lot";
            }
            else if (strBarcode.StartsWith("CA") == true && strBarcode.Length == 9)
            {
                strItemType = "Case";
            }
            else if (strBarcode.StartsWith("=") == true && strBarcode.Length == 16)
            {
                strItemType = "Blood Kit";
            }
            else
            {
                strItemType = "Donor";
            }
            #endregion
            
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());

            // Build form based on type of scan
            if (strItemType == "Donor")
            {
                #region Blood Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DIN] AS [Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[Status] AS [Passed] FROM tblBloodKits WHERE [Active]=1 AND [DonorID]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Blood Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion

                #region Milk Kits
                SqlCommand objCommand2;
                objCommand2 = new SqlCommand(@"SELECT [tblMilkKits].[Barcode],[tblLots].[Barcode] AS [Lot],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial] FROM [tblMilkKits] LEFT JOIN [tblLots] ON [tblMilkKits].[LotID]=[tblLots].[ID] WHERE [tblMilkKits].[DonorID]='" + strBarcode + "' AND [Active]=1 ORDER BY [tblMilkKits].[ID]", objConn);
                SqlDataAdapter da2 = new SqlDataAdapter(objCommand2);
                DataTable dt2 = new DataTable();
                DataRow dr2 = dt2.NewRow();

                // Make dummy row so that footer always shows
                dr2 = dt2.NewRow();
                dt2.Rows.Add(dr2);
                da2.Fill(dt2);

                dgvSecond.DataSource = dt2;
                dgvSecond.DataBind();

                // Hide dummy row
                dgvSecond.Rows[0].Visible = false;

                // Loop through kits adding up total ounces
                decimal decKitVolume = 0;
                for (int i = 0; i < dgvSecond.Rows.Count; i++)
                {
                    if (decimal.TryParse(dgvSecond.Rows[i].Cells[6].Text, out decKitVolume) == false)
                    {
                        decKitVolume = 0;
                    }
                    decTotalOunces += decKitVolume;
                }

                // Show Section
                lblSecondTitle.Text = "Milk Kits - Total Ounces: " + decTotalOunces.ToString();
                lblSecondTitle.Visible = true;
                divSecond.Visible = true;
                #endregion

                #region DNA Kits
                SqlCommand objCommand3;
                objCommand3 = new SqlCommand(@"SELECT [Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number] FROM tblDNAKits WHERE [Active]=1 AND [DonorID]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da3 = new SqlDataAdapter(objCommand3);
                DataTable dt3 = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr3 = dt3.NewRow();
                dt3.Rows.Add(dr3);
                da3.Fill(dt3);

                dgvThird.DataSource = dt3;
                dgvThird.DataBind();

                // Hide dummy row
                dgvThird.Rows[0].Visible = false;

                // Show Section
                lblThirdTitle.Text = "DNA Kits";
                lblThirdTitle.Visible = true;
                divThird.Visible = true;
                #endregion
            }
            else if (strItemType == "Milk Kit")
            {
                #region Milk Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DonorID] AS [Donor ID],[tblLots].[Barcode] AS [Lot],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial],[Active] FROM [tblMilkKits] LEFT JOIN [tblLots] ON [tblMilkKits].[LotID]=tblLots.[ID] WHERE [tblMilkKits].[Barcode]='" + strBarcode + "' ORDER BY [tblMilkKits].[ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();

                // Make dummy row so that footer always shows
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Milk Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }
            else if (strItemType == "Blood Kit")
            {
                #region Blood Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DonorID] AS [Donor ID],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[Status] AS [Passed],[Active] FROM tblBloodKits WHERE [DIN]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Blood Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }
            else if (strItemType == "DNA Kit")
            {
                #region DNA Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DonorID] AS [Donor ID],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[Active] FROM tblDNAKits WHERE [Barcode]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "DNA Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }
            else if (strItemType == "Lot")
            {
                #region Lots
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [BestByDate] AS [Best By Date],[Closed],[Transferred],[TotalCases] AS [Total Cases],[CasesRemaining] AS [Cases Remaining],[SamplePouches] AS [Sample Pouches] FROM [tblLots] WHERE [Barcode]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Lot Information";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion

                #region Included Milk Kits
                SqlCommand objCommand2;
                objCommand2 = new SqlCommand(@"SELECT [tblMilkKits].[Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial] FROM [tblMilkKits] INNER JOIN [tblLots] ON [tblMilkKits].[LotID]=[tblLots].[ID] WHERE [tblMilkKits].[Active]=1 AND [tblLots].[Barcode]='" + strBarcode + "' ORDER BY [tblMilkKits].[ID]", objConn);
                SqlDataAdapter da2 = new SqlDataAdapter(objCommand2);
                DataTable dt2 = new DataTable();
                DataRow dr2 = dt2.NewRow();

                // Make dummy row so that footer always shows
                dr2 = dt2.NewRow();
                dt2.Rows.Add(dr2);
                da2.Fill(dt2);

                dgvSecond.DataSource = dt2;
                dgvSecond.DataBind();

                // Hide dummy row
                dgvSecond.Rows[0].Visible = false;

                // Loop through kits adding up total ounces
                decimal decKitVolume = 0;
                for (int i = 0; i < dgvSecond.Rows.Count; i++)
                {
                    if (decimal.TryParse(dgvSecond.Rows[i].Cells[5].Text, out decKitVolume) == false)
                    {
                        decKitVolume = 0;
                    }
                    decTotalOunces += decKitVolume;
                }

                // Show Section
                lblSecondTitle.Text = "Included Milk Kits - Total Ounces: " + decTotalOunces.ToString();
                lblSecondTitle.Visible = true;
                divSecond.Visible = true;
                #endregion

                #region Cases
                SqlCommand objCommand3;
                objCommand3 = new SqlCommand(@"SELECT [tblCases].[Barcode] AS [Barcode],[tblLocations].[Name] AS [Location],[ShipDate] AS [Ship Date],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[PONumber] AS [PO Number],[CaseQuantity] AS [Cases Shipped] FROM [tblCases] INNER JOIN [tblLots] ON [tblCases].[LotID]=[tblLots].[ID] INNER JOIN [tblLocations] ON [tblCases].[LocationID]=[tblLocations].[ID] WHERE [tblCases].[Active]=1 AND [tblLots].[Barcode]='" + strBarcode + "' ORDER BY [tblCases].[ID]", objConn);
                SqlDataAdapter da3 = new SqlDataAdapter(objCommand3);
                DataTable dt3 = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr3 = dt3.NewRow();
                dt3.Rows.Add(dr3);
                da3.Fill(dt3);

                dgvThird.DataSource = dt3;
                dgvThird.DataBind();

                // Hide dummy row
                dgvThird.Rows[0].Visible = false;

                // Show Section
                lblThirdTitle.Text = "Case Information";
                lblThirdTitle.Visible = true;
                divThird.Visible = true;
                #endregion
            }
            else if (strItemType == "Case")
            {
                #region Cases
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [tblLocations].[Name] AS [Location],[tblLots].[Barcode] AS [Lot],[ShipDate] AS [Ship Date],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[PONumber] AS [PO Number],[CaseQuantity] AS [Case Quantity],[Active] FROM [tblCases] INNER JOIN [tblLots] ON [tblCases].[LotID]=[tblLots].[ID] INNER JOIN [tblLocations] ON [tblCases].[LocationID]=[tblLocations].[ID] WHERE [tblCases].[Barcode]='" + strBarcode + "' ORDER BY [tblCases].[ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Case Information";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }

            // Clear form and display error if no records found
            if (dgvFirst.Rows.Count <= 1 && dgvSecond.Rows.Count <= 1 && dgvThird.Rows.Count <= 1)
            {
                ClearForm();
                lblMessage.Text = "No records found with ID [" + strBarcode + "].";
            }

            txtBarcode.Focus();
        }

        private void ClearForm()
        {
            // Clear the form
            divFirst.Visible = false;
            divSecond.Visible = false;
            divThird.Visible = false;
            lblFirstTitle.Visible = false;
            lblSecondTitle.Visible = false;
            lblThirdTitle.Visible = false;
        }

        protected void btnSearchNames_Click(object sender, EventArgs e)
        {
            String strFirstName = txtFirstNameSearch.Text;
            String strLastName = txtLastNameSearch.Text;
            String strDonorId = "";
            String strMilkKitId = "";
            String strItemType = "";
            String strBarcode = "";
            decimal decTotalOunces = 0;

            // Clear the message
            lblMessage.Text = "";

            ClearForm();

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            #endregion

            #region Validate Input
            // Validate Input and Set Scan Type
            if (strFirstName.Length <= 0 || strLastName.Length <= 0)
            {
                txtFirstNameSearch.Focus();
                txtLastNameSearch.Focus();
                lblMessage.Text = "Please make sure to either type in the first and last name in the text box provided.";
                return;
            }

            #endregion

            #region Finding the Item Type from First and Last name
            try
            {
                conn.Open();
                sqlCommand.CommandText = (@"SELECT [DonorId] FROM [tblDonors] WHERE FirstName='" + strFirstName + "' AND LastName='" + strLastName + "'");
                sqlCommand.ExecuteNonQuery();

                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strDonorId = dr["DonorId"].ToString();
                }
                dr.Dispose();

                sqlCommand.CommandText = (@"SELECT [ID] FROM [tblMilkKits] Where DonorID = '" +strDonorId+ "'");
                sqlCommand.ExecuteNonQuery();

                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strMilkKitId = dr["ID"].ToString();
                }
                dr.Dispose();

                sqlCommand.CommandText = (@"SELECT [ItemType] FROM [tblTransactions] INNER JOIN [tblMilkKits] on tblTransactions.ItemID=tblMilkKits.ID WHERE ItemID = '" + strMilkKitId + "'");
                sqlCommand.ExecuteNonQuery();

                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strItemType = dr["ItemType"].ToString();
                }
                dr.Dispose();

                sqlCommand.CommandText = (@"SELECT [Barcode] FROM [tblMilkKits] WHERE ID= '" + strMilkKitId + "'");
                sqlCommand.ExecuteNonQuery();

                dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    strBarcode = dr["Barcode"].ToString();
                }
                dr.Dispose();

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

            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());

            // Build form based on type of scan
            if (strItemType == "Donor")
            {
                #region Blood Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DIN] AS [Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[Status] AS [Passed] FROM tblBloodKits WHERE [Active]=1 AND [DonorID]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow drb = dt.NewRow();
                dt.Rows.Add(drb);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Blood Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion

                #region Milk Kits
                SqlCommand objCommand2;
                objCommand2 = new SqlCommand(@"SELECT [tblMilkKits].[Barcode],[tblLots].[Barcode] AS [Lot],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial] FROM [tblMilkKits] LEFT JOIN [tblLots] ON [tblMilkKits].[LotID]=[tblLots].[ID] WHERE [tblMilkKits].[DonorID]='" + strBarcode + "' AND [Active]=1 ORDER BY [tblMilkKits].[ID]", objConn);
                SqlDataAdapter da2 = new SqlDataAdapter(objCommand2);
                DataTable dt2 = new DataTable();
                DataRow dr2 = dt2.NewRow();

                // Make dummy row so that footer always shows
                dr2 = dt2.NewRow();
                dt2.Rows.Add(dr2);
                da2.Fill(dt2);

                dgvSecond.DataSource = dt2;
                dgvSecond.DataBind();

                // Hide dummy row
                dgvSecond.Rows[0].Visible = false;

                // Loop through kits adding up total ounces
                decimal decKitVolume = 0;
                for (int i = 0; i < dgvSecond.Rows.Count; i++)
                {
                    if (decimal.TryParse(dgvSecond.Rows[i].Cells[6].Text, out decKitVolume) == false)
                    {
                        decKitVolume = 0;
                    }
                    decTotalOunces += decKitVolume;
                }

                // Show Section
                lblSecondTitle.Text = "Milk Kits - Total Ounces: " + decTotalOunces.ToString();
                lblSecondTitle.Visible = true;
                divSecond.Visible = true;
                #endregion

                #region DNA Kits
                SqlCommand objCommand3;
                objCommand3 = new SqlCommand(@"SELECT [Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number] FROM tblDNAKits WHERE [Active]=1 AND [DonorID]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da3 = new SqlDataAdapter(objCommand3);
                DataTable dt3 = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr3 = dt3.NewRow();
                dt3.Rows.Add(dr3);
                da3.Fill(dt3);

                dgvThird.DataSource = dt3;
                dgvThird.DataBind();

                // Hide dummy row
                dgvThird.Rows[0].Visible = false;

                // Show Section
                lblThirdTitle.Text = "DNA Kits";
                lblThirdTitle.Visible = true;
                divThird.Visible = true;
                #endregion
            }
            else if (strItemType == "Milk Kit")
            {
                #region Milk Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DonorID] AS [Donor ID],[tblLots].[Barcode] AS [Lot],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial],[Active] FROM [tblMilkKits] LEFT JOIN [tblLots] ON [tblMilkKits].[LotID]=tblLots.[ID] WHERE [tblMilkKits].[Barcode]='" + strBarcode + "' ORDER BY [tblMilkKits].[ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();
                DataRow drm = dt.NewRow();

                // Make dummy row so that footer always shows
                drm = dt.NewRow();
                dt.Rows.Add(drm);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Milk Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }
            else if (strItemType == "Blood Kit")
            {
                #region Blood Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DonorID] AS [Donor ID],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[Status] AS [Passed],[Active] FROM tblBloodKits WHERE [DIN]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow drbk = dt.NewRow();
                dt.Rows.Add(drbk);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Blood Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }
            else if (strItemType == "DNA Kit")
            {
                #region DNA Kits
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [DonorID] AS [Donor ID],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[Active] FROM tblDNAKits WHERE [Barcode]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow drdna = dt.NewRow();
                dt.Rows.Add(drdna);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "DNA Kits";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }
            else if (strItemType == "Lot")
            {
                #region Lots
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [BestByDate] AS [Best By Date],[Closed],[Transferred],[TotalCases] AS [Total Cases],[CasesRemaining] AS [Cases Remaining],[SamplePouches] AS [Sample Pouches] FROM [tblLots] WHERE [Barcode]='" + strBarcode + "' ORDER BY [ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow drl = dt.NewRow();
                dt.Rows.Add(drl);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Lot Information";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion

                #region Included Milk Kits
                SqlCommand objCommand2;
                objCommand2 = new SqlCommand(@"SELECT [tblMilkKits].[Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial] FROM [tblMilkKits] INNER JOIN [tblLots] ON [tblMilkKits].[LotID]=[tblLots].[ID] WHERE [tblMilkKits].[Active]=1 AND [tblLots].[Barcode]='" + strBarcode + "' ORDER BY [tblMilkKits].[ID]", objConn);
                SqlDataAdapter da2 = new SqlDataAdapter(objCommand2);
                DataTable dt2 = new DataTable();
                DataRow dr2 = dt2.NewRow();

                // Make dummy row so that footer always shows
                dr2 = dt2.NewRow();
                dt2.Rows.Add(dr2);
                da2.Fill(dt2);

                dgvSecond.DataSource = dt2;
                dgvSecond.DataBind();

                // Hide dummy row
                dgvSecond.Rows[0].Visible = false;

                // Loop through kits adding up total ounces
                decimal decKitVolume = 0;
                for (int i = 0; i < dgvSecond.Rows.Count; i++)
                {
                    if (decimal.TryParse(dgvSecond.Rows[i].Cells[5].Text, out decKitVolume) == false)
                    {
                        decKitVolume = 0;
                    }
                    decTotalOunces += decKitVolume;
                }

                // Show Section
                lblSecondTitle.Text = "Included Milk Kits - Total Ounces: " + decTotalOunces.ToString();
                lblSecondTitle.Visible = true;
                divSecond.Visible = true;
                #endregion

                #region Cases
                SqlCommand objCommand3;
                objCommand3 = new SqlCommand(@"SELECT [tblCases].[Barcode] AS [Barcode],[tblLocations].[Name] AS [Location],[ShipDate] AS [Ship Date],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[PONumber] AS [PO Number],[CaseQuantity] AS [Cases Shipped] FROM [tblCases] INNER JOIN [tblLots] ON [tblCases].[LotID]=[tblLots].[ID] INNER JOIN [tblLocations] ON [tblCases].[LocationID]=[tblLocations].[ID] WHERE [tblCases].[Active]=1 AND [tblLots].[Barcode]='" + strBarcode + "' ORDER BY [tblCases].[ID]", objConn);
                SqlDataAdapter da3 = new SqlDataAdapter(objCommand3);
                DataTable dt3 = new DataTable();

                // Make dummy row so that footer always shows
                DataRow dr3 = dt3.NewRow();
                dt3.Rows.Add(dr3);
                da3.Fill(dt3);

                dgvThird.DataSource = dt3;
                dgvThird.DataBind();

                // Hide dummy row
                dgvThird.Rows[0].Visible = false;

                // Show Section
                lblThirdTitle.Text = "Case Information";
                lblThirdTitle.Visible = true;
                divThird.Visible = true;
                #endregion
            }
            else if (strItemType == "Case")
            {
                #region Cases
                SqlCommand objCommand;
                objCommand = new SqlCommand(@"SELECT [tblLocations].[Name] AS [Location],[tblLots].[Barcode] AS [Lot],[ShipDate] AS [Ship Date],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[PONumber] AS [PO Number],[CaseQuantity] AS [Case Quantity],[Active] FROM [tblCases] INNER JOIN [tblLots] ON [tblCases].[LotID]=[tblLots].[ID] INNER JOIN [tblLocations] ON [tblCases].[LocationID]=[tblLocations].[ID] WHERE [tblCases].[Barcode]='" + strBarcode + "' ORDER BY [tblCases].[ID]", objConn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                DataTable dt = new DataTable();

                // Make dummy row so that footer always shows
                DataRow drc = dt.NewRow();
                dt.Rows.Add(drc);
                da.Fill(dt);

                dgvFirst.DataSource = dt;
                dgvFirst.DataBind();

                // Hide dummy row
                dgvFirst.Rows[0].Visible = false;

                // Show Section
                lblFirstTitle.Text = "Case Information";
                lblFirstTitle.Visible = true;
                divFirst.Visible = true;
                #endregion
            }

            // Clear form and display error if no records found
            if (dgvFirst.Rows.Count <= 1 && dgvSecond.Rows.Count <= 1 && dgvThird.Rows.Count <= 1)
            {
                ClearForm();
                lblMessage.Text = "No records found with first name [" + strFirstName + "] and last name ["+ strLastName + "]. Please try again.";
            }

            txtBarcode.Focus();
        }
    }
}