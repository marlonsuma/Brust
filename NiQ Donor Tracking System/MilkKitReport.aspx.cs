using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace NiQ_Donor_Tracking_System
{
    public partial class MilkKitReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SaveReportBtn_Click(object sender, EventArgs e)
        {
            string name = ReportName.Text;

            List<string> fields = new List<string>();

            foreach(ListItem field in MilkKitFields.Items)
            {
                fields.Add(field.Value);
            }

            foreach (ListItem field in DonorFields.Items)
            {
                fields.Add(field.Value);
            }

            foreach (ListItem field in LotFields.Items)
            {
                fields.Add(field.Value);
            }

            foreach (ListItem field in PalletFields.Items)
            {
                fields.Add(field.Value);
            }

            string fieldsJson = new JavaScriptSerializer().Serialize(fields);

            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            sqlCommand.CommandText = "INSERT INTO tblReports(Name, Fields, Conditions) VALUES('" + name + "', '" + fieldsJson + "', '" + fieldsJson + "')";

            try
            {

                conn.Open();
                sqlCommand.ExecuteNonQuery();
            }

            catch(Exception ex)
            {
                ErrorLabel.Visible = true;
                ErrorLabel.Text = ex.Message;
                conn.Dispose();
                return;
            }

            finally{
                conn.Close();
            }
          
        }
    }
}