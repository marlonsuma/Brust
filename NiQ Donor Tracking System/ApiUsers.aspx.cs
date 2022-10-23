using System;
using System.Security.Claims;
using System.Text;
using System.Web;
using DonorTracking.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace NiQ_Donor_Tracking_System
{
    public partial class ApiUsers : System.Web.UI.Page
    {
        private readonly NiqUserManager _userManager = HttpContext.Current.GetOwinContext().Get<NiqUserManager>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.UserInfo ui = (Global.UserInfo)Session["ui"];
                if (ui.Administrator == false)
                {
                    Response.Redirect("Default.aspx");

                    return;
                }
                UserName.Focus();
                ResultMessage.Visible = false;
            }
            
        }

        protected async void Submit_Click(object sender, EventArgs e)
        {
            //null user
            if (string.IsNullOrEmpty(UserName.Text))
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "Please enter a user name";
                UserName.Focus();
                return;
            }

            //null password
            if (string.IsNullOrEmpty(Password.Text))
            {
                ResultMessage.Visible = true; 
                ResultMessage.Text = "Please enter a password";
                Password.Focus();
                return;
            }
            //password too short
            if (Password.Text.Length < 8)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "Password must be at least 8 characters long";
                Password.Focus();
                return;
            }
            //password bad format
            if (!(await _userManager.PasswordValidator.ValidateAsync(Password.Text)).Succeeded)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "Password must contain a number, upper case letter, and lower case letter";
                Password.Focus();
                return;
            }

            try
            {
                User addedUser = new User(UserName.Text);
                IdentityResult newIdentityResult = await _userManager.CreateAsync(addedUser, Password.Text);

                if (newIdentityResult.Succeeded)
                {
                    await _userManager.AddClaimAsync(addedUser.Id, new Claim("ApiUser", "1"));
                    ResultMessage.Visible = true;
                    ResultMessage.Text = "Api user created";
                }
                else
                {
                    StringBuilder errorBuilder = new StringBuilder();
                    errorBuilder.AppendLine("Error occured creating API user: ");
                    foreach (string error in newIdentityResult.Errors)
                    {
                        errorBuilder.AppendLine(error);
                    }

                    ResultMessage.Visible = true;
                    ResultMessage.Text = errorBuilder.ToString();
                }
            }
            catch //(Exception ex)
            {
                ResultMessage.Visible = true;
                ResultMessage.Text = "An error occured when creating the new API user";
            }

        }

        protected void Cancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }
    }
}