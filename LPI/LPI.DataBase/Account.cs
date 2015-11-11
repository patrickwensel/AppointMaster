using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {

    public enum DentalSoftware {
        EasyDental,		//0
        Dentrix,		//1
        MediSoft,		//2
        Mogo,			//3
        AppointMaster,	//4
        TGI,			//5
        MediFlex,		//6
        Softdent,//7
        Platinum, //8
        TPS, //9
        ChiroSuite, //10
        AceDental,//11
        Orthotrac,//12
        EagleSoft,//13
        DentalVision//14
    }


    public enum Status {
        Active,//0 customer has been identified through email chanel
        Desactivated,//1
        Archived,//2
        Demo,//3
        Test,//4 
    }

    public class Account {
        public int DB;
        public int GroupId;
        public int PartnerId;
        public DateTime created;
        public string name;
        public string mainDoctorName;
        public string notificationEmail;
        public string address;
        public string addressLine2;
        public string city;
        public string state;
        public string zipCode;
        public string mainPhone;
        public string fax;
        public DentalSoftware software;
        public Status status;
        public string mainContact;
        public string webSite;
        public string userId;
        public string password;
        public string DataLinkUserId;
        public string DataLinkPassword;
        public string AMServer;
        public bool   AMSubscribeReminders;
        public bool   trueNewPatientOnly;
        public string WebServicePassword;


        private SqlConnection cn = null;

        public static bool exist(SqlConnection cn, int DB) {
            try {
                string WC = "select DB from account where DB=" + DB.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                bool ActExist = DbReader.Read();
                DbReader.Close();
                DbReader.Dispose();
                return ActExist;
            } catch (Exception ex) {
                throw new Exception("Account.exist exception:" + ex.Message);
            }
        }


        private string getAttributes() {
            return "DB,created,name,mainDoctorName,notificationEmail,address,addressLine2,city,state,zipCode,mainPhone,fax,software,status,mainContact,webSite,userId,password,DataLinkUserId,DataLinkPassword,AMServer,AMSubscribeReminders,trueNewPatientOnly,GroupId,PartnerId, WebServicePassword";
        }

        
        
        private string getValues() {
            if (name==null) name="";
            if (mainDoctorName == null) mainDoctorName = "";
            if (notificationEmail == null) notificationEmail = "";
            if (address == null) address = "";
            if (addressLine2 == null) addressLine2 = "";
            if (city == null) city = "";
            if (state == null) state = "";
            if (zipCode == null) zipCode = "";
            if (mainPhone == null) mainPhone = "";
            if (fax == null) fax = "";
            if (mainContact == null) mainContact = "";
            if (webSite == null) webSite = "";
            if (userId == null) userId = "";
            if (password == null) password = "";
            if (DataLinkUserId == null) DataLinkUserId = "";
            if (DataLinkPassword == null) DataLinkPassword = "";
            if (AMServer == null) AMServer = "";






            string s = DB.ToString() + ",";
            s += "'" + created.ToString() + "',";
            s += "'" + name.Replace("'"," ") + "',";
            s += "'" + mainDoctorName.Replace("'", " ") + "',";
            s += "'" + notificationEmail.Replace("'", " ") + "',";
            s += "'" + address.Replace("'", " ") + "',";
            s += "'" + addressLine2.Replace("'", " ") + "',";
            s += "'" + city.Replace("'", " ") + "',";
            s += "'" + state.Replace("'", " ") + "',";
            s += "'" + zipCode.Replace("'", " ") + "',";
            s += "'" + mainPhone.Replace("'", " ") + "',";
            s += "'" + fax.Replace("'", " ") + "',";
            s += ((int)software).ToString().Replace("'", " ") + ",";
            s += ((int)status).ToString().Replace("'", " ") + ",";
            s += "'" + mainContact.Replace("'", " ") + "',";
            s += "'" + webSite.Replace("'", " ") + "',";
            s += "'" + userId.Replace("'", " ") + "',";
            s += "'" + password.Replace("'", " ") + "',";
            s += "'" + DataLinkUserId.Replace("'", " ") + "',";
            s += "'" + DataLinkPassword.Replace("'", " ") + "',";
            s += "'" + AMServer + "',";
            if (AMSubscribeReminders)
                s += "1,";
            else
                s += "0,";
            if (trueNewPatientOnly)
                s += "1,";
            else
                s += "0,";
            s += GroupId.ToString() + ",";
            s += PartnerId.ToString() + ",";
            s += "'" + WebServicePassword + "'";
            

            return s;
        }
        public void RemoveDuplicateLeads() {
            try {
                using (SqlCommand command = new SqlCommand("usp_RemoveDuplicateLeadsForDb "+this.DB.ToString(), cn)) {
                    int nbRows = command.ExecuteNonQuery();
                    command.Dispose();
                }
            } catch (Exception ex) {
                throw new Exception("RemoveDuplicateLeads exception:" + ex.Message);
            }
        }


        public bool DBGet(int DBnumber) {
            try {
                string WC = "select " + getAttributes() + " from account where DB=" + DBnumber.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader( command.ExecuteReader());
                if (DbReader.Read()) {
                    DB = DbReader.GetInt32(0);
                    this.created = DbReader.GetDateTime(1);
                    name = DbReader.GetString(2);
                    mainDoctorName = DbReader.GetString(3);
                    notificationEmail = DbReader.GetString(4);
                    address = DbReader.GetString(5);
                    addressLine2 = DbReader.GetString(6);
                    city = DbReader.GetString(7);
                    state = DbReader.GetString(8);
                    zipCode = DbReader.GetString(9);
                    mainPhone = DbReader.GetString(10);
                    fax = DbReader.GetString(11);
                    software = (DentalSoftware)DbReader.GetInt16(12);
                    status = (Status)DbReader.GetInt16(13);
                    mainContact = DbReader.GetString(14);
                    webSite = DbReader.GetString(15);
                    userId = DbReader.GetString(16);
                    password = DbReader.GetString(17);
                    DataLinkUserId = DbReader.GetString(18);
                    DataLinkPassword = DbReader.GetString(19);
                    AMServer = DbReader.GetString(20);
                    AMSubscribeReminders = (DbReader.GetInt16(21) == 1);
                    trueNewPatientOnly = (DbReader.GetInt16(22) == 1);
                    GroupId = DbReader.GetInt32(23);
                    PartnerId = DbReader.GetInt32(24);
                    if (DbReader.IsDBNull(25))
                        WebServicePassword = "";
                    else
                        WebServicePassword = DbReader.GetString(25);
                    DbReader.Close();
                    DbReader.Dispose();
                    command.Cancel();
                    command.Dispose();

                    return true;
                } else {
                    command.Dispose();
                    return false;
                }
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }
        
        public void DBCreate() {
            try {
                string WC = "insert into account (" + getAttributes() + ") values ("+getValues()+")";
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow=command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:"+ex.Message); 
            }
        }


        public string ExcludedCodes {
            get {
                return SysOption.GetValue(cn, this.DB, "EXCLUDED_MATHCHING_CODES", "");
            }
            set {
                SysOption.SetValue(cn, this.DB, "EXCLUDED_MATHCHING_CODES", (string)value);
            }
        }

        
        
        public void DBUpdate(string nativeUpdateFields) {
            try {
                string WC = "update account set "+nativeUpdateFields+" where DB="+this.DB.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }

        public int DBFindDbBywebServicePassword(string password) {
            try {
                int db = -1;
                string WC = "select DB from account  where WebServicePassword='" + password + "'";
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader r = command.ExecuteReader();
                if (r.Read()) {
                    db = r.GetInt32(0);
                }
                r.Close();
                r.Dispose();
                r = null;
                command.Dispose();
                command = null;
                return db;
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }

        public void DBUpdateWebServicePassword(string password) {
            try {
                string WC = "update account set WebServicePassword='" + password + "' where DB=" + this.DB.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
                this.WebServicePassword = password;
            } catch (Exception ex) {
                throw new Exception("DBUpdateWebServicePassword exception:" + ex.Message);
            }
        }

        public void CreateWebServiceAccessPassword() {
            try {
                int nbtry = 0;
                while (true) {
                    string newPass = Util.GetRandomPassword(8);
                    if (this.DBFindDbBywebServicePassword(newPass) == -1) {
                        this.DBUpdateWebServicePassword(newPass);
                        break;
                    } else
                        nbtry++;
                    if (nbtry > 100)
                        throw new Exception("Could not find an empty password after 100 tries!!!");
                }
            } catch (Exception ex) {
                throw new Exception("CreateWebServiceAccessPassword exception:" + ex.Message);
            }
        }

        public Account(SqlConnection cn) {
            this.cn = cn;
        }

        public Account(SqlConnection cn, int DB) {
            this.cn = cn;
            if (!this.DBGet(DB)) {
                throw new Exception("Cannot find account:"+DB.ToString());
            }
        }


    }




}
