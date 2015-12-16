using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataBase {
    public class Campaign {

        private SqlConnection cn = null;


        public int      DB;
        public int      ID;
        public string   name; //a unic name
        public string   phoneNumber; // a unic phone number associated with the campaign
        public double   budget;
        public bool     visible;

        public int     _nbLeads;//not stored but for args passing


        public const string FormCapture = "FormCapture";

        public int getHighestId() {
            try {
                string WC = "select max(id) from Campaign where DB=" + DB.ToString() ;
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader( command.ExecuteReader());
                int value=0;
                if (DbReader.Read()){
                    if (!DbReader.IsDBNull(0))
                        value=DbReader.GetInt32(0);
                }
                DbReader.Close();
                DbReader.Dispose();
                return value;
            } catch (Exception ex) {
                throw new Exception("Account.exist exception:" + ex.Message);
            }
        }
        
        
        public static bool exist(SqlConnection cn, int DB, string MainCampaignPhone) {
            try {
                string WC = "select DB from Campaign where DB=" + DB.ToString() + " and phoneNumber='" + MainCampaignPhone + "'";
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader( command.ExecuteReader());
                bool ActExist = DbReader.Read();
                DbReader.Close();
                DbReader.Dispose();
                return ActExist;
            } catch (Exception ex) {
                throw new Exception("Campaign.exist exception:" + ex.Message);
            }
        }

        private string getAttributes() {
            return "DB,ID,name,phoneNumber,budget,visible";
        }

        public static int BoolToInt(bool b) {
            if (b)
                return 1;
            else
                return 0;
        }

        private string getValues() {
            string s = DB.ToString() + ",";
            s += "'" + ID.ToString() + "',";
            s += "'" + name.Replace("'", " ") + "',";
            s += "'" + phoneNumber.Replace("'", " ") + "',";
            s += budget.ToString()+",";
            s += BoolToInt(visible).ToString();

            return s;
        }


        /// <summary>
        /// Search by ID
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DBGet(int DB, int ID) {
            try {
                string WC = "select "+getAttributes()+" from Campaign where DB=" + DB.ToString() + " and ID='" + ID.ToString()+"'";
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader( command.ExecuteReader());
                if (DbReader.Read()) {
                    this.name = DbReader.GetString(2);
                    this.phoneNumber = DbReader.GetString(3);
                    this.budget = (double)DbReader.GetDecimal(4);
                    if (DbReader.IsDBNull(5))
                        this.visible = true;
                    else
                        this.visible = DbReader.GetByte(5) == 1;
                    this.ID = ID;
                    this.DB = DB;
                    DbReader.Close();
                    DbReader.Dispose();
                    command.Dispose();
                    return true;
                }

                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }

        /// <summary>
        /// Search by phoneNumber
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="MainCampaignPhone"></param>
        /// <returns></returns>
        public bool DBGet(int DB, string MainCampaignPhone) {
            try {
                string WC = "select " + getAttributes() + " from Campaign where DB=" + DB.ToString() + " and phoneNumber='" + MainCampaignPhone + "'";
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader( command.ExecuteReader());
                if (DbReader.Read()) {
                    this.ID = DbReader.GetInt32(1);
                    this.name = DbReader.GetString(2);
                    this.phoneNumber = DbReader.GetString(3);
                    this.DB = DB;
                    DbReader.Close();
                    DbReader.Dispose();
                    command.Dispose();
                    return true;
                }

                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }

        public bool DBGetByName(int db, string campaignName) {
            try {
                string WC = "select " + getAttributes() + " from Campaign where DB=" + db.ToString() + " and name='" + campaignName + "'";
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader dbReader = new SafeDbReader(command.ExecuteReader());
                if (dbReader.Read()) {
                    this.ID = dbReader.GetInt32(1);
                    this.name = dbReader.GetString(2);
                    this.phoneNumber = dbReader.GetString(3);
                    this.DB = DB;
                    dbReader.Close();
                    dbReader.Dispose();
                    command.Dispose();
                    return true;
                }

                dbReader.Close();
                dbReader.Dispose();
                command.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }
        

        public void DBCreate() {
            try {
                this.ID = getHighestId();
                this.ID++;
                string WC = "insert into campaign (" + getAttributes() + ") values (" + getValues() + ")";
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("Campaign .DBcreate exception:" + ex.Message);
            }
        }


        public int leadCount(string whereClause) {
            try {
                int count = 0;
                string WC = "select count(*) from lead where DB=" + this.DB.ToString() + " and campaignID='" + this.ID + "' and original=0";
                if (whereClause!="")
                    WC+=" and (" + whereClause + ")";
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                if (DbReader.Read()) {
                    count = DbReader.GetInt32(0);
                }
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return count;
            } catch (Exception ex) {
                throw new Exception("lead.count exception:" + ex.Message);
            }
        }




        public void Clear() {
            this.ID = 0;
            this.DB = 0;
            this.name = "";
            this.phoneNumber = "";
            visible = true;
        }

        public Campaign (SqlConnection cn) {
            Clear();
            this.cn = cn;
        }
    
    
    }
}
