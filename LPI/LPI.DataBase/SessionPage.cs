using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace DataBase {
    public abstract class SessionPage : System.Web.UI.Page {
        protected AppliUser _currentUser = null;
        protected Partner _currentPartner = null;

        private SqlConnection _cn = null;

        protected SqlConnection cn {
            get {
                try {
                    if (_cn == null) {
                        _cn = Connect.getDefaultConnection();
                    }
                    return _cn;
                } catch (Exception ex) {
                    throw new Exception("SessinPage.cn ex:"+ex.Message);
                }
            }
        }

        protected AppliUser CurrentUser {
            get {
                if (_currentUser == null) {
                    if (Session["currentUser"] != null)
                        _currentUser = (AppliUser)Session["currentUser"];
                    else
                        return null;
                }
                return _currentUser;
            }
            set {
                _currentUser = (AppliUser)value;
                Session["currentUser"] = _currentUser;
            }
        }//CurrentUser

        protected Partner CurrentPartner {
            get {
                if (_currentPartner == null) {
                    if (Session["currentPartner"] != null)
                        _currentPartner = (Partner)Session["currentPartner"];
                    else
                        return null;
                }
                return _currentPartner;
            }
            set {
                _currentPartner = (Partner)value;
                Session["currentPartner"] = _currentPartner;
            }
        }


        protected bool Logged() {
            try {
                return (CurrentUser != null);
            } catch (Exception ex) {
                throw new Exception("Logged ex:"+ex.Message);
            }
        }//Logged
    }
}
