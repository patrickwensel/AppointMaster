using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using LPI.DataBase;

namespace dashboard {
    public class DashUtil {

        public PROI_WS.accountData getAmAccount(int DB, System.Web.SessionState.HttpSessionState Session, ref string error) {
            try {
                error = "";
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
                                    error += "Account " + DB.ToString() + " setup on AM servers but do not exist on PROI server!!!";
                                }
                            } else {
                                error += "Account " + DB.ToString() + " not setup on AM servers";
                            }
                        } else {
                            error += "Could not read Account " + DB.ToString();
                        }
                    }
                }
                return (PROI_WS.accountData)Session["AmAccount"];

            } catch (Exception ex) {
                error += "getAmAccount exception:" + ex.Message;
                return null;
            }
        }



    }
}
