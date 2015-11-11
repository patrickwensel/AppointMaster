using System;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Hosting;
using LPI.DataBase;

namespace dashboard {
    public partial class Settings : System.Web.UI.Page {
        protected string getServerName() {
            try {
                if (Session["server"] == null) {
                    Session["server"] = Util.getServerName();
                }
                return (string)Session["server"];
            } catch (Exception ex) {
                throw new Exception("getServerName exception:" + ex.Message);
            }
        }



        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (!this.IsPostBack) {
                    if (Session["DB"] != null) {
                        if (Session["iFrame"] != null) {
                            this.PanelHeader.Visible = false;
                        }
                        this.linkLead.Text = "<a href=\"dashboard.aspx?p=4\"><span>Lead Generation</span></a>";
                        DashUtil util = new DashUtil();
                        string error = "";
                        PROI_WS.accountData amAct = util.getAmAccount((int)Session["DB"], Session, ref error);
                        if (amAct != null) {
                            using (SqlConnection cn = new SqlConnection()) {
                                cn.ConnectionString = Connect.connectString();
                                cn.Open();

                                Account act = new Account(cn);
                                if (act.DBGet(((Account)Session["Account"]).DB)) {
                                    Session["Account"] = act;
                                    this.AccountName.Text = act.name + " <small>" + act.DB.ToString() + "</small>";
                                    this.notificationEmail.Text = act.notificationEmail;
                                    this.labelMain.Text += "<div style='font-family:Arial;font-size:12px;text-align:left;margin:10px;'>";
                                    string SQLserver = getServerName();
                                    string DataBase = "LPI";
                                    this.cbDisplayDuplicates.Checked = SysOption.GetValue(cn, act.DB, "DASHBOARD_DISPLAY_DUPLICATE", false);
                                    this.cbDisplayBudget.Checked = SysOption.GetValue(cn, act.DB, "DisplayBudget", false);
                                    this.excludedCodes.Text = act.ExcludedCodes;
                                }
                                cn.Close();
                            }
                        } else {
                            this.labelMain.Text += "<br>Account Error:" + error;

                        }
                    } else {
                        Response.Redirect("login.aspx");
                    }
                }
            } catch (Exception ex) {
                this.labelMain.Text = "Exception "+ex.Message;
            }
        }

        protected void pbCancel_Click(object sender, EventArgs e) {
            Response.Redirect("dashboard.aspx?p=4");
        }

        protected void pbSave_Click(object sender, EventArgs e) {
            try {
                if (!Util.isValidEmailAddress(this.notificationEmail.Text))
                    throw new Exception("Invalid Email");
                DashUtil util = new DashUtil();
                string error = "";
                PROI_WS.accountData amAct = util.getAmAccount((int)Session["DB"], Session, ref error);
                if (amAct != null) {
                    string SQLserver = getServerName();
                    string DataBase = "LPI";
                    using (SqlConnection cn = new SqlConnection()) {
                        cn.ConnectionString = Connect.connectString();
                        cn.Open();
                        Account act = new Account(cn);
                        if (act.DBGet(((Account)Session["Account"]).DB)) {
                            SysOption.SetValue(cn, act.DB, "DASHBOARD_DISPLAY_DUPLICATE", this.cbDisplayDuplicates.Checked);
                            SysOption.SetValue(cn, act.DB, "DisplayBudget", this.cbDisplayBudget.Checked);
                            act.DBUpdate("notificationEmail='" + this.notificationEmail.Text + "'");
                            act.ExcludedCodes = this.excludedCodes.Text;
                        }
                        cn.Close();
                    }

                    Response.Redirect("dashboard.aspx?p=4");
                } else {
                    this.labelMain.Text += "<br>Account Error:" + error;

                }
                
            } catch (Exception ex) {
                this.labelMain.Text = "Exception " + ex.Message;
            }
        }

    }
}
