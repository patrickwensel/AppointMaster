using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    public class AppliUser {
        
        public enum Profile {
            LpiAdmin,        //0
            PartnerAdmin,    //1
            LpiUser,         //2
            PartnerUser,
            User
        }


        public int      DB;         //0 if PartnerId<>0
        public int      Id;
        public int      PartnerId;  //0 if DB<>0
        public string   Name;
        public string   UserName;
        public string   Password;
        public Profile  UserProfile;
        public string   Email;

        private SqlConnection cn = null;

        private string getAttributes() {
            return "DB,Id,PartnerId,name,username,password,Profile,email";
        }

        private void GetDataFromRead(SafeDbReader DbReader) {
            try {
                DB = DbReader.GetInt32(0);
                Id = DbReader.GetInt32(1);
                PartnerId = DbReader.GetInt32(2);
                Name = DbReader.GetString(3);
                UserName = DbReader.GetString(4);
                Password = DbReader.GetString(5);
                UserProfile = (Profile)DbReader.GetInt16(6);
                Email = DbReader.GetString(7);
            } catch (Exception ex) {
                throw new Exception("GetDataFromRead exception:" + ex.Message);
            }
        }//GetDataFromRead
        
        public bool DBGet(int DBnumber, int partneId, int TheId) {
            try {
                string WC = "select " + getAttributes() + " from appliuser where DB=" + DBnumber.ToString() + " and id=" + TheId.ToString()+" and partnerId="+partneId.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                if (DbReader.Read()) {
                    GetDataFromRead(DbReader);
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
                if (this.Id == 0) {
                    SqlCommand command = new SqlCommand("Select max(id) from appliuser where db=" + this.DB.ToString()+" and partnerid="+this.PartnerId.ToString(), cn);
                    SqlDataReader r = command.ExecuteReader();
                    int max = 0;
                    if (r.Read()){
                        if (!r.IsDBNull(0))
                            max=r.GetInt32(0);
                        max++;
                    }
                    r.Close();
                    r.Dispose();
                    command.Dispose();
                    command = null;
                    this.Id = max;

                    string WC = "insert into appliuser (" + getAttributes() + ") values (" + getValues() + ")";
                    command = new SqlCommand(WC, cn);
                    int nbRow = command.ExecuteNonQuery();
                    command.Dispose();
                    command = null;
                } else
                    throw new Exception("Id Not Null");
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }


        public void DBUpdate(string nativeUpdateFields) {
            try {
                string WC = "update appliuser set " + nativeUpdateFields + " where DB=" + this.DB.ToString() +" and id="+this.Id.ToString()+" and partnerId="+this.PartnerId.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }

        public void DBUpdate() {
            try {
                string s = "";
                s = "DB=" + DB.ToString() + ",";
                s += "Id=" + Id.ToString() + ",";
                s += "PartnerId=" + PartnerId.ToString()+",";
                s += "Name='" + Name + "',";
                s += "UserName='" + UserName + "',";
                s += "Password='" + Password + "',";
                s += "Profile=" + ((int)UserProfile).ToString() + ",";
                s += "Email='" + Email + "'";
                DBUpdate(s);
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }

        public void DBDelete() {
            try {
                string WC = "delete appliuser where DB=" + this.DB.ToString() + " and id=" + this.Id.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }


        public bool DBSearch(string userName) {
            try{
                return DBSearchWC("username='" + userName + "'");
            } catch (Exception ex) {
                throw new Exception("DBSearch exception:" + ex.Message);
            }
        }//DBSearch

        public bool DBSearchWC(string whereClause) {
            try {
                string WC = "select " + getAttributes() + " from appliuser where " + whereClause;
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                if (DbReader.Read()) {
                    GetDataFromRead(DbReader);
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
                throw new Exception("DBSearch exception:" + ex.Message);
            }
        }//DBSearch


        
        private string getValues() {
            try{
                string s="";
                s = DB.ToString() + ",";
                s += Id.ToString() + ",";
                s += PartnerId.ToString() + ",";
                s += "'" + Name + "',";
                s += "'" + UserName + "',";
                s += "'" + Password + "',";
                s += ((int)UserProfile).ToString() + ",";
                s += "'" + Email + "'";
                return s;
            } catch (Exception ex) {
                throw new Exception("AppliUser.getValues exception:" + ex.Message);
            }
        }

        public AppliUser(SqlConnection cn) {
            this.cn = cn;
        }

        public AppliUser(SqlConnection cn,int DB) {
            this.DB = DB;
            this.cn = cn;
        }

        public AppliUser(SqlConnection cn, int DB, int partneId, int ID)
            : this(cn, DB) {
            if (!this.DBGet(DB, partneId, ID)) {
                throw new Exception("Cannot find Appli User " + DB.ToString() + "/" + Id.ToString());
            }
        }

    }
}
