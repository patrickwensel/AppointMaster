using System;
using DataBase;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;


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
                    select c.name as 'Camp', inComingNumber as 'Incoming', timeStamp as 'Time Stamp', durationMinutes as 'Duration', firstName, lastName,email,fileURL, birthday as 'DOB',
                    PrimaryPhone as 'Prim. Phone',alterPrimaryPhone as 'alt. Prim. Phone', l.ID 
                    from {0} as l
                    join campaign as c on c.Id=campaignID
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
                        DataBase.Data.LPIContext context = new LPI.DataBase.Data.LPIContext();

                        foreach (var item in leadIdsToMove)
                        {
                            var lead = context.LEAD.FirstOrDefault(x => x.ID == item);
                            if (lead != null)
                            {
                                lead.campaignID = campaignId;
                            }
                        }

                        context.SaveChanges();
                        Response.Redirect(Context.Request.Url.AbsoluteUri);
                    }
                    return;
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LPI.DataBase.Data.LPIContext context = new LPI.DataBase.Data.LPIContext();
            List<int> leadIdsToDelete = GetSelectedLeadIds();
            foreach (var item in leadIdsToDelete)
            {
                var lead = context.LEAD.FirstOrDefault(x => x.ID == item);
                if (lead != null)
                {
                    context.AddToLeadBeans(new LPI.DataBase.Data.LeadBean()
                    {
                        DB = lead.DB,
                        alterFirstName = lead.alterFirstName,
                        alterName = lead.alterName,
                        alterPrimaryPhone = lead.alterPrimaryPhone,
                        birthday = lead.birthday,
                        campaign = lead.campaign,
                        campaignID = lead.campaignID,
                        content = lead.content,
                        dentalCareIsFor = lead.dentalCareIsFor,
                        dentalNeed = lead.dentalNeed,
                        durationMinutes = lead.durationMinutes,
                        email = lead.email,
                        fileURL = lead.fileURL,
                        firstName = lead.firstName,
                        ID = lead.ID,
                        inComingNumber = lead.inComingNumber,
                        insurancePlanBudget = lead.insurancePlanBudget,
                        lastName = lead.lastName,
                        MatchingStatus = lead.MatchingStatus,
                        medium = lead.medium,
                        newPatient = lead.newPatient,
                        original = lead.original,
                        patientId = lead.patientId,
                        preferredAppointmentTime = lead.preferredAppointmentTime,
                        PrimaryPhone = lead.PrimaryPhone,
                        referred_by = lead.referred_by,
                        source = lead.source,
                        term = lead.term,
                        timeStamp = lead.timeStamp
                    });
                    context.DeleteObject(lead);
                }
            }
            context.SaveChanges();
            Response.Redirect(Context.Request.Url.AbsoluteUri);
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            LPI.DataBase.Data.LPIContext context = new LPI.DataBase.Data.LPIContext();
            List<int> leadIdsToDelete = GetSelectedLeadIds();
            foreach (var item in leadIdsToDelete)
            {
                var lead = context.LeadBeans.FirstOrDefault(x => x.ID == item);
                if (lead != null)
                {
                    context.AddToLEAD(new LPI.DataBase.Data.LEAD()
                    {
                        DB = lead.DB,
                        alterFirstName = lead.alterFirstName,
                        alterName = lead.alterName,
                        alterPrimaryPhone = lead.alterPrimaryPhone,
                        birthday = lead.birthday,
                        campaign = lead.campaign,
                        campaignID = lead.campaignID,
                        content = lead.content,
                        dentalCareIsFor = lead.dentalCareIsFor,
                        dentalNeed = lead.dentalNeed,
                        durationMinutes = lead.durationMinutes,
                        email = lead.email,
                        fileURL = lead.fileURL,
                        firstName = lead.firstName,
                        ID = lead.ID,
                        inComingNumber = lead.inComingNumber,
                        insurancePlanBudget = lead.insurancePlanBudget,
                        lastName = lead.lastName,
                        MatchingStatus = lead.MatchingStatus,
                        medium = lead.medium,
                        newPatient = lead.newPatient,
                        original = lead.original,
                        patientId = lead.patientId,
                        preferredAppointmentTime = lead.preferredAppointmentTime,
                        PrimaryPhone = lead.PrimaryPhone,
                        referred_by = lead.referred_by,
                        source = lead.source,
                        term = lead.term,
                        timeStamp = lead.timeStamp
                    });
                    context.DeleteObject(lead);
                }
            }
            context.SaveChanges();
            Response.Redirect(Context.Request.Url.AbsoluteUri);
        }

        protected void ddlCampaign_Load(object sender, EventArgs e)
        {
            RadDropDownList ddlCampaign = (RadDropDownList)sender;
            LPI.DataBase.Data.LPIContext context = new LPI.DataBase.Data.LPIContext();
            var campaigns = context.Campaigns.Where(x => x.AccountID == Db).ToList();
            foreach (var item in campaigns)
            {
                ddlCampaign.Items.Add(new DropDownListItem("<div class='campaignItem'>" + item.name + "</div>", item.ID.ToString()));
            }
        }
    }
}
