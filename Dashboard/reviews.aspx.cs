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
    public partial class reviews : System.Web.UI.Page {


        private PROI_WS.reviewStatistic getAmReviewStat(int DB) {
            try {
                if (Session["AmReviewStat"] == null) {
                    using (SqlConnection cn = new SqlConnection()) {
                        cn.ConnectionString = Connect.connectString();
                        cn.Open();
                        Account act = new Account(cn);
                        if (act.DBGet(DB)) {
                            PROI_WS.ROIServices roiWs = new PROI_WS.ROIServices();
                            PROI_WS.reviewStatistic rs= roiWs.getreviewStatistic(act.DataLinkUserId, act.DataLinkPassword);
                            Session["AmReviewStat"] = rs;
                        } else {
                            this.labelMain.Text += "Could not read Account " + DB.ToString();
                        }
                    }
                }
                return (PROI_WS.reviewStatistic)Session["AmReviewStat"];

            } catch (Exception ex) {
                this.labelMain.Text += "getAmReviewStat exception:" + ex.Message;
                return null;
            }
        }
        

        private PROI_WS.accountData getAmAccount(int DB) {
            try {
                if (Session["AmAccount"] == null) {
                    using (SqlConnection cn = new SqlConnection()) {
                        cn.ConnectionString = Connect.connectString();
                        cn.Open();
                        Account act = new Account(cn);
                        if (act.DBGet(DB)) {
                            PROI_WS.accountDataReply ar = new PROI_WS.accountDataReply();

                            PROI_WS.ROIServices roiWs = new PROI_WS.ROIServices();
                            string ak = roiWs.getPracticeAccessKey(act.DataLinkUserId, act.DataLinkPassword);
                            if (ak.ToLower().IndexOf("found") < 0) {
                                PROI_WS.accountData actAm = roiWs.getPracticeData(ak);
                                if (act.DBGet(DB)) {
                                    Session["AmAccount"] = actAm;
                                    Session["Account"] = act;
                                } else {
                                    this.labelMain.Text += "Account " + DB.ToString() + " setup on AM servers but do not exist on PROI server!!!";
                                }
                            } else {
                                this.labelMain.Text += "Account " + DB.ToString()+" not setup on AM servers";
                            }
                        } else {
                            this.labelMain.Text += "Could not read Account "+DB.ToString();
                        }
                    }
                }
                return (PROI_WS.accountData)Session["AmAccount"];

            } catch (Exception ex) {
                this.labelMain.Text += "getAmAccount exception:" + ex.Message;
                return null;
            }
        }

        protected string getRecentSurveyList() {
            try {
                string s = "";
                if (Session["Account"] != null) {
                    Account act = (Account)Session["Account"];
                    PROI_WS.ROIServices webServices = new global::dashboard.PROI_WS.ROIServices();
                    PROI_WS.reviewPosted[] reviews = webServices.getreviewPostedPerPractice(act.DataLinkUserId, act.DataLinkPassword);
                    if (reviews.Length > 0) {
                        s += "<center><table class='surveyList' cellspacing=1 callpadding=0>";
                        s += "<tr><th width=1%>ID</th><th>Date</th><th>Patient</th><th>Rating</th><th>Review</th><th>Status</th><th>Action</th></tr>";
                        s += "";
                        for (int i = 0; i < reviews.Length; i++) {
                            s += "<tr>";

                            s += "<td align=center style='color:#AAAAAA;font-size:10px;'>" + reviews[i].surveyId+ "</td>";
                            s += "<td>" + reviews[i].posted.ToShortDateString() + "</td>";
                            
                            if (reviews[i].patientCheckedConfidential)
                                s += "<td>Anonymous</td>";
                            else
                                s += "<td>" + reviews[i].patientFirstName+" "+reviews[i].patientName+ "</td>";

                            s += "<td align=center><b> " + reviews[i].rating100.ToString() + " %</b> </td>";

                            s += "<td> " + reviews[i].review + "</td>";

                            s += "<td align=center>";
                            if (reviews[i].flagged)
                                s += "EXCLUDED";
                            s += "</td>";


                            s += "<td align=center>"; 
                            System.TimeSpan ts = DateTime.Today.Subtract(reviews[i].posted);
                            if (ts.TotalDays < 30) {
                                if (reviews[i].flagged)
                                    s += "<a href='reviews.aspx?unflag=" + reviews[i].surveyId + "&RECENT=1'>Insert back</a>";
                                else
                                    s += "<a href='reviews.aspx?flag=" + reviews[i].surveyId + "&RECENT=1'>Flag</a>";
                            }
                            s += "</td> " ;

                            s += "</tr>";
                        }
                        s += "</table></center>";
                    } else {
                        s += "No Review found";
                    }
                    return s;
                } else {
                    return "Time out";
                }
            } catch (Exception ex) {
                return "Exception:"+ex.Message;
            }
        }

        protected void setflag(string ID,bool flag) {
            try {
                if (Session["Account"] != null) {
                    Account act = (Account)Session["Account"];
                    PROI_WS.ROIServices roiWs = new PROI_WS.ROIServices();

                    string s=roiWs.setReviewFlag(act.DataLinkUserId, act.DataLinkPassword, ID, flag);
                    if (s!="")
                        this.labelMain.Text += "Flag error:" + s;
                }
            } catch (Exception ex) {
                this.labelMain.Text += "Cannot flag Exception:"+ex.Message;
            }
        }

        protected void unflag(string ID) {
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (Session["DB"] != null) {
                this.linkLead.Text = "<a href=\"dashboard.aspx?p=4\"><span>Lead Generation</span></a>";
                DashUtil util = new DashUtil();
                string error = "";
                PROI_WS.accountData amAct = util.getAmAccount((int)Session["DB"],Session,ref error);
                if (amAct != null) {
                    Account act = (Account)Session["Account"];
                    this.AccountName.Text = act.name + " <small>" + act.DB.ToString() + "</small>";
                    this.labelMain.Text += "<div style='font-family:Arial;font-size:12px;text-align:left;margin:10px;'>";

                    if (Request["flag"] != null) {
                        setflag((string)Request["flag"], true);
                    } else if (Request["unflag"] != null) {
                        setflag((string)Request["unflag"], false);
                    }






                    if (Request["WEBSITE"] != null) {
                        this.labelMain.Text += "<h2>Published Reviews On The Web</h2>";

                        this.labelMain.Text += "<a href='reviews.aspx'>< Reviews Menu</a><br><br>";
                        this.labelMain.Text += "<iframe src=\"" + amAct.reviewsUrl + "\" width=\"100%\" height=\"700\" frameborder=\"0\" scrolling=\"auto\" display=none></iframe>";
                    } else if (Request["RECENT"] != null) {
                        this.labelMain.Text += "<h2>50 Recent Reviews for " + act.name + "<br><small># <small>" + act.DB.ToString() + "</small></h2>";
                        this.labelMain.Text += "<a href='reviews.aspx'>< Reviews Menu</a><br>";
                        this.labelMain.Text += "<br>";
                        this.labelMain.Text += getRecentSurveyList();

                    } else {
                        /// Just display the menu with statistic
                        try {
                            PROI_WS.reviewStatistic rs = getAmReviewStat((int)Session["DB"]);

                            this.labelMain.Text += "<center><div style='font-family:Arial;font-size:14px;text-align:left;margin:10px;width:50%;border:solid 1px #346711;padding:20px;background-color:#f0f0f0;'>";
                            this.labelMain.Text += "<div style='font-weight:bold;width:100%;font-size:17px;text-align:center;padding:10px;background-image: url(images/line_03.jpg); background-repeat:repeat-x;;color:white'>";
                            this.labelMain.Text += "Activity Summary";
                            this.labelMain.Text += "</div>";

                            this.labelMain.Text += "<p><br><br>We sent out <b>" + rs.nbInvitationsSent.ToString() + "</b> invitations since <b>" + rs.firstInvitationSentout.ToShortDateString() + "</b>";
                            this.labelMain.Text += "<p>";


                            this.labelMain.Text += "So far, we have collected for you:";
                            this.labelMain.Text += "<ul style='line-height:20px;'>";

                            double ratio = 0;

                            this.labelMain.Text += "<li><b><big>" + amAct.nbRatings.ToString() + "</big></b> ratings";
                            if (rs.nbInvitationsSent > 0) {
                                ratio = amAct.nbRatings;
                                ratio = ratio / rs.nbInvitationsSent;
                                this.labelMain.Text += " (" + ratio.ToString("P") + ")";
                            }

                            this.labelMain.Text += "<li><b><big>" + amAct.nbReviews.ToString() + "</big></b> reviews";
                            if (rs.nbInvitationsSent > 0) {
                                ratio = amAct.nbReviews;
                                ratio = ratio / rs.nbInvitationsSent;
                                this.labelMain.Text += " (" + ratio.ToString("P") + ")";
                            }


                            if (amAct.lastReview.CompareTo(DateTime.MinValue) > 0)
                                this.labelMain.Text += "<li>Last review posted on <b>" + amAct.lastReview.ToShortDateString() + "<b>";

                            this.labelMain.Text += "</ul>";
                            this.labelMain.Text += "</div></center>";

                            this.labelMain.Text += "<br><br><a href='reviews.aspx?RECENT=1'><big>Most Recent Reviews</a></big><small> monitor and manage recently posted reviews</small><br><br>";
                            //this.labelMain.Text += "<a href='reviews.aspx?WEBSITE=1'><big>All Published Reviews On The Web</big></a><small> check your Patient Satisfaction Web Site</small><br><br>";
                        } catch (Exception ex) {
                            this.labelMain.Text += "Exception:" + ex.Message;
                        }
                    }
                    this.labelMain.Text += "</div>";


                    if (amAct.lastSync.CompareTo(new DateTime(1900, 1, 1)) > 0) {
                        this.labelMain.Text += "<br>Last Sync was:" + amAct.lastSync.ToString();
                    } else {
                        this.labelMain.Text += "<h3>This Account Has Never Synchronized<h3>";
                    }
                } else {
                    this.labelMain.Text += "<br>Account Error:" + error;
                }
            } else {
                Response.Redirect("login.aspx");
            }
        }
    }
}
