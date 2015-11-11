using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
    public partial class localListings : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["DB"] != null) {
                //this.linkLead.Text = "<a href=\"dashboard.aspx?p=4\"><span>Lead Generation</span></a>";
                DashUtil util = new DashUtil();
                string error = "";
                PROI_WS.accountData amAct = util.getAmAccount((int)Session["DB"], Session, ref error);
                if (amAct != null) {
                    Account act = (Account)Session["Account"];
                    this.AccountName.Text = act.name + " <small>" + act.DB.ToString() + "</small>";
                    //this.labelMain.Text += "<div style='font-family:Arial;font-size:12px;text-align:left;margin:10px;'>";
                    
                    this.labelIframe.Text = "<iframe frameborder=0  src=\""+Util.getLocalListingURL()+"?accountId="+act.DB.ToString()+"#\" style=\"width:1000px;height:550px; overflow:visible; margin:0px; padding:10px; text-align:center; border-style:none; \" ></iframe>";
                }
            } else {
                Response.Redirect("login.aspx");
            }
        }
    }
}
