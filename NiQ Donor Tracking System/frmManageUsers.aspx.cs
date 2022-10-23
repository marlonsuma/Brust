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
    public partial class frmManageUsers : System.Web.UI.Page
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
                ClearUserInfo();
                LoadUsers();
                btnSave.Enabled = true;
                txtUsername.Focus();
                lstExistingUsers.SelectedIndex = 0;
                lstExistingUsers_SelectedIndexChanged(null, null);
            }
        }

        private void LoadUsers()
        {
            lblMessage.Text = "";

            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            conn.Open();
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM tblUsers ORDER BY [Username]";
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;
            lstExistingUsers.Items.Clear();

            try
            {
                dr = sqlCommand.ExecuteReader();
                lstExistingUsers.Items.Add("+ Add New User");
                while (dr.Read())
                {
                    ListItem li = new ListItem();
                    li.Text = dr["Username"].ToString();
                    li.Value = dr["ID"].ToString();
                    lstExistingUsers.Items.Add(li);
                }
                dr.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }
        }

        private void ClearUserInfo()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            lblSubPassword.Visible = false;
            radAdministrator.Checked = false;
            radUser.Checked = true;
            radActive.Checked = true;
            radInactive.Checked = false;
            btnSave.Enabled = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }

        protected void lstExistingUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstExistingUsers.SelectedIndex == 0)
            {
                ClearUserInfo();
                btnSave.Text = "Add";
                lblPassword.Text = "Password";
                lblSubPassword.Visible = false;
                txtUsername.Focus();
                btnSave.Enabled = true;
            }
            else if (lstExistingUsers.SelectedIndex > 0)
            {
                ShowUserInfo();
                btnSave.Text = "Save";
                lblPassword.Text = "New Password";
                lblSubPassword.Visible = true;
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void ShowUserInfo()
        {
            lblMessage.Text = "";
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM tblUsers WHERE ID = " +
                                      lstExistingUsers.SelectedItem.Value.ToString();
            sqlCommand.Connection = conn;
            System.Data.SqlClient.SqlDataReader dr;

            try
            {
                conn.Open();
                dr = sqlCommand.ExecuteReader();
                dr.Read();
                if (dr.HasRows == false)
                {
                    lblMessage.Text = "Could not find user information.";
                    return;
                }
                // Show the info
                txtUsername.Text = dr["Username"].ToString();
                radAdministrator.Checked = (bool)dr["Administrator"];
                radUser.Checked = !(bool)dr["Administrator"];
                radActive.Checked = (bool)dr["Active"];
                radInactive.Checked = !(bool)dr["Active"];

                dr.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = (ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            string strPattern = "[ \"]";

            #region Input Validation
            // Username Validation
            if (txtUsername.Text.Trim().Length == 0)
            {
                lblMessage.Text = "Please enter a Username.";
                txtUsername.Focus();
                return;
            }
            else if (Regex.IsMatch(txtUsername.Text, strPattern))
            {
                lblMessage.Text = "Usernames cannot contain a space or a double quote.";
                txtUsername.Focus();
                return;
            }

            // Password Validation
            if (lstExistingUsers.SelectedIndex == 0)
            {
                if (txtPassword.Text.Trim().Length == 0)
                {
                    lblMessage.Text = "Please enter a Password.";
                    txtPassword.Focus();
                    return;
                }
            }
            if (Regex.IsMatch(txtUsername.Text, strPattern))
            {
                lblMessage.Text = "Passwords cannot contain a space or a double quote.";
                txtPassword.Focus();
                return;
            }
            #endregion

            #region Check for duplicate users or if we are removing the last Administrator
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            conn.Open();
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM tblUsers";
            sqlCommand.Connection = conn;

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
            if (lstExistingUsers.SelectedIndex == 0)
            {
                // Need to figure out if Username is unique
                foreach (DataRow datarow in dt.Rows)
                {
                    if (datarow["Username"].ToString().ToLower() == txtUsername.Text.ToLower())
                    {
                        lblMessage.Text = "The requested Username is already in use" +
                                          "; please enter another one.";
                        txtUsername.Focus();
                        return;
                    }
                }
            }
            else
            {
                // Need to make sure Username is unique and that there is at least one active
                // Admin left
                int intActiveAdmins = 0;
                foreach (DataRow datarow in dt.Rows)
                {
                    if (datarow["Username"].ToString().ToLower() == txtUsername.Text.ToLower())
                    {
                        if (datarow["ID"].ToString() != lstExistingUsers.SelectedItem.Value.ToString())
                        {
                            lblMessage.Text = "The requested Username is already in use" +
                                              "; please enter another one.";
                            txtUsername.Focus();
                            return;
                        }
                    }
                    if (((bool)datarow["Administrator"]) &&
                        (datarow["ID"].ToString() != lstExistingUsers.SelectedItem.Value.ToString()) &&
                        (bool)datarow["Active"])
                    {
                        intActiveAdmins++;
                    }
                }

                if ((radAdministrator.Checked == false && intActiveAdmins == 0) ||
                    (radInactive.Checked && intActiveAdmins == 0))
                {
                    lblMessage.Text = "There must at least be one active Admin. " +
                                      "The user being edited must be an Admin.";
                    return;
                }
            }
            #endregion

            #region Create or Update User
            // Now update the user table
            conn.Open();
            int intAdmin = radAdministrator.Checked ? 1 : 0;
            int intActive = radActive.Checked ? 1 : 0;

            if (lstExistingUsers.SelectedIndex == 0)
            {
                try
                {
                    sqlCommand.CommandText = "INSERT INTO tblUsers (Username, Password, " +
                                             "Administrator, Active) VALUES('" +
                                              txtUsername.Text.Trim() + "','" +
                                              txtPassword.Text.Trim() + "'," +
                                              intAdmin.ToString() + ", " +
                                              intActive.ToString() + ")";
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
            else if (lstExistingUsers.SelectedIndex > 0)
            {
                try
                {
                    if (txtPassword.Text.Trim().Length > 0)
                    {
                        sqlCommand.CommandText = "UPDATE tblUsers SET " +
                                                 "Username = '" + txtUsername.Text.Trim() + "', " +
                                                 "Password = '" + txtPassword.Text.Trim() + "', " +
                                                 "Administrator = " + intAdmin.ToString() + ", " +
                                                 "Active = " + intActive.ToString() +
                                                 " WHERE ID = " + lstExistingUsers.SelectedItem.Value.ToString();
                    }
                    else
                    {
                        sqlCommand.CommandText = "UPDATE tblUsers SET " +
                                                 "Username = '" + txtUsername.Text.Trim() + "', " +
                                                 "Administrator = " + intAdmin.ToString() + ", " +
                                                 "Active = " + intActive.ToString() +
                                                 " WHERE ID = " + lstExistingUsers.SelectedItem.Value.ToString();
                    }

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

            LoadUsers();
            ClearUserInfo();
            lstExistingUsers.SelectedIndex = 0;
            lstExistingUsers_SelectedIndexChanged(null, null);
            lblMessage.Text = "User updated successfully.";
        }
    }
}