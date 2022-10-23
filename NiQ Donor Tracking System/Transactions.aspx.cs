using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.IO;


namespace NiQ_Donor_Tracking_System
{
    public partial class Transactions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            GridView1.Visible = false;

           
          
        }

        protected void search_Click(object sender, EventArgs e)
        {
            int mk = int.Parse(Item.Text.Replace("MK000", ""));
            GridView1.Visible = true;
          
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            using (System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblTransactions t INNER JOIN tblTransactionDetails td ON t.ID = td.TransactionID WHERE itemID = " + mk, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                conn.Dispose();
                conn.Close();
            }

        }

        protected void back_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }
    }
}