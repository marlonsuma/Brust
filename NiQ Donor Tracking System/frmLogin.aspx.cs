using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;

namespace NiQ_Donor_Tracking_System
{
    public partial class frmLogin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string strPattern = "[' \"]";
            string password = "";
            bool blnActive = false, blnAdmin = false;
            int intUserId = 0;
            Global.UserInfo ui = (Global.UserInfo) Session["ui"];

            // Clear the message
            ResultMessage.Text = "";

            #region Validate Input

            // See if user provided all the info
            if (txtUsername.Text.Length == 0)
            {
                ResultMessage.Text = "Please enter a username.";
                txtUsername.Focus();

                return;
            }

            if (Regex.IsMatch(txtUsername.Text, strPattern))
            {
                ResultMessage.Text = "Username cannot contain a space, a single or a double quote.";
                txtUsername.Focus();

                return;
            }

            if (txtPassword.Text.Length == 0)
            {
                ResultMessage.Text = "Please enter a password.";
                txtPassword.Focus();

                return;
            }

            #endregion

            #region Setup Database Connection

            string connection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand sqlCommand = new SqlCommand { CommandType = CommandType.Text, Connection = conn };
            
            #endregion

            #region Check if User is Active and Password matches

            // Read User information from database
            try
            {
                conn.Open();
                sqlCommand.CommandText = "SELECT * FROM tblUsers WHERE [UserName]='" + txtUsername.Text.Trim() + "'";

                using (SqlDataReader dr = sqlCommand.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        password = dr["Password"].ToString();
                        blnActive = dr["Active"].ToString() == "True";
                        blnAdmin = dr["Administrator"].ToString() == "True";
                        intUserId = (int)dr["ID"];
                    }
                }
            }
            catch (Exception ex)
            {
                ResultMessage.Text = ex.Message;
                conn.Dispose();

                return;
            }
            finally
            {
                conn.Close();
            }

            // Check Password and Active Status
            if (password == txtPassword.Text.Trim())
            {
                if (blnActive == false)
                {
                    ResultMessage.Text = "User account is not active.  Please contact your administrator.";
                    txtPassword.Focus();

                    return;
                }
            }
            else
            {
                ResultMessage.Text = "Invalid login information.  Please try again.";
                txtPassword.Focus();

                return;
            }

            #endregion

            // Set user information
            ui.ID = intUserId;
            ui.Username = txtUsername.Text.Trim();
            ui.Administrator = blnAdmin;

            // Show the main page
            FormsAuthentication.RedirectFromLoginPage(ui.ID.ToString(), false);
        }
    }
}