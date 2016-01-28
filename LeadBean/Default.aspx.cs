using System;
using DataBase;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;


namespace LeadBean
{
    public partial class _Default : System.Web.UI.Page
    {

        private int _db = -1;

        protected int Db
        {
            get
            {
                if (_db == -1)
                {
                    if (Session["account"] == null)
                        throw new Exception("DB not defined");
                    _db = ((Account)Session["account"]).DB;
                }
                return _db;
            }
            set
            {
                _db = (int)value;
            }
        }

        protected void RadGrid2_Load(object sender, EventArgs e)
        {
            try
            {
                RadGrid2.ClientSettings.Selecting.AllowRowSelect = true;
                RadGrid2.AllowMultiRowSelection = true;
                RadGrid2.ClientSettings.Selecting.UseClientSelectColumnOnly = true;

                this.Leads.ConnectionString = Util.GetConfigValue("connnectionstring");
                this.Leads.SelectParameters["DB"].DefaultValue = Db.ToString();
                this.Leads.SelectParameters["TABLE"].DefaultValue = "Lead";
                string table = "LEAD";
                string command = " <div class='actionButton' title='Edit-{0}'><i class='fa fa-pencil actionIcon'></i></div>";
                string URL = "CONFIRM.aspx?DELID={0}";
                if (Request["leadbean"] != null)
                {
                    table = "leadbean";
                    command = "<b>Restore - {0}</b>";
                    URL = "CONFIRM.aspx?RESTOREID={0}";
                }
                this.Leads.SelectCommand = string.Format(@"
                    select 
                        CASE
	                        WHEN c.name is not null
		                        THEN c.name 
		                        ELSE '-'
                        END  as 'Camp', inComingNumber as 'Incoming', timeStamp as 'Time Stamp', durationMinutes as 'Duration', firstName, lastName,email,fileURL, birthday as 'DOB',
                    PrimaryPhone as 'Prim. Phone',alterPrimaryPhone as 'alt. Prim. Phone', l.ID 
                    from {0} as l
                    LEFT join campaign as c on c.Id=campaignID and c.db = l.db
                     where l.DB={1}", table, Db.ToString());

                Telerik.Web.UI.GridHyperLinkColumn col = (Telerik.Web.UI.GridHyperLinkColumn)this.RadGrid2.Columns[1];
                col.DataTextFormatString = command;
                col.DataNavigateUrlFormatString = URL;
            }
            catch (Exception ex)
            {
                throw new Exception("RadGrid1_Load ex:" + ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["account"] != null)
                {
                    if (!this.IsPostBack)
                    {
                        if (Request["leadbean"] != null)
                        {
                            this.Title.Text = "<u>Deleted</u> Leads for Account #" + ((Account)Session["account"]).DB.ToString() + " " + ((Account)Session["account"]).name;
                            this.LinkSwitch.Text = "Show Leads";
                            this.LinkSwitch.NavigateUrl = "default.aspx";
                        }
                        else
                        {
                            this.Title.Text = "Leads for Account #" + ((Account)Session["account"]).DB.ToString() + " " + ((Account)Session["account"]).name;
                            this.LinkSwitch.Text = "Show Deleted Leads";
                            this.LinkSwitch.NavigateUrl = "default.aspx?leadbean=1";
                        }
                        if (Request["DELID"] != null)
                        {
                            Response.Redirect("confirm.aspx?DELID=" + Request["DELID"]);
                        }
                    }
                }
                else
                    Response.Redirect("login.aspx");
            }
            catch (Exception ex)
            {
                this.LabelError.Text = "Exception:" + ex.Message;
            }
        }

        List<int> GetSelectedLeadIds()
        {
            List<int> ids = new List<int>();
            var selectedGridItems = RadGrid2.SelectedItems;
            foreach (GridDataItem item in selectedGridItems)
            {
                int leadId;
                if (int.TryParse(item["ID"].Text, out leadId))
                {
                    ids.Add(leadId);
                }
            }
            return ids;
        }

        protected void btnMoveToCampaign_Click(object sender, EventArgs e)
        {
            RadDropDownList ddlCampaign = null;
            List<int> leadIdsToMove = GetSelectedLeadIds();
            foreach (GridFilteringItem filterItem in RadGrid2.MasterTableView.GetItems(GridItemType.FilteringItem))
            {
                ddlCampaign = (RadDropDownList)filterItem.FindControl("ddlCampaign");
                if (ddlCampaign != null)
                {
                    int campaignId;

                    if (int.TryParse(ddlCampaign.SelectedValue, out campaignId))
                    {
                        try
                        {
                            using (SqlConnection cn = Connect.getDefaultConnection())
                            {
                                if (leadIdsToMove.Count > 0)
                                {
                                    string where = "WHERE ";
                                    foreach (var item in leadIdsToMove)
                                    {
                                        where += "id = " + item + " OR ";
                                    }
                                    int index = where.LastIndexOf("OR");
                                    where = where.Remove(index);

                                    string sql = string.Format("UPDATE LEAD SET campaignID = {0} {1}", campaignId, where);
                                    SqlCommand cmd = new SqlCommand(sql, cn);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                    cmd = null;
                                    cn.Close();
                                    Response.Redirect(Context.Request.Url.AbsoluteUri);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.LabelError.Text = "Exception:" + ex.Message;
                        }
                    }
                    return;
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> leadIdsToDelete = GetSelectedLeadIds();
            using (SqlConnection cn = Connect.getDefaultConnection())
            {
                if (leadIdsToDelete.Count > 0)
                {
                    string where = "WHERE ";
                    foreach (var item in leadIdsToDelete)
                    {
                        where += "id = " + item + " OR ";
                    }
                    int index = where.LastIndexOf("OR");
                    where = where.Remove(index);

                    string insertSql = string.Format("INSERT INTO LeadBean SELECT * FROM LEAD {0}", where);
                    SqlCommand insertCmd = new SqlCommand(insertSql, cn);
                    insertCmd.ExecuteNonQuery();
                    insertCmd.Dispose();
                    insertCmd = null;

                    string deleteSql = string.Format("DELETE FROM LEAD {0}", where);
                    SqlCommand deleteCmd = new SqlCommand(deleteSql, cn);
                    deleteCmd.ExecuteNonQuery();
                    deleteCmd.Dispose();
                    deleteCmd = null;
                    cn.Close();

                    Response.Redirect(Context.Request.Url.AbsoluteUri);
                }
            }
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            List<int> leadIdsToRestore = GetSelectedLeadIds();

            using (SqlConnection cn = Connect.getDefaultConnection())
            {
                if (leadIdsToRestore.Count > 0)
                {
                    string where = "WHERE ";
                    foreach (var item in leadIdsToRestore)
                    {
                        where += "id = " + item + " OR ";
                    }
                    int index = where.LastIndexOf("OR");
                    where = where.Remove(index);

                    string insertSql = string.Format("INSERT INTO LEAD SELECT * FROM LeadBean {0}", where);
                    SqlCommand insertCmd = new SqlCommand(insertSql, cn);
                    insertCmd.ExecuteNonQuery();
                    insertCmd.Dispose();
                    insertCmd = null;

                    string deleteSql = string.Format("DELETE FROM LeadBean {0}", where);
                    SqlCommand deleteCmd = new SqlCommand(deleteSql, cn);
                    deleteCmd.ExecuteNonQuery();
                    deleteCmd.Dispose();
                    deleteCmd = null;
                    cn.Close();

                    Response.Redirect(Context.Request.Url.AbsoluteUri);
                }
            }
        }

        protected void ddlCampaign_Load(object sender, EventArgs e)
        {
            RadDropDownList ddlCampaign = (RadDropDownList)sender;

            using (SqlConnection cn = Connect.getDefaultConnection())
            {
                string sql = string.Format("SELECT ID, name FROM CAMPAIGN WHERE DB = {0}", Db.ToString());
                SqlCommand cmd = new SqlCommand(sql, cn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ddlCampaign.Items.Add(new DropDownListItem("<div class='moveCampaignItem'>" + reader["name"] + "</div>", reader["ID"].ToString()));
                }
                cmd.Dispose();
                cmd = null;
                cn.Close();
            }
        }

        protected void btnDeleteCampaign_Click(object sender, EventArgs e)
        {
            var campaignIdToDelete = Request["__EVENTARGUMENT"];

            using (SqlConnection cn = Connect.getDefaultConnection())
            {
                string insertLeadSql = "INSERT INTO LeadBean SELECT * FROM LEAD WHERE campaignID = " + campaignIdToDelete;
                SqlCommand insertLeadCmd = new SqlCommand(insertLeadSql, cn);
                insertLeadCmd.ExecuteNonQuery();
                insertLeadCmd.Dispose();
                insertLeadCmd = null;

                string deleteLeadSql = "DELETE FROM LEAD WHERE campaignID = " + campaignIdToDelete;
                SqlCommand deleteLeadCmd = new SqlCommand(deleteLeadSql, cn);
                deleteLeadCmd.ExecuteNonQuery();
                deleteLeadCmd.Dispose();
                deleteLeadCmd = null;

                string deleteCampaignSql = "DELETE FROM CAMPAIGN WHERE ID = " + campaignIdToDelete;
                SqlCommand deleteCampaignCmd = new SqlCommand(deleteCampaignSql, cn);
                deleteCampaignCmd.ExecuteNonQuery();
                deleteCampaignCmd.Dispose();
                deleteCampaignCmd = null;

                cn.Close();

                Response.Redirect(Context.Request.Url.AbsoluteUri);
            }
        }

        protected void ddlDeleteCampaign_Load(object sender, EventArgs e)
        {
            RadDropDownList ddlCampaign = (RadDropDownList)sender;
            ddlCampaign.Items.Clear();
            using (SqlConnection cn = Connect.getDefaultConnection())
            {
                string sql = string.Format("SELECT ID, name FROM CAMPAIGN WHERE DB = {0}", Db.ToString());
                SqlCommand cmd = new SqlCommand(sql, cn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ddlCampaign.Items.Add(new DropDownListItem("<div class='campaignItem deleteCampaignItem'>" + reader["name"] + "</div>", reader["ID"].ToString()));
                }
                cmd.Dispose();
                cmd = null;
                cn.Close();
            }
        }
    }
}
