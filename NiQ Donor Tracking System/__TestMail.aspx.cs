using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NiQ_Donor_Tracking_System
{
    public partial class __TestMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            string result = EMailHelper.SendEmail(toAddress.Text , body.Text, subject.Text);
            body.Text = result;
        }
    }
}