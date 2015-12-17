using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataBase;

namespace dashboard {
    public partial class AM : System.Web.UI.Page {


        private void displayPage(string pageName) {
            try {
                string directory = ConfigurationManager.AppSettings["html_source_pages"];
                if (System.IO.Directory.Exists(directory)) {
                    string filename=directory + "\\" + pageName;
                    if (System.IO.File.Exists(filename))
                        this.labelMain.Text = Util.getTextFileContent(filename, false, false);
                    else
                        this.labelMain.Text = "File do not exist: " + filename;
                } else
                    this.labelMain.Text = "Directory do not exist: "+directory;

            } catch (Exception ex) {
                this.labelMain.Text = "displayPage Exception:" + ex.Message;
            }
        }
        
        
        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (Session["accountname"] != null) {
                    //this.panelSignup.Visible = false;
                    this.AccountName.Text = (string)Session["accountname"];
                    DashUtil util = new DashUtil();
                    string error = "";
                    PROI_WS.accountData amAct = util.getAmAccount((int)Session["DB"], Session, ref error);
                    Account act = (Account)Session["account"];

                    if (amAct != null) {
                        if (amAct.useReminder) {
                            this.panelSignup.Visible = false;
                            displayPage("AMLoginConfirm.htm");
                            
                            //Preparing return URL
                            string returnURL = Request.Url.ToString().Replace("AM.aspx", "login.aspx?direct="+HttpUtility.UrlEncode( Util.EncodeBase64String(act.DB.ToString())));
                            string URL=amAct.server + "/" + amAct.loginURLonServer + "&return=" + HttpUtility.UrlEncode( Util.EncodeBase64String(returnURL))+"&partner=LPI";
                            this.labelMain.Text = this.labelMain.Text.Replace("@LOGINURL", URL);
                            Response.Redirect(URL);
                        } else {
                            displayPage("AMSignup.htm");
                            this.panelSignup.Visible = true;
                        }
                    } else {
                        if ((Request["SignupConfirm"] != null) | (act.AMSubscribeReminders)) {
                            displayPage("AMSignupThankYou.htm");
                            this.panelSignup.Visible = false;
                        }else
                            displayPage("AMSignup.htm");//displayPage("AMAccount.htm");
                    }
                } else {
                    Response.Redirect("login.aspx");
                }
            } catch (Exception ex) {
                this.labelMain.Text = "General Exception:"+ex.Message;
            }
        }

        protected void pbSignup_Click(object sender, EventArgs e) {
            try {
                if (Util.isValidEmailAddress(this.email.Text)) {
                    Email email = new Email(0);
                    email.content = "New signup for reminder from account "+(string)Session["accountname"]+" <br><br>Email:"+this.email.Text;
                    email.content += "<br><br>LPI Account:#" + ((int)Session["DB"]).ToString();
                    email.content += "<br><br>From:" + Request.UserHostAddress;
                    email.content += "<br><br>Time:" + DateTime.Now.ToString();

                    if (email.sendTo(Util.GetConfigValue("FROM_NOTIFICATION"), "Admin on LPI Server",
                                      "cgissinger@appointmaster.com", "Christophe", "New Signup for Reminders", true,"")) {

                        using (SqlConnection cn = Connect.getDefaultConnection()) {
                            Account act = new Account(cn);
                            if (act.DBGet((int)Session["DB"])) {
                                act.DBUpdate("AMSubscribeReminders=1");
                            }
                            act = (Account)Session["account"];
                            act.AMSubscribeReminders = true;
                            cn.Close();
                        }
                        
                        Response.Redirect("AM.aspx?SignupConfirm=1");
                    } else {
                        this.labelError.Text = "Problem sending email<br>"+email.error;
                    }


                } else
                    this.labelError.Text = "Please provide a valid email address";
            } catch (Exception ex) {
                this.labelMain.Text = "General Exception:" + ex.Message;
            }
        }
    }
}
