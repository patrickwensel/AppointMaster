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
    public partial class campaignNames : System.Web.UI.Page {


        private void showOption(SqlConnection cn) {
            this.cbDisplayRoi.Checked = SysOption.GetValue(cn, (int)Session["DB"], "DisplayRoi", false);
            this.cbAutoHide.Checked = SysOption.GetValue(cn, (int)Session["DB"], "AutoHide", true);
        }

        private void DisplayCampaign() {
            try {
                this.panelCampaign.Visible = true;
                using (SqlConnection cn = Connect.getDefaultConnection()) {
                    Campaign c = new Campaign(cn);
                    if (c.DBGet((int)Session["DB"], Util.safeIntParse(Request["ID"]))) {
                        this.ID.Text = Request["ID"];
                        this.Phone.Text = c.phoneNumber;
                        this.name.Text = c.name;
                        this.budget.Text = c.budget.ToString("N");
                        this.cbVisible.Checked = c.visible;
                        if (c.phoneNumber.StartsWith("FormCapture")){
                            this.Phone.Enabled=false;
                        }
                    } else
                        this.labelmain.Text = "Cannot read campaign";
                    showOption(cn);
                    cn.Close();
                }
            } catch (Exception ex) {
                this.labelmain.Text = "DisplayCampaign Exception ex:" + ex.Message;
            }
        }

        private void ListCampaigns() {
            try {
                using (SqlConnection cn = Connect.getDefaultConnection()) {
                    string WC = "Select id,name,phoneNumber,budget, visible from campaign where DB=" + ((int)Session["DB"]).ToString();
                    SqlCommand cmd = new SqlCommand(WC, cn);
                    SqlDataReader r = cmd.ExecuteReader();
                    string s = "<table class=\"listTable\"><tr><th>ID</th><th>Name</th><th>Phone</th><th>Monhtly Budget</th><th>Visible</th><th>Action</th></tr>";
                    int ID=0;
                    int visible = 0;
                    while (r.Read()) {
                        ID=r.GetInt32(0);

                        if (r.IsDBNull(4))
                            visible = 1;
                        else
                            visible = r.GetByte(4);
                        if (visible == 1)
                            s += "<tr>";
                        else
                            s += "<tr class=\"invisibleLine\"\">";
                        s += "<td>";
                        s += ID.ToString() + "</td><td>" + r.GetString(1) + "</td><td>" + r.GetString(2) + "</td>";
                        s += "<td style=\"text-align:right\">" + r.GetDecimal(3).ToString("N") + "</td>";
                        
                        s += "<td>" + visible.ToString() + "</td>";
                        s += "<td><a href='campaignNames.aspx?ID=" + ID.ToString() + "'>Edit</a></td>";
                        /*if (r.GetString(2) != "FormCapture")
                            s += "<td><a href='campaignNames.aspx?ID=" + ID.ToString() + "'>Edit</a></td>";
                        else
                            s += "<td></td>";
                         * */
                        s += "</tr>";
                    }
                    r.Close();
                    r.Dispose();
                    showOption(cn);
                    cn.Close();
                    s += "</table>";
                    this.labelmain.Text += s;
                }
            } catch (Exception ex) {
                this.labelmain.Text = "DisplayCampaign Exception ex:" + ex.Message;
            }
        }


        protected void pbsave_Click(object sender, EventArgs e) {
            try {
                double d = double.Parse(this.budget.Text);
                if (this.name.Text.Length > 0) {
                    if (this.name.Text.Length < 255) {
                        using (SqlConnection cn = Connect.getDefaultConnection()) {
                            string WC = "update campaign set name='" + this.name.Text + "',budget=" + d.ToString() + ",visible=" + Campaign.BoolToInt(cbVisible.Checked).ToString() + " where DB=" + ((int)Session["DB"]).ToString() + " and id=" + this.ID.Text;
                            SqlCommand cmd = new SqlCommand(WC, cn);
                            int i = cmd.ExecuteNonQuery();
                            if (i >= 0) {
                                cn.Close();
                                Response.Redirect("campaignNames.aspx");
                            } else
                                this.labelmain.Text = "Pb";

                            cn.Close();
                        }
                    } else
                        this.labelmain.Text = "Name is 255 cars max!!!!";
                } else
                    this.labelmain.Text = "Name cannot be empty!!!!";
            } catch (Exception ex) {
                this.labelmain.Text = "DisplayCampaign Exception ex:" + ex.Message;
            }
        }

        protected void pbCancel_Click(object sender, EventArgs e) {
            Response.Redirect("campaignNames.aspx");
        }

        protected void pbReturn_Click(object sender, EventArgs e) {
            if (Session["DB"] != null) {
                Response.Redirect("dashboard.aspx");
            } else {
                Response.Redirect("login.aspx");
            }

        }

        protected void pbSaveOptions_Click(object sender, EventArgs e) {
            try {
                using (SqlConnection cn = Connect.getDefaultConnection()) {
                    SysOption.SetValue(cn, (int)Session["DB"], "DisplayRoi", this.cbDisplayRoi.Checked);
                    SysOption.SetValue(cn, (int)Session["DB"], "AutoHide", this.cbAutoHide.Checked);
                    Response.Redirect("dashboard.aspx");
                    cn.Close();
                }
            } catch (Exception ex) {
                this.labelmain.Text = "DisplayCampaign Exception ex:" + ex.Message;
            }
            
        }
        
        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (!this.IsPostBack) {
                    if (Session["iFrame"] == null) {
                        this.LabelHeader.Text = "<div class=\"center-column\"><img src=\"images/logo.png\"   style=\"margin:20px;\" /><h2 style=\" margin:20px; float:right;color: rgb(47,87,122)\">Dashboard</h2>";
                        this.LabelFooter.Text = "</div>";
                        Literal cssFile = new Literal() { Text = @"<link href=""" + this.ResolveUrl("css/base.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                        Literal cssFile1 = new Literal() { Text = @"<link href=""" + this.ResolveUrl("css/popups.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                        Literal cssFile2 = new Literal() { Text = @"<link href=""" + this.ResolveUrl("css/practiceroi.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                        this.Header.Controls.Add(cssFile);
                        this.Header.Controls.Add(cssFile1);
                        this.Header.Controls.Add(cssFile2);
                        //this.Header.Controls.Add(
                        
                    }
                    this.panelCampaign.Visible = false;
                    this.PanelList.Visible = false;
                    if (Session["DB"] != null) {
                        bool ok = false;
                        if (Session["user"] != null) {
                            AppliUser u = (AppliUser)Session["user"];
                            if (u.UserProfile == AppliUser.Profile.PartnerUser)
                                ok = true;
                        } else
                            ok = true;
                        if ((Session["superadmin"] != null) | (ok)) {
                            if (Request["ID"] != null) {
                                DisplayCampaign();
                                this.pbReturn.Visible = false;
                            } else {
                                ListCampaigns();
                                this.PanelList.Visible = true;
                            }
                        } else
                            this.labelmain.Text = "Not authorized";
                    } else {
                        Response.Redirect("login.aspx");
                    }
                }
            } catch (Exception ex) {
                this.labelmain.Text = "Exception ex:" + ex.Message;
            }
        }

    }
}
