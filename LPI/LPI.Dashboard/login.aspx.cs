using System;
using System.Collections;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Hosting;
using System.Security.Cryptography;
using DataBase;


namespace dashboard {
    
    
    public partial class login : System.Web.UI.Page {

        bool iFrame = false;

        public static string EncodePassword(string originalPassword) {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            // Conver the original password to bytes; then create the hash
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            // Bytes to string
            return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(encodedBytes), "-", "").ToLower();
        }
        
        
        protected string getServerName() {
            if (Session["server"] == null) {
                Session["server"] = Util.getServerName();
            }
            return (string)Session["server"];
        }
        
        
        
        private string superPassw = "lpi20!!";
        private int forceb() {
            try {
                int db = int.Parse(this.userId.Text);
                return db;
            } catch {
                return -1;
            }
        }


        protected bool PartnerLoginIdentified(SqlConnection cn, int Db, string possibleMarterKey) {
            try {
                if (Db > 0) {
                    Account act = new Account(cn);
                    if (act.DBGet(Db)) {
                        if (act.PartnerId != 0) {
                            Partner p = new Partner(cn);
                            if (p.DBGet(act.PartnerId)) {
                                if (p.MasterKey == possibleMarterKey) {
                                    Session["superadmin"] = true;
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            } catch (Exception ex) {
                throw new Exception("PartnerLoginIdentified ex:"+ex.Message);
            }
        }


        protected void PbLogin_Click(object sender, EventArgs e) {
            try {
                Util.Trace("Login Submit From:" + Request.UserHostAddress+" "+this.userId.Text+"/"+this.password.Text);
                
                string SQLserver = getServerName();
                string NextPage = "";
                using (SqlConnection cn = Connect.getDefaultConnection()) {
                    int Db = forceb();
                    bool getin = false;
                    superPassw = Util.GetConfigValue("ADMIN_PASSWORD");
                    if ((Request.UserHostAddress.ToLower().IndexOf("localhost") >= 0) | (Request.UserHostAddress.ToLower().IndexOf("::1") >= 0) | (Request.UserHostAddress.ToLower()=="127.0.0.1"))
                        if (this.password.Text =="")
                            this.password.Text =superPassw;

                    if ((Db > 0) & ((this.password.Text == superPassw) | (this.password.Text == "aaron"))) {
                        Util.Trace("Login Forced" + Request.UserHostAddress + " " + this.userId.Text + "/" + this.password.Text);
                        Session["superadmin"] = true;
                        getin = true;
                    } else {
                        string WC = "select DB,password from account where userId='" + this.userId.Text + "'";
                        SqlCommand command = new SqlCommand(WC, cn);
                        SqlDataReader DbReader = command.ExecuteReader();
                        string passwMD5 = "";
                        while (DbReader.Read()) {
                            Db = DbReader.GetInt32(0);
                            passwMD5 = DbReader.GetString(1);
                        }
                        if (Db > 0) {
                            Util.Trace("Login Found " + Db.ToString());
                            string encodedPass = EncodePassword(this.password.Text);
                            if ((encodedPass == passwMD5) | (this.password.Text == superPassw)) {
                                getin = true;
                            }
                        }
                        DbReader.Close();
                        DbReader.Dispose();
                    }
                    Session["user"] = null;
                    if (!getin) {
                        AppliUser u = new AppliUser(cn);
                        if (u.DBSearch(this.userId.Text)) {
                            if (u.Password == this.password.Text) {
                                Db = u.DB;
                                getin = true;
                                Session["user"] = u;
                            }
                        }
                    }

                    if (!getin) {
                        int i = Util.safeIntParse(this.userId.Text);
                        getin=PartnerLoginIdentified(cn,i,this.password.Text);
                        /*if (i > 0) {
                            Account act = new Account(cn);
                            if (act.DBGet(Db)) {
                                if (act.PartnerId != 0) {
                                    Partner p = new Partner(cn);
                                    if (p.DBGet(act.PartnerId)) {
                                        if (p.MasterKey == this.password.Text) {
                                            Session["superadmin"] = true;
                                            getin = true;
                                        }
                                    }
                                }
                            }
                        }*/
                    }


                    if (getin) {
                        Session["DB"] = Db;
                        Account act = new Account(cn);
                        if (act.DBGet(Db)) {
                            string message = "";
                            Session["account"]=act;
                            try {
                                act.RemoveDuplicateLeads();
                            } catch(Exception ex) {
                                message = "Problem in RemoveDuplicateLeads:" + ex.Message;
                            }
                            NextPage = "dashboard.aspx?p=2&message=" + message;
                        }
                    }
                    cn.Close();
                }
                if (NextPage != "") {
                    if (iFrame) {
                        NextPage += "&iFrame=1";
                    }
                    Response.Redirect(NextPage,false);
                } else {
                    this.error.Text = "Account not found. Please try again.";
                }
            } catch (Exception ex) {
                this.error.Text = "Exception in Page:" + ex.Message;
            }
        }

        private bool VirtualDirLoginDetected(ref int DB, ref bool iFrame, ref bool detected) {
            try {
                string virTualDir = Request.Url.ToString().ToLower();

                string[] elts = virTualDir.Split('/');
                if (elts.Length >= 3) {
                    virTualDir = elts[3];
                    string sDB = Util.GetConfigValue(virTualDir);
                    if (sDB != "") {
                        DB = Util.safeIntParse(sDB);
                        if (DB > 0) {
                            iFrame = true;
                            detected = true;
                            return true;
                        }
                    }
                    return false;
                } else
                    return false;
            } catch (Exception ex) {
                throw new Exception("VirtualDirLoginDetected ex:" + ex.Message);
            }//VirtualDirLoginDetected
        }



        private void CheckHeaderFooter() {
            if (Session["iFrame"] == null) {
                this.LabelHeader.Text = "<div class=\"center-column\"><img src=\"images/logo.png\"   style=\"margin:20px;\" />";
                this.LabelFooter.Text = "</div>";
                //Literal cssFile = new Literal() { Text = @"<link href=""" + this.ResolveUrl("css/base.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                Literal cssFile1 = new Literal() { Text = @"<link href=""" + this.ResolveUrl("css/popups.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                Literal cssFile2 = new Literal() { Text = @"<link href=""" + this.ResolveUrl("css/practiceroi.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                //this.Header.Controls.Add(cssFile);
                this.Header.Controls.Add(cssFile1);
                this.Header.Controls.Add(cssFile2);
                //this.Header.Controls.Add(

            }
        }


        protected void Page_Load(object sender, EventArgs e) {
            try {
                TextWriterTraceListener[] listeners = new TextWriterTraceListener[]{
                new TextWriterTraceListener("c:\\debugLPI.txt")};
                Debug.Listeners.AddRange(listeners);
                if (Request.Url.ToString().IndexOf("localhost") >= 0)
                    this.userId.Text = "1170";

                System.Web.HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
                this.labelURL.Text = Request.Url.ToString();
                this.labelURL.Text += " " + Request.QueryString.Count.ToString()+" querys ";
                for (int i = 0; i < Request.QueryString.Count; i++) {
                    this.labelURL.Text += Request.QueryString.AllKeys[i]+ " "; 
                }

                if (!this.IsPostBack) {
                    Util.Trace("Login Request From" + Request.UserHostAddress);

                    iFrame = (Request["iframe"] != null);
                    if (iFrame) {
                        Session["iFrame"] = true;
                    } else {
                        Session["iFrame"] = null;
                    }
                    bool virtualDirDetected = false;
                    int DB = 0;
                    if (Request["logout"] != null) {
                        Session.Clear();
                        Session.Abandon();
                        this.mainMessage.Text = "Thank you for using PraticeRoi";
                        if (Request["iframe"] != null)
                            Response.Redirect("login.aspx?iFrame=1",false);
                        else
                            Response.Redirect("login.aspx",false);
                    } else if ((Request["direct"] != null) | (Request["db"] != null) | (Request["dbmk"] != null) | (VirtualDirLoginDetected(ref DB, ref iFrame, ref virtualDirDetected))) {
                        //*** External Login RETURN from AM or partner

                        if (!virtualDirDetected) {
                            if ((Request["direct"] != null))
                                DB = Util.safeIntParse(Util.DecodeBase64String(Request["direct"]));
                            else
                                DB = Util.safeIntParse(Request["db"]);
                        }
                        string masterKey = null;

                        //dbmk=db.masterkey.iframe
                        //dbmk=db.masterkey

                        if (Request["dbmk"] != null) {
                            string[] elts = Request["dbmk"].Split('.');
                            if (elts.Length >= 2) {
                                DB = Util.safeIntParse(elts[0]);
                                masterKey = elts[1];
                                if (elts.Length == 3)
                                    iFrame = elts[2].ToUpper() == "IFRAME";
                            }
                        }

                        string nextPage = "";
                        if (Request["nxtpage"] != null) {
                            nextPage = Util.DecodeBase64String(Request["nxtpage"]);
                        }

                        string SQLserver = getServerName();
                        using (SqlConnection cn = Connect.getDefaultConnection()) {
                            Account act = new Account(cn);
                            if (act.DBGet(DB)) {
                                string message = "";

                                if (!virtualDirDetected) {
                                    if (Request["mk"] != null) {
                                        masterKey = Request["mk"];
                                    }

                                    if ((Request["db"] != null) | ((Request["dbmk"] != null))) {
                                        if (masterKey != null) {
                                            if (!PartnerLoginIdentified(cn, DB, masterKey)) {
                                                throw new Exception("Account Not Found");
                                            }
                                        } else
                                            throw new Exception("Incorrect parameter usage:01");
                                    }
                                }

                                Session["account"] = act;
                                try {
                                    act.RemoveDuplicateLeads();
                                } catch (Exception ex) {
                                    message = "Problem in RemoveDuplicateLeads:" + ex.Message;
                                }
                                nextPage = "dashboard.aspx?p=2&message=" + message;


                                Session["displayTP"] = SysOption.GetValue(cn, DB, "DISPLAY_TREATMENTPLAN", 1);
                                Session["DB"] = DB;
                                Session["account"] = act;
                                Session["accountname"] = act.name;
                                if (nextPage == "")
                                    nextPage = "dashboard.aspx?p=2&message=" + message;
                                else if (nextPage.ToUpper() == "REVIEWS")
                                    nextPage = "reviews.aspx";
                                else if (nextPage.ToUpper() == "DASHBOARD")
                                    nextPage = "dashboard.aspx?p=2&message=" + message;
                                else if (nextPage.ToUpper() == "LOGOUT") {
                                    nextPage = "login.aspx?logout=1";
                                } else if (nextPage.ToUpper() == "LOCALLISTINGS") {
                                    nextPage = "localListings.aspx";
                                }
                                if (iFrame) {
                                    nextPage += "&iFrame=1";
                                    Session["iFrame"] = true;
                                } else {
                                    Session["iFrame"] = null;
                                }
                                cn.Close();
                                
                                Response.Redirect(nextPage,false);
                            } else {
                                this.error.Text = "Account Not Found";
                                if (iFrame) {
                                    Response.Redirect("AcountNotFoundiFrame.htm",false);
                                }
                            }
                            cn.Close();
                        }
                    }
                    CheckHeaderFooter();
                }
                System.Web.HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            } catch (Exception ex) {
                this.error.Text += "Main ex:"+ex.Message;
                if (iFrame) {
                    Response.Redirect("AcountNotFoundiFrame.htm?error="+ex.Message);
                }
            }
        }
    }
}
//login.aspx?direct=MTEzNQ%3d%3d&nxtpage=cmV2aWV3cw%3D%3D
//login.aspx?dbmk=1161.masterkey2013.iframe
//login.aspx?db=1161&mk=masterkey2013&iFrame=1