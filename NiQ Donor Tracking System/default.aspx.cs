using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NiQ_Donor_Tracking_System
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];

            if (ui.ID == 0)
            {
                btnBack_Click(this, null);
            }
            btnAdmin.Visible = ui.Administrator;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Global.UserInfo ui = (Global.UserInfo)Session["ui"];
            ui.Administrator = false;
            ui.ID = 0;
            ui.Username = string.Empty;
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnCreateBloodKit_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCreateBloodKit.aspx");
        }

        protected void btnReceiveBloodKit_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmReceiveBloodKit.aspx");
        }

        protected void btnCreateMilkKit_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCreateMilkKit.aspx");
        }

        protected void btnReceiveMilkKit_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmReceiveMilkKit.aspx");
        }

        protected void btnQuarantineMilk_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmQuarantineMilk.aspx");
        }

        protected void btnMilkTestResults_Click(object sender, EventArgs e)
        {
            Response.Redirect("LabResults.aspx");
        }

        protected void btnCreateLot_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCreateLot.aspx");
        }

        protected void btnShipCase_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmShipCase.aspx");
        }

        protected void btnReporting_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmReportParameters.aspx");
        }

        protected void btnAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmReprintLabels.aspx");
        }

        protected void btnInventoryProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("Search.aspx");
        }

        protected void btnLotTransfer_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLotTransfer.aspx");
        }

        protected void btnMilkKitPayments_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmMilkKitPayments.aspx");
        }

        protected void btnDonorForms_Click(object sender, EventArgs e)
        {
            Response.Redirect("DonorForms.aspx");
        }

        protected void btnLabOrders_click(object sender, EventArgs e)
        {
            Response.Redirect("LabOrder.aspx");
        }

        protected void btnReturnCase_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReturnCase.aspx");
        }

        protected void btnCreatePallet_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCreatePallet.aspx");
        }
    }
}