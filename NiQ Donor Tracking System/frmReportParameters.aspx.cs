using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace NiQ_Donor_Tracking_System
{
    public partial class frmReportParameters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlReportType_SelectedIndexChanged(null, null);
            }
        }

        protected void chkAllDates_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllDates.Checked == true)
            {
                calDateFrom.SelectedDates.Clear();
                calDateTo.SelectedDates.Clear();
                calDateFrom.Enabled = false;
                calDateTo.Enabled = false;
                lblDateFrom.Enabled = false;
                lblDateTo.Enabled = false;
            }
            else
            {
                calDateFrom.Enabled = true;
                calDateTo.Enabled = true;
                lblDateFrom.Enabled = true;
                lblDateTo.Enabled = true;
                calDateFrom.SelectedDate = DateTime.Today;
                calDateTo.SelectedDate = DateTime.Today;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            String strData = "";
            String strTransactionTypeFilter = ddlTransactionType.Text;
            String strUserIDFilter = "0";
            String strTransactionDateTo = "0";
            String strTransactionDateFrom = "0";
            String strItemIDFilter = "0";
            String strItemTable = "";
            String strSQLQuery = "";
            Int32 intWhere = 0;
            String strTransactionID = "";
            String strHeaderData = "";
            Int32 intRow = 0;
            Int32 intHeaderCount = 11;
            StringCollection strHeaders = new StringCollection();
            String[,] strValues = new String[100000, 100];

            lblMessage.Text = "";

            if (ddlReportType.Text == "Audit Report")
            {
                if ((ddlTransactionType.Text == "Create Blood Kit")|| (ddlTransactionType.Text == "Receive Blood Kit"))
                {
                    strHeaders.Add("Transaction Date");
                    //  strHeaders.Add("Transaction Type");
                    strHeaders.Add("Blood Kit Barcode");
                    strHeaders.Add("Donor ID");
                    strHeaders.Add("Last Name");
                    strHeaders.Add("First Name");
                    //strHeaders.Add("User ID");
                    //strHeaders.Add("Microbial Order Date");
                    //strHeaders.Add("Toxicology Order Date");
                    //strHeaders.Add("Genetics Order Date");
                    //strHeaders.Add("Date Finalized");
                }
                else if ((ddlTransactionType.Text == "Create Milk Kit") || (ddlTransactionType.Text == "Receive Milk Kit"))
                {

                    strHeaders.Add("Transaction Date");
                   // strHeaders.Add("Transaction Type");
                    strHeaders.Add("Milk kit Barcode");
                    strHeaders.Add("Donor ID");
                    strHeaders.Add("Last Name");
                    strHeaders.Add("First Name");
                    strHeaders.Add("Tracking #");
                    //strHeaders.Add("User ID");
                    //strHeaders.Add("Microbial Order Date");
                    //strHeaders.Add("Toxicology Order Date");
                    //strHeaders.Add("Genetics Order Date");
                    //strHeaders.Add("Date Finalized");
                }
                else if ((ddlTransactionType.Text == "Quarantine Milk Bag"))
                {
                    strHeaders.Add("Date Quarantined");
                    // strHeaders.Add("Transaction Type");
                    strHeaders.Add("Milk kit Barcode");
                    strHeaders.Add("Volume");
                    strHeaders.Add("Donor ID");
                    strHeaders.Add("Last Name");
                    strHeaders.Add("First Name");
                    
                    //strHeaders.Add("User ID");
                    //strHeaders.Add("Microbial Order Date");
                    //strHeaders.Add("Toxicology Order Date");
                    //strHeaders.Add("Genetics Order Date");
                    //strHeaders.Add("Date Finalized");
                }
                else if (ddlTransactionType.Text == "Create Case")
                {

                    strHeaders.Add("Date Created");
                    // strHeaders.Add("Transaction Type");
                    strHeaders.Add("Case Barcode");
                    strHeaders.Add("Quantity");
                    strHeaders.Add("PO Number");
                    strHeaders.Add("Ship Type");
                    strHeaders.Add("Tracking #");
                    //strHeaders.Add("User ID");
                    //strHeaders.Add("Microbial Order Date");
                    //strHeaders.Add("Toxicology Order Date");
                    //strHeaders.Add("Genetics Order Date");
                    //strHeaders.Add("Date Finalized");
                }
                #region Set Filters
                //Set Filters
                // Date Range
                if (chkAllDates.Checked == false)
                {
                    strTransactionDateFrom = calDateFrom.SelectedDate.ToShortDateString();
                    strTransactionDateTo = calDateTo.SelectedDate.ToShortDateString();
                }

                // UserID
                if (txtUserID.Text.Trim() != "")
                {
                    strUserIDFilter = txtUserID.Text.Trim().Replace("'", "''");
                }

                // ItemID
                if (txtItemID.Text.Trim() != "")
                {
                    strItemIDFilter = txtItemID.Text.Trim().Replace("'", "''");
                    if (strItemIDFilter.StartsWith("MK") == true)
                    {
                        strItemTable = "tblMilkKits.[Barcode]";
                    }
                    else if (strItemIDFilter.StartsWith("BK") == true)
                    {
                        strItemTable = "tblBloodKits.[DIN]";
                    }
                    else if (strItemIDFilter.StartsWith("DK") == true)
                    {
                        strItemTable = "tblDNAKits.[Barcode]";
                    }
                    else if (strItemIDFilter.StartsWith("CA") == true)
                    {
                        strItemTable = "tblCases.[Barcode]";
                    }
                    else if (strItemIDFilter.StartsWith("LT") == true)
                    {
                        strItemTable = "tblLots.[Barcode]";
                    }
                    else
                    {
                        lblMessage.Text = "Invalid Item ID filter.  Must be a Milk Kit, Blood Kit, DNA Kit, Case or Lot barcode.";
                        return;
                    }
                }

                #endregion

                // Fill dataset
                #region Setup Database Connection
                // Setup Database Connection
                string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
                System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = conn;
                System.Data.SqlClient.SqlDataReader dr;
                #endregion

                #region Setup Second Database Connection
                // Setup Second Database Connection
                System.Data.SqlClient.SqlConnection conn2 = new SqlConnection(strConnection);
                System.Data.SqlClient.SqlCommand sqlCommand2 = new System.Data.SqlClient.SqlCommand();
                sqlCommand2.CommandType = CommandType.Text;
                sqlCommand2.Connection = conn2;
                System.Data.SqlClient.SqlDataReader dr2;
                #endregion

                // Add filters to SQL query if needed
                #region Date Range Filter

                string baseQuery = string.Empty;
                baseQuery += "SELECT t. *,";
                baseQuery += "tblMilkKits.Finalized AS DateFinalized,";
                baseQuery += "tblDNAKits.MicrobialOrdered as MicrobialOrdered,";
                baseQuery += "tblDonors.DonorID AS DonorID,";
                baseQuery += "tblDNAKits.ToxicologyOrdered as ToxicologyOrdered,";
                baseQuery += "tblDNAKits.GeneticOrdered AS GeneticOrdered,";
                baseQuery += "tblDonors.FirstName AS FirstName,";
                baseQuery += "tblDonors.LastName AS LastName,";
                baseQuery += " tblBloodKits.DIN AS DIN,";
                baseQuery += "tblMilkKits.Barcode AS MilkKitBarcode,";
                baseQuery += "tblDNAKits.Barcode AS DNAKitBarcode,";
                baseQuery += "tblCases.Barcode AS CaseBarcode,";
                baseQuery += "tblCases.PONumber AS PONo,";
                baseQuery += "tblCases.TrackingNumber AS CaseTrckNo,";
                baseQuery += "tblCases.CaseQuantity AS CaseQty,";
                baseQuery += "tblLots.Barcode AS LotBarcode , ";
                baseQuery += "tblMilkKits.Volume AS MilkKitVolume, ";
                baseQuery += "tblMilkKits.TrackingNumber AS MilkkitTrckNo ";
                baseQuery += "FROM tblTransactions AS t";
                baseQuery += " LEFT JOIN tblUsers ON t.TransactionUser=tblUsers.ID";
                baseQuery += " LEFT JOIN tblBloodKits ON t.ItemID=tblBloodKits.ID";
                baseQuery += " LEFT JOIN tblMilkKits ON t.ItemID=tblMilkKits.ID";
                baseQuery += " LEFT JOIN tblDNAKits ON t.ItemID=tblDNAKits.ID";
                baseQuery += " LEFT JOIN tblLots ON t.ItemID=tblLots.ID";
                baseQuery += " LEFT JOIN tblCases ON t.ItemID=tblCases.ID";
                baseQuery += " LEFT JOIN tblDonors ON tblMilkKits.DonorID=tblDonors.DonorId";
           
                if (strTransactionDateTo != "0")
                {
                    strSQLQuery = "DECLARE @startDate DateTime DECLARE @endDate DateTime";
                    strSQLQuery += " SET @startDate='" + strTransactionDateFrom + " 00:00:00" + "' SET @endDate='" + strTransactionDateTo + " 23:59:59'";
                    strSQLQuery += baseQuery;
                    strSQLQuery += " WHERE t.[TransactionDate] >= @startDate AND t.[TransactionDate] <= @endDate";

                    intWhere = 1;
                }
                else
                {
                    strSQLQuery += baseQuery;
                }
                #endregion

                #region Transaction Type Filter
                // Transaction Type
                if (strTransactionTypeFilter != "All Transactions")
                {
                    if (intWhere == 0)
                    {
                        strSQLQuery += " WHERE ";
                    }
                    else
                    {
                        strSQLQuery += " AND ";
                    }
                    strSQLQuery += "t.[TransactionType]='" + strTransactionTypeFilter + "'";
                    intWhere = 1;
                }
                #endregion

                #region UserID Filter
                // UserID
                if (strUserIDFilter != "0")
                {
                    if (intWhere == 0)
                    {
                        strSQLQuery += " WHERE ";
                    }
                    else
                    {
                        strSQLQuery += " AND ";
                    }
                    strSQLQuery += "tblUsers.[Username]='" + strUserIDFilter + "'";
                    intWhere = 1;
                }
                #endregion

                #region Item ID Filter
                // WorkProductID
                if (strItemIDFilter != "0")
                {
                    if (intWhere == 0)
                    {
                        strSQLQuery += " WHERE ";
                    }
                    else
                    {
                        strSQLQuery += " AND ";
                    }

                    strSQLQuery += strItemTable + "='" + strItemIDFilter + "'";
                    intWhere = 1;
                }
                #endregion

                strSQLQuery += " ORDER BY t.[TransactionDate]";

                sqlCommand.CommandText = strSQLQuery;
                sqlCommand.Connection = conn;
                intRow = 0;
                try
                {
                    conn.Open();
                    dr = sqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        if ((ddlTransactionType.Text == "Create Blood Kit") || (ddlTransactionType.Text == "Receive Blood Kit") ||
                            (ddlTransactionType.Text == "Create Milk Kit") || (ddlTransactionType.Text == "Receive Milk Kit") ||
                            (ddlTransactionType.Text == "Quarantine Milk Bag"))
                        {
                            strValues[intRow, 0] = dr["TransactionDate"].ToString();

                            /// strValues[intRow, 1] = dr["DateFinalized"].ToString();
                            //if (dr["DateFinalized"].ToString() == "")
                            //{
                            //    strValues[intRow, 1] = "NA";
                            //}
                            //strValues[intRow, 2] = string.IsNullOrEmpty(dr["MicrobialOrdered"].ToString()) ? "NA" : dr["MicrobialOrdered"].ToString();
                            //strValues[intRow, 3] = string.IsNullOrEmpty(dr["ToxicologyOrdered"].ToString()) ? "NA" : dr["ToxicologyOrdered"].ToString();
                            //strValues[intRow, 4] = string.IsNullOrEmpty(dr["GeneticOrdered"].ToString()) ? "NA": dr["GeneticOrdered"].ToString();
                            if (ddlTransactionType.Text != "Quarantine Milk Bag")
                            {
                                strValues[intRow, 3] = dr["LastName"].ToString();
                                strValues[intRow, 4] = dr["FirstName"].ToString();
                            }
                            else
                            {
                                strValues[intRow, 4] = dr["LastName"].ToString();
                                strValues[intRow, 5] = dr["FirstName"].ToString();
                            }

                            //strValues[intRow, 7] = dr["TransactionType"].ToString();
                            //strValues[intRow, 8] = dr["Username"].ToString();
                            if (dr["ItemType"].ToString() == "Blood Kit")
                            {
                                strValues[intRow, 1] = "[" + dr["DIN"].ToString() + "]";
                            }
                            else if (dr["ItemType"].ToString() == "Milk Kit")
                            {
                                strValues[intRow, 1] = dr["MilkKitBarcode"].ToString();
                            }
                            else if (dr["ItemType"].ToString() == "DNA Kit")
                            {
                                strValues[intRow, 1] = dr["DNAKitBarcode"].ToString();
                            }
                            else if (dr["ItemType"].ToString() == "Lot")
                            {
                                strValues[intRow, 1] = dr["LotBarcode"].ToString();
                            }
                            else if (dr["ItemType"].ToString() == "Case")
                            {
                                strValues[intRow, 1] = dr["CaseBarcode"].ToString();
                            }
                            // if (dr["ItemType"].ToString() == "Lot" || dr["ItemType"].ToString() == "Case")
                            //{
                            //strValues[intRow, 10] = "N/A";
                            // }
                            //else
                            //{
                            //if (dr["MilkDonorID"].ToString().Length > 0)
                            //{
                            //    var avalue = strValues[intRow, 10] = dr["MilkDonorID"].ToString();
                            //}
                            //else if (dr["DNADonorID"].ToString().Length > 0)
                            //{
                            //    strValues[intRow, 10] = dr["DNADonorID"].ToString();
                            //}
                            //else
                            //{
                            if (ddlTransactionType.Text != "Quarantine Milk Bag")
                            {
                                strValues[intRow, 2] = dr["DonorID"].ToString();
                            }
                            else
                            {
                                strValues[intRow, 2] = dr["MilkKitVolume"].ToString();
                                strValues[intRow, 3] = dr["DonorID"].ToString();
                            }
                            //}
                            if((ddlTransactionType.Text == "Create Milk Kit") || (ddlTransactionType.Text == "Receive Milk Kit"))
                            {
                                strValues[intRow, 5] = dr["MilkkitTrckNo"].ToString();
                            }
                            //}
                        }
                        else
                        {
                            strValues[intRow, 0] = dr["TransactionDate"].ToString();
                           if (dr["ItemType"].ToString() == "Case")
                            {
                                strValues[intRow, 1] = dr["CaseBarcode"].ToString();
                            }
                            strValues[intRow, 2] = dr["CaseQty"].ToString();
                            strValues[intRow, 3] = dr["PONo"].ToString();
                            strValues[intRow, 4] = "";//dr["PONo"].ToString();
                            strValues[intRow, 5] = dr["CaseTrckNo"].ToString();


                        }
                        //strTransactionID = dr["ID"].ToString();

                        //sqlCommand2.CommandText = "SELECT * FROM tblTransactionDetails WHERE [TransactionID]=" + strTransactionID;
                        //try
                        //{
                        //    conn2.Open();
                        //    dr2 = sqlCommand2.ExecuteReader();
                        //    while (dr2.Read())
                        //    {
                        //        strValues[intRow, 11] = dr2["Field"].ToString();
                        //        if (dr2["Field"].ToString() == "Tracking Number" || dr2["Field"].ToString() == "DIN")
                        //        {
                        //            var anothervalue = strValues[intRow, 12] = "[" + dr2["Value"].ToString() + "]";
                        //        }
                        //        else
                        //        {
                        //            var value1= strValues[intRow, 12] = dr2["Value"].ToString();
                        //        }
                        //        intRow++;
                        //    }
                        //    intRow--;
                        //    dr2.Dispose();
                        //}
                        //catch (Exception ex)
                        //{
                        //    lblMessage.Text = (ex.Message.ToString());
                        //    return;
                        //}
                        //finally
                        //{
                        //    conn2.Close();
                        //}
                        intRow++;
                    }
                    dr.Dispose();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = (ex.Message.ToString());
                    return;
                }
                finally
                {
                    conn.Dispose();
                    conn2.Dispose();
                }

                // Create Header Data
                for (int i = 0; i < strHeaders.Count; i++)
                {
                    strHeaderData += strHeaders[i] + ",";
                }

                strHeaderData = strHeaderData.Substring(0, strHeaderData.Length - 1);

                for (int i = 0; i < intRow; i++)
                {
                    for (int j = 0; j < intHeaderCount; j++)
                    {
                        strData += strValues[i, j] + ",";
                    }
                    strData = strData.Substring(0, strData.Length - 1);
                    strData += Environment.NewLine;
                }

                Response.Clear();
                Response.Charset = "UTF-8";
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", "attachment;filename=NiQ_Export.csv");
                
                Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                Response.Write(strHeaderData + Environment.NewLine + strData);
                Response.End();
            }
            else if (ddlReportType.Text == "Inventory Report")
            {
                if (ddlTransactionType.Text == "Milk Kit Status Report")
                {
                    String strHeader = "";
                    String strPassedMilkKits = "";
                    String strPendingMilkKits = "";
                    String strFailedMilkKits = "";

                    #region Milk Kit Testing Status
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
                    try
                    {
                        SqlCommand sqlCommand;
                        System.Data.SqlClient.SqlDataReader dr;
                        //calDateFrom.SelectedDate.ToShortDateString()
                        if (chkAllDates.Checked == true)
                        {
                            sqlCommand = new SqlCommand(@"SELECT * FROM [NiQ_DonorTracking].[dbo].[tblMilkKits] mk INNER JOIN [NiQ_DonorTracking].[dbo].[tblDonors] d ON mk.DonorID = d.DonorID WHERE mk.Active = 1 AND mk.Finalized IS NOT NULL ORDER BY mk.ID", conn);
                        }

                        else
                        {
                            sqlCommand = new SqlCommand(@"SELECT * FROM [NiQ_DonorTracking].[dbo].[tblMilkKits] mk INNER JOIN [NiQ_DonorTracking].[dbo].[tblDonors] d ON mk.DonorID = d.DonorID WHERE mk.Active = 1 AND mk.Finalized IS NOT NULL AND ReceiveDate BETWEEN '" + calDateFrom.SelectedDate.ToShortDateString() + "' AND '"+calDateTo.SelectedDate.ToShortDateString() +"' ORDER BY mk.ID", conn);
                        }
                            
                        sqlCommand.Connection = conn;
                        conn.Open();
                        dr = sqlCommand.ExecuteReader();
                        strHeader = "Status,Barcode,Donor ID,LastName,FirstName,Finalized,Volume,Date Paid" + Environment.NewLine;

                        while (dr.Read())
                        {
                            // Only kits that are not in Lots
                           
                                // Only kits that have testing results
                                if (dr["DNATest"].ToString().Length > 0 || dr["DrugAlcoholTest"].ToString().Length > 0 || dr["MicrobialTest"].ToString().Length > 0)
                                {
                                    if (dr["DNATest"].ToString() == "True" && dr["DrugAlcoholTest"].ToString() == "True" && dr["MicrobialTest"].ToString() == "True")
                                    {
                                        // All passed
                                        strPassedMilkKits += "PASSED,";
                                        strPassedMilkKits += dr["Barcode"].ToString() + ",";
                                        strPassedMilkKits += dr["DonorID"].ToString() + ",";
                                        strPassedMilkKits += dr["LastName"].ToString() + ",";
                                        strPassedMilkKits += dr["FirstName"].ToString() + ",";
                                       
                                        //strPassedMilkKits += dr["ReceiveDate"].ToString() + ",";
                                        //strPassedMilkKits += dr["QuarantineDate"].ToString() + ",";
                                        strPassedMilkKits += dr["Finalized"].ToString() + ",";
                                        strPassedMilkKits += dr["Volume"].ToString() + ",";
                                        //strPassedMilkKits += dr["DNATest"].ToString() + ",";
                                        //strPassedMilkKits += dr["DrugAlcoholTest"].ToString() + ",";
                                        //strPassedMilkKits += dr["MicrobialTest"].ToString() + ",";
                                       
                                        strPassedMilkKits += dr["DatePaid"].ToString() + Environment.NewLine;

                                    }

                                    /*
                                    else if (dr["DNATest"].ToString() == "False" || dr["DNATest"].ToString() == "NULL" || dr["DrugAlcoholTest"].ToString() == "False" || dr["MicrobialTest"].ToString() == "False")
                                    {
                                        strPassedMilkKits += "FAILED,";
                                        strPassedMilkKits += dr["Barcode"].ToString() + ",";
                                        strPassedMilkKits += dr["DonorID"].ToString() + ",";
                                        strPassedMilkKits += dr["LastName"].ToString() + ",";
                                        strPassedMilkKits += dr["FirstName"].ToString() + ",";

                                        //strPassedMilkKits += dr["ReceiveDate"].ToString() + ",";
                                        //strPassedMilkKits += dr["QuarantineDate"].ToString() + ",";
                                        strPassedMilkKits += dr["Finalized"].ToString() + ",";
                                        strPassedMilkKits += dr["Volume"].ToString() + ",";
                                        //strPassedMilkKits += dr["DNATest"].ToString() + ",";
                                        //strPassedMilkKits += dr["DrugAlcoholTest"].ToString() + ",";
                                        //strPassedMilkKits += dr["MicrobialTest"].ToString() + ",";

                                        strPassedMilkKits += dr["DatePaid"].ToString() + Environment.NewLine;
                                    }
                                    else
                                    {
                                        strPassedMilkKits += "PENDING,";
                                        strPassedMilkKits += dr["Barcode"].ToString() + ",";
                                        strPassedMilkKits += dr["DonorID"].ToString() + ",";
                                        strPassedMilkKits += dr["LastName"].ToString() + ",";
                                        strPassedMilkKits += dr["FirstName"].ToString() + ",";

                                        //strPassedMilkKits += dr["ReceiveDate"].ToString() + ",";
                                        //strPassedMilkKits += dr["QuarantineDate"].ToString() + ",";
                                        strPassedMilkKits += dr["Finalized"].ToString() + ",";
                                        strPassedMilkKits += dr["Volume"].ToString() + ",";
                                        //strPassedMilkKits += dr["DNATest"].ToString() + ",";
                                        //strPassedMilkKits += dr["DrugAlcoholTest"].ToString() + ",";
                                        //strPassedMilkKits += dr["MicrobialTest"].ToString() + ",";

                                        strPassedMilkKits += dr["DatePaid"].ToString() + Environment.NewLine;
                                    } */
                                }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    #endregion

                    // Generate CSV report
                    Response.Clear();
                    Response.Charset = "UTF-8";
                    Response.ContentType = "text/csv";
                    Response.AddHeader("Content-Disposition", "attachment;filename=NiQ_Export.csv");
                    Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                    Response.Write(strHeader + strPassedMilkKits + strPendingMilkKits + strFailedMilkKits);
                    Response.End();

                }
                else if (ddlTransactionType.Text == "Milk Kit Grade 1 Report")
                {
                    String strHeader = "";
                    String strPassedMilkKits = "";
                    String strPendingMilkKits = "";
                    String strFailedMilkKits = "";

                    #region Milk Kit Testing Status
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
                    try
                    {
                        SqlCommand sqlCommand;
                        System.Data.SqlClient.SqlDataReader dr;
                        //calDateFrom.SelectedDate.ToShortDateString()
                        if (chkAllDates.Checked == true)
                        {
                            sqlCommand = new SqlCommand(@"SELECT * FROM [NiQ_DonorTracking].[dbo].[tblMilkKits] mk INNER JOIN [NiQ_DonorTracking].[dbo].[tblDonors] d ON mk.DonorID = d.DonorID  WHERE mk.Active = 1 AND Grade = 1 AND mk.Finalized IS NOT NULL AND LotID IS NULL ORDER BY mk.ID", conn);
                        }

                        else
                        {
                            sqlCommand = new SqlCommand(@"SELECT * FROM [NiQ_DonorTracking].[dbo].[tblMilkKits] mk INNER JOIN [NiQ_DonorTracking].[dbo].[tblDonors] d ON mk.DonorID = d.DonorID WHERE mk.Active = 1 AND Grade = 1 AND mk.Finalized IS NOT NULL AND ReceiveDate BETWEEN '" + calDateFrom.SelectedDate.ToShortDateString() + "' AND '" + calDateTo.SelectedDate.ToShortDateString() + "' AND LotID IS NULL  ORDER BY mk.ID", conn);
                        }

                        sqlCommand.Connection = conn;
                        conn.Open();
                        dr = sqlCommand.ExecuteReader();
                        strHeader = "Barcode, Grade, Volume" + Environment.NewLine;

                        while (dr.Read())
                        {
                            // Only kits that are not in Lots

                            // Only kits that have testing results
                            if (dr["DNATest"].ToString().Length > 0 || dr["DrugAlcoholTest"].ToString().Length > 0 || dr["MicrobialTest"].ToString().Length > 0)
                            {
                                if (dr["DNATest"].ToString() == "True" && dr["DrugAlcoholTest"].ToString() == "True" && dr["MicrobialTest"].ToString() == "True")
                                {
                                  
                                    strPassedMilkKits += dr["Barcode"].ToString() + ",";

                                    strPassedMilkKits += dr["Grade"].ToString() + ",";
                                    strPassedMilkKits += dr["Volume"].ToString() + Environment.NewLine;
                                   

                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    #endregion

                    // Generate CSV report
                    Response.Clear();
                    Response.Charset = "UTF-8";
                    Response.ContentType = "text/csv";
                    Response.AddHeader("Content-Disposition", "attachment;filename=NiQ_Grade_1_Export.csv");
                    Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                    Response.Write(strHeader + strPassedMilkKits + strPendingMilkKits + strFailedMilkKits);
                    Response.End();

                }
                else if (ddlTransactionType.Text == "Milk Kit Grade 2 Report")
                {
                    String strHeader = "";
                    String strPassedMilkKits = "";
                    String strPendingMilkKits = "";
                    String strFailedMilkKits = "";

                    #region Milk Kit Testing Status
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
                    try
                    {
                        SqlCommand sqlCommand;
                        System.Data.SqlClient.SqlDataReader dr;
                        //calDateFrom.SelectedDate.ToShortDateString()
                        if (chkAllDates.Checked == true)
                        {
                            sqlCommand = new SqlCommand(@"SELECT * FROM [NiQ_DonorTracking].[dbo].[tblMilkKits] mk INNER JOIN [NiQ_DonorTracking].[dbo].[tblDonors] d ON mk.DonorID = d.DonorID  WHERE mk.Active = 1 AND Grade = 2 AND mk.Finalized IS NOT NULL AND LotID IS NULL ORDER BY mk.ID", conn);
                        }

                        else
                        {
                            sqlCommand = new SqlCommand(@"SELECT * FROM [NiQ_DonorTracking].[dbo].[tblMilkKits] mk INNER JOIN [NiQ_DonorTracking].[dbo].[tblDonors] d ON mk.DonorID = d.DonorID WHERE mk.Active = 1 AND Grade = 2 AND mk.Finalized IS NOT NULL AND ReceiveDate BETWEEN '" + calDateFrom.SelectedDate.ToShortDateString() + "' AND '" + calDateTo.SelectedDate.ToShortDateString() + "' AND LotID IS NULL  ORDER BY mk.ID", conn);
                        }

                        sqlCommand.Connection = conn;
                        conn.Open();
                        dr = sqlCommand.ExecuteReader();
                        strHeader = "Barcode, Grade, Volume" + Environment.NewLine;

                        while (dr.Read())
                        {
                            // Only kits that are not in Lots

                            // Only kits that have testing results
                            if (dr["DNATest"].ToString().Length > 0 || dr["DrugAlcoholTest"].ToString().Length > 0 || dr["MicrobialTest"].ToString().Length > 0)
                            {
                                if (dr["DNATest"].ToString() == "True" && dr["DrugAlcoholTest"].ToString() == "True" && dr["MicrobialTest"].ToString() == "True")
                                {

                                    strPassedMilkKits += dr["Barcode"].ToString() + ",";

                                    strPassedMilkKits += dr["Grade"].ToString() + ",";
                                    strPassedMilkKits += dr["Volume"].ToString() + Environment.NewLine;


                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    #endregion

                    // Generate CSV report
                    Response.Clear();
                    Response.Charset = "UTF-8";
                    Response.ContentType = "text/csv";
                    Response.AddHeader("Content-Disposition", "attachment;filename=NiQ_Grade_2_Export.csv");
                    Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                    Response.Write(strHeader + strPassedMilkKits + strPendingMilkKits + strFailedMilkKits);
                    Response.End();

                }
                else if (ddlTransactionType.Text == "Lot Information Report")
                {
                    String strLotSection = "";
                    String strMilkKitSection = "";
                    String strCaseSection = "";
                    decimal decTotalVolume = 0;
                    decimal decMilkKitVolume = 0;
                    Int32 intRecordCount = 0;
                    String strBarcode = txtItemID.Text.Trim().Replace("'","''");

                    // Check for valid Lot Number
                    if (txtItemID.Text.Trim() == "" || txtItemID.Text.StartsWith("LT") == false)
                    {
                        lblMessage.Text = "Please enter a valid Lot Number.";
                        txtItemID.Focus();
                        return;
                    }

                    #region Lots
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
                    try
                    {
                        SqlCommand sqlCommand;
                        System.Data.SqlClient.SqlDataReader dr;
                        sqlCommand = new SqlCommand(@"SELECT * FROM [tblLots] WHERE [Barcode]='" + strBarcode + "' ORDER BY [ID]", conn);
                        sqlCommand.Connection = conn;
                        conn.Open();
                        dr = sqlCommand.ExecuteReader();
                        while (dr.Read())
                        {
                            intRecordCount++;
                            strLotSection = "Lot Number:," + dr["Barcode"].ToString() + Environment.NewLine;
                            strLotSection += "Best By Date:," + dr["BestByDate"].ToString() + Environment.NewLine;
                            strLotSection += "Transferred:," + dr["Transferred"].ToString() + Environment.NewLine;
                            strLotSection += "Closed:," + dr["Closed"].ToString() + Environment.NewLine;
                            strLotSection += "Total Cases:," + dr["TotalCases"].ToString() + Environment.NewLine;
                            strLotSection += "Cases Remaining:," + dr["CasesRemaining"].ToString() + Environment.NewLine;
                            strLotSection += "Sample Pouches:," + dr["SamplePouches"].ToString() + Environment.NewLine;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    #endregion

                    #region Included Milk Kits
                    SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
                    try
                    {
                        SqlCommand sqlCommand2;
                        System.Data.SqlClient.SqlDataReader dr2;
                        sqlCommand2 = new SqlCommand(@"SELECT [tblMilkKits].[Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial], [DatePaid] AS [Date Paid] FROM [tblMilkKits] INNER JOIN [tblLots] ON [tblMilkKits].[LotID]=[tblLots].[ID] WHERE [tblMilkKits].[Active]=1 AND [tblLots].[Barcode]='" + strBarcode + "' ORDER BY [tblMilkKits].[ID]", conn2);
                        sqlCommand2.Connection = conn2;
                        conn2.Open();
                        dr2 = sqlCommand2.ExecuteReader();

                        strMilkKitSection = "Included Milk Kits:" + Environment.NewLine;
                        //strMilkKitSection += "Barcode,Shipping Service,Tracking Number,Received,Quarantined,Volume,DNA,Drug,Microbial,Date Paid" + Environment.NewLine;
                        strMilkKitSection += "Barcode,Finalized Date,Volume,Genetic,Drug,Microbial" + Environment.NewLine;

                        while (dr2.Read())
                        {
                            intRecordCount++;
                            strMilkKitSection += dr2["Barcode"].ToString() + ",";
                           // strMilkKitSection += dr2["Shipping Service"].ToString() + ",";
                            //strMilkKitSection += "[" + dr2["Tracking Number"].ToString() + "],";
                            strMilkKitSection += dr2["Received"].ToString() + ",";
                            //strMilkKitSection += dr2["Quarantined"].ToString() + ",";
                            strMilkKitSection += dr2["Volume"].ToString() + ",";
                            strMilkKitSection += dr2["DNA"].ToString() + ",";
                            strMilkKitSection += dr2["Drug"].ToString() + ",";
                            strMilkKitSection += dr2["Microbial"].ToString() + System.Environment.NewLine; //+ ",";
                            //strMilkKitSection += (dr2["Date Paid"].ToString())+System.Environment.NewLine;
                            var dates = dr2["Date Paid"].ToString();

                            if (decimal.TryParse(dr2["Volume"].ToString(), out decMilkKitVolume) == false)
                            {
                                decMilkKitVolume = 0;
                            }
                            decTotalVolume += decMilkKitVolume;
                        }

                        // Add total volume to first section
                        strLotSection += "Total Volume:," + decTotalVolume.ToString() + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn2.Close();
                        conn2.Dispose();
                    }
                    #endregion

                    #region Cases
                    SqlConnection conn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
                    try
                    {
                        SqlCommand sqlCommand3;
                        System.Data.SqlClient.SqlDataReader dr3;
                        sqlCommand3 = new SqlCommand(@"SELECT [tblCases].[Barcode] AS [Barcode],[tblLocations].[Name] AS [Location],[ShipDate] AS [Ship Date],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[PONumber] AS [PO Number],[CaseQuantity] AS [Cases Shipped] FROM [tblCases] INNER JOIN [tblLots] ON [tblCases].[LotID]=[tblLots].[ID] INNER JOIN [tblLocations] ON [tblCases].[LocationID]=[tblLocations].[ID] WHERE [tblCases].[Active]=1 AND [tblLots].[Barcode]='" + strBarcode + "' ORDER BY [tblCases].[ID]", conn3);
                        sqlCommand3.Connection = conn3;
                        conn3.Open();
                        dr3 = sqlCommand3.ExecuteReader();

                        strCaseSection = "Case Information:" + Environment.NewLine;
                        //strCaseSection += "Barcode,Location,Ship Date,Shipping Service,Tracking Number,PO Number,Cases Shipped" + Environment.NewLine;
                        strCaseSection += "Barcode,Ship Date,Shipping Service,Tracking Number,PO Number,Cases Shipped" + Environment.NewLine;

                        while (dr3.Read())
                        {
                            intRecordCount++;
                            strCaseSection += dr3["Barcode"].ToString() + ",";
                           // strCaseSection += dr3["Location"].ToString() + ",";
                            strCaseSection += dr3["Ship Date"].ToString() + ",";
                            strCaseSection += dr3["Shipping Service"].ToString() + ",";
                            strCaseSection += dr3["Tracking Number"].ToString() + ",";
                            strCaseSection += dr3["PO Number"].ToString() + ",";
                            strCaseSection += dr3["Cases Shipped"].ToString() + Environment.NewLine;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn3.Close();
                        conn3.Dispose();
                    }
                    #endregion

                    // Display an error if Lot is not found
                    if (intRecordCount <= 0)
                    {
                        lblMessage.Text = "Lot Number [" + txtItemID.Text.Trim() + "] not found.";
                        return;
                    }

                    // Generate CSV report
                    Response.Clear();
                    Response.Charset = "UTF-8";
                    Response.ContentType = "text/csv";
                    Response.AddHeader("Content-Disposition", "attachment;filename=NiQ_Export.csv");
                    Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                    Response.Write(strLotSection + Environment.NewLine + strMilkKitSection + Environment.NewLine + strCaseSection);
                    Response.End();
                }
                else if (ddlTransactionType.Text == "Pallet Information Report")
                {
                    String strPalletSection = "";
                    String strPassedMilkKitSection = "";
                    String strFailedMilkKitSection = "";
                    decimal decTotalPassedVolume = 0;
                    decimal decTotalFailedVolume = 0;
                    decimal decTotalVolume = 0;
                    decimal decPassedMilkKitVolume = 0;
                    decimal decFailedMilkKitVolume = 0;
                    int PalletID = 0;
                    Int32 intRecordCount = 0;
                    String strBarcode = txtItemID.Text.Trim().Replace("'", "''");

                    // Check for valid Pallet
                    if (txtItemID.Text.Trim() == "")
                    {
                        lblMessage.Text = "Please enter a valid Pallet Number.";
                        txtItemID.Focus();
                        return;
                    }

                    SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());

                    strPalletSection = "Pallet:," + strBarcode + Environment.NewLine;
                    #region LoadPallet
                    try
                    {
                        SqlCommand sqlCommand2;
                        System.Data.SqlClient.SqlDataReader dr2;
                        sqlCommand2 = new SqlCommand(@"SELECT ID FROM [tblPallets] WHERE Barcode ='" + strBarcode + "'", conn2);
                        sqlCommand2.Connection = conn2;
                        conn2.Open();
                        dr2 = sqlCommand2.ExecuteReader();
                        while (dr2.Read())
                        {
                            PalletID = int.Parse(dr2["ID"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn2.Close();
                       
                    }

                    #endregion
                   
                    #region Included Milk Kits

                    try
                    {
                        conn2.Open();
                        SqlCommand sqlCommand2;
                        System.Data.SqlClient.SqlDataReader dr2;
                        sqlCommand2 = new SqlCommand(@"SELECT [tblMilkKits].[Barcode],[ShippingService] AS [Shipping Service],[TrackingNumber] AS [Tracking Number],[ReceiveDate] AS [Received],[QuarantineDate] AS [Quarantined],[Volume],[DNATest] AS [DNA],[DrugAlcoholTest] AS [Drug],[MicrobialTest] AS [Microbial], APC, EB, CC, RYM, Mold, STX, ECOLI, SAL FROM [tblMilkKits] WHERE [tblMilkKits].[Active]=1 AND [tblMilkKits].[PalletID]='" + PalletID + "' ORDER BY [tblMilkKits].[ID]", conn2);
                        sqlCommand2.Connection = conn2;
                       
                        dr2 = sqlCommand2.ExecuteReader();

                        strPassedMilkKitSection = "Included Milk Kits (Passed):" + Environment.NewLine;
                        strFailedMilkKitSection = "Included Milk Kits (Failed):" + Environment.NewLine;
                        strPassedMilkKitSection += "Barcode,Shipping Service,Tracking Number,Received,Quarantined,Volume(OZ),Genetics,Drug,Microbial,APC,EB,CC,RYM,Mold,STX,E.Coli,SAL" + Environment.NewLine;
                        strFailedMilkKitSection += "Barcode,Shipping Service,Tracking Number,Received,Quarantined,Volume(OZ),Genetics,Drug,Microbial,APC,EB,CC,RYM,Mold,STX,E.Coli,SAL" + Environment.NewLine;

                        while (dr2.Read())
                        {
                            intRecordCount++;

                            if (dr2["DNA"].ToString() == "True" && dr2["Drug"].ToString() == "True" && dr2["Microbial"].ToString() == "True")
                            {
                                strPassedMilkKitSection += dr2["Barcode"].ToString() + ",";
                                strPassedMilkKitSection += dr2["Shipping Service"].ToString() + ",";
                                strPassedMilkKitSection += "[" + dr2["Tracking Number"].ToString() + "],";
                                strPassedMilkKitSection += dr2["Received"].ToString() + ",";
                                strPassedMilkKitSection += dr2["Quarantined"].ToString() + ",";
                                strPassedMilkKitSection += dr2["Volume"].ToString() + ",";
                                strPassedMilkKitSection += dr2["DNA"].ToString() + ",";
                                strPassedMilkKitSection += dr2["Drug"].ToString() + ",";
                                strPassedMilkKitSection += dr2["Microbial"].ToString() + ",";
                                strPassedMilkKitSection += dr2["APC"].ToString() + ",";
                                strPassedMilkKitSection += dr2["EB"].ToString() + ",";
                                strPassedMilkKitSection += dr2["CC"].ToString() + ",";
                                strPassedMilkKitSection += dr2["RYM"].ToString() + ",";
                                strPassedMilkKitSection += dr2["Mold"].ToString() + ",";
                                strPassedMilkKitSection += dr2["STX"].ToString() + ",";
                                strPassedMilkKitSection += dr2["ECOLI"].ToString() + ",";
                                strPassedMilkKitSection += dr2["SAL"].ToString() + Environment.NewLine;

                                if (decimal.TryParse(dr2["Volume"].ToString(), out decPassedMilkKitVolume) == false)
                                {
                                    decPassedMilkKitVolume = 0;
                                }
                                decTotalPassedVolume += decPassedMilkKitVolume;
                            }
                            else
                            {
                                strFailedMilkKitSection += dr2["Barcode"].ToString() + ",";
                                strFailedMilkKitSection += dr2["Shipping Service"].ToString() + ",";
                                strFailedMilkKitSection += "[" + dr2["Tracking Number"].ToString() + "],";
                                strFailedMilkKitSection += dr2["Received"].ToString() + ",";
                                strFailedMilkKitSection += dr2["Quarantined"].ToString() + ",";
                                strFailedMilkKitSection += dr2["Volume"].ToString() + ",";
                                strFailedMilkKitSection += dr2["DNA"].ToString() + ",";
                                strFailedMilkKitSection += dr2["Drug"].ToString() + ",";
                                strFailedMilkKitSection += dr2["Microbial"].ToString() + ",";
                                strPassedMilkKitSection += dr2["APC"].ToString() + ",";
                                strPassedMilkKitSection += dr2["EB"].ToString() + ",";
                                strPassedMilkKitSection += dr2["CC"].ToString() + ",";
                                strPassedMilkKitSection += dr2["RYM"].ToString() + ",";
                                strPassedMilkKitSection += dr2["Mold"].ToString() + ",";
                                strPassedMilkKitSection += dr2["STX"].ToString() + ",";
                                strPassedMilkKitSection += dr2["ECOLI"].ToString() + ",";
                                strPassedMilkKitSection += dr2["SAL"].ToString() + Environment.NewLine;

                                if (decimal.TryParse(dr2["Volume"].ToString(), out decFailedMilkKitVolume) == false)
                                {
                                    decFailedMilkKitVolume = 0;
                                }
                                decTotalFailedVolume += decFailedMilkKitVolume;
                            }


                        }

                        // Add kit count to first section
                        strPalletSection += "Number of Kits:," + intRecordCount.ToString() + Environment.NewLine;

                        // Add total volume to first section
                        decTotalVolume = decTotalPassedVolume + decTotalFailedVolume;
                        strPalletSection += "Total Volume (Passed):," + decTotalPassedVolume.ToString() + Environment.NewLine;
                        strPalletSection += "Total Volume (Failed):," + decTotalFailedVolume.ToString() + Environment.NewLine;
                        strPalletSection += "Total Volume:," + decTotalVolume.ToString() + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = (ex.Message.ToString());
                        return;
                    }
                    finally
                    {
                        conn2.Close();
                        conn2.Dispose();
                    }
                    #endregion

                    // Display an error if Pallet is not found
                    if (intRecordCount <= 0)
                    {
                        lblMessage.Text = "Pallet Number [" + txtItemID.Text.Trim() + "] not found.";
                        return;
                    }

                    // Generate CSV report
                    Response.Clear();
                    Response.Charset = "UTF-8";
                    Response.ContentType = "text/csv";
                    Response.AddHeader("Content-Disposition", "attachment;filename=NiQ_Export.csv");
                    Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                    Response.Write(strPalletSection + Environment.NewLine + strPassedMilkKitSection + Environment.NewLine + strFailedMilkKitSection);
                    Response.End();
                }
            }
        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportType.Text == "Audit Report")
            {
                ddlTransactionType.Items.Clear();
                ddlTransactionType.Items.Add("All Transactions");
                ddlTransactionType.Items.Add("Create Blood Kit");
                ddlTransactionType.Items.Add("Receive Blood Kit");
                ddlTransactionType.Items.Add("Create Milk Kit");
                ddlTransactionType.Items.Add("Receive Milk Kit");
                ddlTransactionType.Items.Add("Quarantine Milk Bag");
                //ddlTransactionType.Items.Add("Create DNA Kit");
                //ddlTransactionType.Items.Add("Lab Results");
               // ddlTransactionType.Items.Add("Create Lot");
                //ddlTransactionType.Items.Add("Transfer Lot");
                ddlTransactionType.Items.Add("Create Case");
                
                chkAllDates.Checked = true;
                chkAllDates.Enabled = true;
                calDateFrom.SelectedDates.Clear();
                calDateTo.SelectedDates.Clear();
                calDateFrom.Enabled = false;
                calDateTo.Enabled = false;
                lblDateFrom.Enabled = false;
                lblDateTo.Enabled = false;
                txtUserID.Text = "";
                txtUserID.Enabled = true;
                txtItemID.Text = "";
                txtItemID.Enabled = true;
            }
            else if (ddlReportType.Text == "Inventory Report")
            {
                ddlTransactionType.Items.Clear();
                ddlTransactionType.Items.Add("Milk Kit Status Report");
                ddlTransactionType.Items.Add("Lot Information Report");
                ddlTransactionType.Items.Add("Pallet Information Report");
                ddlTransactionType.Items.Add("Milk Kit Grade 1 Report");
                ddlTransactionType.Items.Add("Milk Kit Grade 2 Report");

                if (chkAllDates.Checked)
                {
                    //chkAllDates.Checked = true;
                    //chkAllDates.Enabled = true;
                    calDateFrom.SelectedDates.Clear();
                    calDateTo.SelectedDates.Clear();
                    calDateFrom.Enabled = false;
                    calDateTo.Enabled = false;
                    lblDateFrom.Enabled = false;
                    lblDateTo.Enabled = false;
                    txtUserID.Text = "";
                    txtUserID.Enabled = false;
                    txtItemID.Text = "";
                    txtItemID.Enabled = false;
                }

                else
                {
                    //chkAllDates.Checked = true;
                    //chkAllDates.Enabled = true;
                    //calDateFrom.SelectedDates.Clear();
                    //calDateTo.SelectedDates.Clear();
                    calDateFrom.Enabled = true;
                    calDateTo.Enabled = true;
                    lblDateFrom.Enabled = true;
                    lblDateTo.Enabled = true;
                    txtUserID.Text = "";
                    txtUserID.Enabled = false;
                    txtItemID.Text = "";
                    txtItemID.Enabled = false;
                }
               
                
            }
        }

        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportType.Text == "Inventory Report")
            {
                if (ddlTransactionType.Text == "Milk Kit Status Report")
                {
                    txtItemID.Text = "";
                    txtItemID.Enabled = false;
                }
                else if (ddlTransactionType.Text == "Lot Information Report")
                {
                    txtItemID.Text = "";
                    txtItemID.Enabled = true;
                }
                else if (ddlTransactionType.Text == "Pallet Information Report")
                {
                    txtItemID.Text = "";
                    txtItemID.Enabled = true;
                }
            }
        }

        protected void searchbtn_Click(object sender, EventArgs e)
        {

        }
    }
}