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

namespace NiQ_Donor_Tracking_System
{
    public partial class frmManagePrinters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                if (ui.Administrator == false)
                {
                    Response.Redirect("Default.aspx");
                }
                ClearPrinterInfo();
                LoadPrinters();
                btnSave.Enabled = true;
                txtPrinterName.Focus();
                lstExistingPrinters.SelectedIndex = 0;
                lstExistingPrinters_SelectedIndexChanged(null, null);
            }
        }

        private void ClearPrinterInfo()
        {
            txtPrinterName.Text = "";
            txtIP.Text = "";
            btnSave.Enabled = false;
        }

        protected void lstExistingPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstExistingPrinters.SelectedIndex == 0)
            {
                ClearPrinterInfo();
                btnSave.Text = "Add";
                btnRemove.Visible = false;
                txtPrinterName.Focus();
                btnSave.Enabled = true;
            }
            else if (lstExistingPrinters.SelectedIndex > 0)
            {
                ShowPrinterInfo();
                btnSave.Text = "Save";
                btnRemove.Visible = true;
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void ShowPrinterInfo()
        {
            lblMessage.Text = "";
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM tblPrinters WHERE ID = " +
                                      lstExistingPrinters.SelectedItem.Value.ToString();
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;

            try
            {
                conn.Open();
                dr = sqlCommand.ExecuteReader();
                dr.Read();
                if (dr.HasRows == false)
                {
                    lblMessage.Text = "Could not find printer information.";
                    return;
                }
                // Show the info
                txtPrinterName.Text = dr["PrinterName"].ToString();
                txtIP.Text = dr["PrinterIP"].ToString();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String strPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
            lblMessage.Text = "";

            #region Input Validation
            // Printer Name Validation
            if (txtPrinterName.Text.Trim().Length == 0)
            {
                lblMessage.Text = "Please enter a valid Printer Name";
                txtPrinterName.Focus();
                return;
            }

            // IP Address Validation
            if (!Regex.IsMatch(txtIP.Text, strPattern))
            {
                lblMessage.Text = "Please enter a valid IP Address";
                txtIP.Focus();
                return;
            }
            #endregion

            #region Check for duplicate information
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM tblPrinters";
            sqlCommand.Connection = conn;
            conn.Open();

            DataTable dt = new DataTable("temp");
            SqlDataAdapter da;
            try
            {
                da = new SqlDataAdapter(sqlCommand);
                da.Fill(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            // See if we're adding or editing			
            if (lstExistingPrinters.SelectedIndex == 0)
            {
                // Need to figure out if Printer Name is unique
                foreach (DataRow datarow in dt.Rows)
                {
                    if (datarow["PrinterName"].ToString().ToLower() == txtPrinterName.Text.ToLower())
                    {
                        lblMessage.Text = "The requested Printer Name is already in use" +
                                          "; please enter another one.";
                        txtPrinterName.Focus();
                        return;
                    }
                    else if (datarow["PrinterIP"].ToString().ToLower() == txtIP.Text.ToLower())
                    {
                        lblMessage.Text = "The requested IP Address is already in use" +
                                          "; please enter another one.";
                        txtIP.Focus();
                        return;
                    }
                }
            }
            else
            {
                foreach (DataRow datarow in dt.Rows)
                {
                    if (datarow["PrinterName"].ToString().ToLower() == txtPrinterName.Text.ToLower())
                    {
                        if (datarow["ID"].ToString() != lstExistingPrinters.SelectedItem.Value.ToString())
                        {
                            lblMessage.Text = "The requested Printer Name is already in use" +
                                              "; please enter another one.";
                            txtPrinterName.Focus();
                            return;
                        }
                    }
                    else if (datarow["PrinterIP"].ToString().ToLower() == txtIP.Text.ToLower())
                    {
                        if (datarow["ID"].ToString() != lstExistingPrinters.SelectedItem.Value.ToString())
                        {
                            lblMessage.Text = "The requested IP Address is already in use" +
                                              "; please enter another one.";
                            txtIP.Focus();
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Create or Update Printer
            conn.Open();
            if (lstExistingPrinters.SelectedIndex == 0)
            {
                try
                {
                    sqlCommand.CommandText = "INSERT INTO tblPrinters (PrinterName, PrinterIP) " +
                                             "VALUES('" +
                                              txtPrinterName.Text.Trim() + "','" +
                                              txtIP.Text.Trim() + "');";

                    sqlCommand.ExecuteNonQuery();
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
            }
            else if (lstExistingPrinters.SelectedIndex > 0)
            {
                try
                {

                    sqlCommand.CommandText = "UPDATE tblPrinters SET " +
                                                "PrinterName = '" + txtPrinterName.Text.Trim() + "', " +
                                                "PrinterIP = '" + txtIP.Text.Trim() + "'" +
                                                " WHERE ID = " + lstExistingPrinters.SelectedItem.Value.ToString();
                    sqlCommand.ExecuteNonQuery();
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
            }
            #endregion

            LoadPrinters();
            ClearPrinterInfo();
            lstExistingPrinters.SelectedIndex = 0;
            lstExistingPrinters_SelectedIndexChanged(null, null);
            lblMessage.Text = "Printer updated successfully";
        }

        private void LoadPrinters()
        {
            lblMessage.Text = "";

            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            conn.Open();
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM tblPrinters ORDER BY [PrinterName]";
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            lstExistingPrinters.Items.Clear();

            try
            {
                dr = sqlCommand.ExecuteReader();
                lstExistingPrinters.Items.Add("+ Add New Printer");
                while (dr.Read())
                {
                    ListItem li = new ListItem();
                    li.Text = dr["PrinterName"].ToString();
                    li.Value = dr["ID"].ToString();
                    lstExistingPrinters.Items.Add(li);
                }
                dr.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstExistingPrinters.SelectedIndex > 0)
            {
                string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
                System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = conn;
                conn.Open();

                try
                {
                    sqlCommand.CommandText = "DELETE FROM tblPrinters WHERE [ID]=" +
                                                lstExistingPrinters.Items[lstExistingPrinters.SelectedIndex].Value.ToString();

                    sqlCommand.ExecuteNonQuery();
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

                LoadPrinters();
                ClearPrinterInfo();
                lstExistingPrinters.SelectedIndex = 0;
                lstExistingPrinters_SelectedIndexChanged(null, null);
                lblMessage.Text = "Printer removed successfully";
            }
        }
    }
}