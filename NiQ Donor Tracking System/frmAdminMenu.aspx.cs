using System;

namespace NiQ_Donor_Tracking_System
{
    public partial class frmAdminMenu : System.Web.UI.Page
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
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnManageUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManageUsers.aspx");
        }

        protected void btnManagePrinters_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManagePrinters.aspx");
        }

        protected void btnManageLocations_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManageLocations.aspx");
        }

        protected void btnManageBloodKits_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManageBloodKits.aspx");
        }

        protected void btnManageMilkKits_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageMilkKit.aspx");
        }

        protected void btnManageDNAKits_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManageDNAKits.aspx");
        }

        protected void btnManageLots_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManageLots.aspx");
        }

        protected void btnManageCases_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManageCases.aspx");
        }

        protected void btnManageDonors_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageDonors.aspx");
        }

        protected void btnManageApiUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("ApiUsers.aspx");
        }

        protected void btnManagePallets_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmManagePallet.aspx");
        }

        protected void btnTransactions_Click(object sender, EventArgs e)
        {
            Response.Redirect("Transactions.aspx");
        }
    }
}