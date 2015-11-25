using System;
using LPI.DataBase;


namespace LeadBean {
    public partial class _Default : System.Web.UI.Page {

        private int _db = -1;

        protected int Db {
            get {
                if (_db == -1) {
                    if (Session["account"]==null)
                        throw new Exception("DB not defined");
                    _db = ((Account)Session["account"]).DB;
                }
                return _db;
            }
            set{
                _db = (int)value;
            }
        }

        protected void RadGrid2_Load(object sender, EventArgs e) {
            try {
                this.Leads.ConnectionString = Util.GetConfigValue("connnectionstring");
                this.Leads.SelectParameters["DB"].DefaultValue = Db.ToString();
                this.Leads.SelectParameters["TABLE"].DefaultValue = "Lead";
                string table = "LEAD";
                string command = "<b>Edit-{0}</b>";
                string URL = "CONFIRM.aspx?DELID={0}";
                if (Request["leadbean"] != null) {
                    table = "leadbean";
                    command = "<b>Restore-{0}</b>";
                    URL="CONFIRM.aspx?RESTOREID={0}";
                }
                this.Leads.SelectCommand = "select campaignID as 'Camp', inComingNumber as 'Incoming', timeStamp as 'Time Stamp', durationMinutes as 'Duration', firstName, lastName,email,fileURL, birthday as 'DOB',PrimaryPhone as 'Prim. Phone',alterPrimaryPhone as 'alt. Prim. Phone', ID from " + table + " where DB= " + Db.ToString();
                Telerik.Web.UI.GridHyperLinkColumn col = (Telerik.Web.UI.GridHyperLinkColumn)this.RadGrid2.Columns[0];
                col.DataTextFormatString = command;
                col.DataNavigateUrlFormatString = URL;
            } catch (Exception ex) {
                throw new Exception("RadGrid1_Load ex:" + ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (Session["account"]!=null) {
                    if (!this.IsPostBack) {
                        if (Request["leadbean"] != null) {
                            this.Title.Text = "<u>Deleted</u> Leads for Account #" + ((Account)Session["account"]).DB.ToString() + " " + ((Account)Session["account"]).name;
                            this.LinkSwitch.Text = "Show Leads";
                            this.LinkSwitch.NavigateUrl = "default.aspx";
                        } else {
                            this.Title.Text = "Leads for Account #" + ((Account)Session["account"]).DB.ToString() + " " + ((Account)Session["account"]).name;
                            this.LinkSwitch.Text = "Show Deleted Leads";
                            this.LinkSwitch.NavigateUrl = "default.aspx?leadbean=1";
                        }
                        if (Request["DELID"] != null) {
                            Response.Redirect("confirm.aspx?DELID=" + Request["DELID"]);
                        }
                    }
                } else
                    Response.Redirect("login.aspx");
            } catch (Exception ex) {
                this.LabelError.Text = "Exception:" + ex.Message;
            }
        }

    }
}
