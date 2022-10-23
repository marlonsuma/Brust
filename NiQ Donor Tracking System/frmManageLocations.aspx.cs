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
using System.Net.Sockets;
using System.IO;
namespace NiQ_Donor_Tracking_System
{
    public partial class frmManageLocations : System.Web.UI.Page
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
                LoadRootLocations();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAdminMenu.aspx");
        }

        private void LoadRootLocations()
        {
            treeLocations.Nodes.Clear();
            treeLocations.Nodes.Add(new TreeNode("[Add New Root Location]", "NewRoot"));
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
            SqlCommand objCommand = new SqlCommand(@"SELECT * FROM tblLocations WHERE [ParentID]=0 AND [Active]=1", objConn);
            SqlDataAdapter da = new SqlDataAdapter(objCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            PopulateNodes(dt, treeLocations.Nodes);

            // Default to top selection
            treeLocations.Nodes[0].Select();
            txtParentLocation.Text = "";
            txtParentLocation.Enabled = false;
            btnSave.Enabled = false;
            btnRemove.Enabled = false;
            txtNewLocation.Focus();
        }

        private void PopulateSubLevel(int intParentID, TreeNode parentNode)
        {
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString());
            SqlCommand objCommand = new SqlCommand(@"SELECT * FROM tblLocations WHERE [ParentID]=" + intParentID.ToString() + " AND [Active]=1", objConn);
            SqlDataAdapter da = new SqlDataAdapter(objCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            PopulateNodes(dt, parentNode.ChildNodes);
        }


        protected void treeLocations_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            PopulateSubLevel(Int32.Parse(e.Node.Value.ToString()), e.Node);
        }

        private void PopulateNodes(DataTable dt, TreeNodeCollection nodes)
        {
            foreach (DataRow dr in dt.Rows)
            {
                TreeNode tn = new TreeNode();
                tn.Text = dr["Name"].ToString();
                tn.Value = dr["ID"].ToString();
                nodes.Add(tn);

                //If node has child nodes, then enable on-demand populating
                tn.PopulateOnDemand = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            #region Input Validation
            // Location Name Validation
            if (txtParentLocation.Text.Trim().Length == 0)
            {
                lblMessage.Text = "Please enter a valid Location Name.";
                txtParentLocation.Focus();
                return;
            }
            else if (txtParentLocation.Text.Trim() == treeLocations.SelectedNode.Text)
            {
                // Nothing to update
                lblMessage.Text = "No change detected.";
                return;
            }
            #endregion

            #region Update Location
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            conn.Open();

            try
            {

                sqlCommand.CommandText = "UPDATE tblLocations SET " +
                                            "Name = '" + txtParentLocation.Text.Trim() +
                                            "' WHERE ID = " + treeLocations.SelectedValue.ToString();
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
            #endregion

            treeLocations.SelectedNode.Text = txtParentLocation.Text.Trim();
            treeLocations.SelectedNode.Value = treeLocations.SelectedValue.ToString();
            lblMessage.Text = "Location updated successfully.";
        }

        protected void treeLocations_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (treeLocations.SelectedValue != "NewRoot")
            {
                txtParentLocation.Enabled = true;
                txtParentLocation.Text = treeLocations.SelectedNode.Text;
                btnSave.Enabled = true;
                btnRemove.Enabled = true;
            }
            else
            {
                txtParentLocation.Text = "";
                txtParentLocation.Enabled = false;
                btnSave.Enabled = false;
                btnRemove.Enabled = false;
            }

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            bool blnError = false;
            String strSelectedLocationID = "0";

            if (treeLocations.SelectedValue != "NewRoot")
            {
                strSelectedLocationID = treeLocations.SelectedValue.ToString();
            }

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            System.Data.SqlClient.SqlDataReader dr;
            sqlCommand.Connection = conn;
            #endregion

            #region Check for Child Locations
            try
            {
                sqlCommand.CommandText = "SELECT * FROM tblLocations WHERE [ParentID]=" + strSelectedLocationID + " AND [Active]=1";
                conn.Open();
                dr = sqlCommand.ExecuteReader();
                if (dr.HasRows)
                {
                    blnError = true;
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
            }

            if (blnError)
            {
                lblMessage.Text = "Unable to remove.  Location contains child locations that must be removed first.";
                conn.Dispose();
                return;
            }
            #endregion

            #region Check for Samples
            try
            {
                sqlCommand.CommandText = "SELECT * FROM tblSamples WHERE [LocationID]=" + strSelectedLocationID + " AND [Active]=1";
                conn.Open();
                dr = sqlCommand.ExecuteReader();
                if (dr.HasRows)
                {
                    blnError = true;
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
            }

            if (blnError)
            {
                lblMessage.Text = "Unable to remove.  Location contains samples that must be moved first.";
                conn.Dispose();
                return;
            }
            #endregion

            #region Remove Location
            try
            {
                conn.Open();
                sqlCommand.CommandText = "UPDATE tblLocations SET [Active]=0 WHERE [ID]=" +
                                            strSelectedLocationID;
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
            #endregion

            LoadRootLocations();
            lblMessage.Text = "Location removed successfully.";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            int intParentID = 0;
            String strSelectedLocationID = "0";

            #region Input Validation
            // Location Name Validation
            if (txtNewLocation.Text.Trim().Length == 0)
            {
                lblMessage.Text = "Please enter a valid Location Name.";
                txtNewLocation.Focus();
                return;
            }

            // Selected Location ID
            if (treeLocations.SelectedValue != "NewRoot")
            {
                strSelectedLocationID = treeLocations.SelectedValue.ToString();
            }

            #endregion

            #region Setup Database Connection
            string strConnection = ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ToString();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(strConnection);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = conn;
            #endregion

            #region Add Location
            if (treeLocations.SelectedValue != "NewRoot")
            {
                intParentID = Int32.Parse(strSelectedLocationID);
                strSelectedLocationID = treeLocations.SelectedValue.ToString();
            }
            try
            {
                conn.Open();
                sqlCommand.CommandText = "INSERT INTO tblLocations ([Name], [ParentID]) VALUES(' " +
                                          txtNewLocation.Text.Trim() + "'," +
                                          intParentID.ToString() + ")";
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
            #endregion

            txtNewLocation.Text = "";
            lblMessage.Text = "Location added successfully.";
            LoadRootLocations();
        }
    }
}