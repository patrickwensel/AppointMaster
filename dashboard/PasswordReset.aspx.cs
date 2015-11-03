using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;

namespace dashboard {
    public partial class PasswordReset : SessionPage {

        private string GetRecoveryEmailContent() {
            return "Dear Dashboard user,<p>you have requested a password for you access to Practice ROI - Dashboard.<p>Please <a href=\"@\">click here</a> to rest your password.<p>Thank you<p>PracticeRoi Support Team";
        }

        protected void pbSend_Click(object sender, EventArgs e) {
            try {
                if (Util.isValidEmailAddress(this.Email.Text)) {
                    using (cn) {
                        AppliUser u = new AppliUser(cn);
                        if (u.DBSearchWC("email='" + this.Email.Text + "'")) {
                            string UID = u.DB.ToString() + "." + u.Id.ToString();
                            UID = Util.EncodeBase64String(UID);
                            UID = Util.DecodeBase64String(UID);
                            string URL = this.Request.Url.ToString() + "?reset=" + Util.EncodeBase64String(UID);
                            Email em = new Email(0);
                            em.content = GetRecoveryEmailContent().Replace("@", URL);
                            if (em.sendTo(Util.GetConfigValue("FROM_NOTIFICATION"), "PracticeRoi Administrator", u.Email, u.Name, "Your Recent Request", true, null)) {
                                this.ErrorSend.Text = "Please check your mail box. A recovery email has just been sent to " + u.Email;
                                this.Email.Text = "";
                            } else {
                                this.ErrorSend.Text = "Although this email has been recognized, we have experienced difficulties sending a recovery email. Please try again later.<p>We apologize for this.";
                            }
                        } else {
                            this.ErrorSend.Text = "If this email is recognized as a legitimate user email, a recovery email would have been sent to its recipient. Please check your mail box.";
                        }
                        cn.Close();
                    }
                } else
                    this.ErrorSend.Text = "Invalid email";
            } catch (Exception ex) {
                this.mainMessage.Text += "<br>Error:" + ex.Message;
            }
        }

        protected void pbRest_Click(object sender, EventArgs e) {
            try {
                if (this.password1.Text == this.password2.Text) {
                    if (this.password1.Text != "") {
                        using (cn) {
                            AppliUser u = new AppliUser(cn, Util.safeIntParse(this.DB.Text), 0, Util.safeIntParse(this.ID.Text));
                            u.DBUpdate("Password='"+this.password1.Text+"'");
                            this.errorReset.Text = "<p>Your password has been reset.";
                            this.mainMessage.Text = "Password Has Been Reset";
                            this.password1.Enabled = false;
                            this.password2.Enabled = false;
                            this.pbRest.Enabled = false;
                            cn.Close();
                        }
                    }else
                        this.errorReset.Text = "Password cannot be empty";
                } else
                    this.errorReset.Text = "Password do not match";
            } catch (Exception ex) {
                this.mainMessage.Text += "<br>Error:" + ex.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (!this.IsPostBack) {
                    this.PanelReset.Visible = false;
                    this.PanelForgot.Visible = false;

                    if (Request["reset"] != null) {
                        this.PanelReset.Visible = true;
                        this.mainMessage.Text = "Reset Password";
                        string UID = Util.DecodeBase64String(Request["reset"]);
                        string[] elts = UID.Split('.');
                        if (elts.Length == 2) {
                            int DB = Util.safeIntParse(elts[0]);
                            int id = Util.safeIntParse(elts[1]);
                            using (cn) {
                                AppliUser u = new AppliUser(cn, DB, 0, id);
                                this.DB.Text = DB.ToString();
                                this.ID.Text = id.ToString();
                                cn.Close();
                            }
                        } else
                            throw new Exception("Sorry Invalid Entry Parameters<br>Please contact you PRoi Administrator or contact");
                    } else {
                        this.PanelForgot.Visible = true;
                        this.mainMessage.Text = "Sending Email Recovery";
                    }
                }
            } catch (Exception ex) {
                this.mainMessage.Text += "<br>Error:" + ex.Message;
            }
        }
    }
}
