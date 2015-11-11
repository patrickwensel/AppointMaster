using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    public class SysOption {

        private SqlConnection cn = null;


        public int      DB;
        public string   name; //a unic name
        public string   svalue; // a unic phone number associated with the SysOption



        private string getAttributes() {
            return "DB,name,value";
        }

        private string getValues() {
            string s = DB.ToString() + ",";
            s += "'" + name + "',";
            s += "'" + svalue + "'";

            return s;
        }

        public static string GetValue(SqlConnection cn, int DB, string key, string sdefault) {
            try {
                SysOption opt = new SysOption(cn);
                if (opt.DBGet(DB, key)) {
                    string s = opt.svalue;
                    opt = null;
                    return s;
                } else {
                    opt = null;
                    return sdefault;
                }
            } catch (Exception ex) {
                throw new Exception("Exception Option.GetValue:" + ex.Message);
            }
        }
        public static bool GetValue(SqlConnection cn, int DB, string key, bool bdefault) {
            try {
                SysOption opt = new SysOption(cn);
                if (opt.DBGet(DB, key)) {
                    bool b = bool.Parse(opt.svalue);
                    opt = null;
                    return b;
                } else {
                    opt = null;
                    return bdefault;
                }
            } catch(Exception ex) {
                throw new Exception("Exception Option.GetValue:"+ex.Message);
            }
        }
        public static int GetValue(SqlConnection cn, int DB, string key, int bdefault) {
            try {
                SysOption opt = new SysOption(cn);
                if (opt.DBGet(DB, key)) {
                    int i = int.Parse(opt.svalue);
                    opt = null;
                    return i;
                } else {
                    opt = null;
                    return bdefault;
                }
            } catch (Exception ex) {
                throw new Exception("Exception Option.GetValue:" + ex.Message);
            }
        }

        public static void SetValue(SqlConnection cn, int DB, string key, object oValue) {
            try {
                SysOption opt = new SysOption(cn);
                if (opt.DBGet(DB, key)) {
                    opt.svalue = oValue.ToString();
                    opt.DBUpdate();
                } else {
                    opt.DBCreate(DB,key,oValue.ToString());
                }
                opt = null;
            } catch (Exception ex) {
                throw new Exception("Exception Option.GetValue:" + ex.Message);
            }
        }





        public bool DBGet(int DB, string name) {
            try {
                string WC = "select " + getAttributes() + " from SysOption where DB=" + DB.ToString() + " and name='" + name+"'";
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                if (DbReader.Read()) {
                    Clear();
                    this.name = DbReader.GetString(1);
                    this.svalue= DbReader.GetString(2);
                    this.DB = DB;
                    DbReader.Close();
                    DbReader.Dispose();
                    return true;
                }

                DbReader.Close();
                DbReader.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }

        public void DBCreate(int DB,string name,string value) {
            this.DB = DB;
            this.name = name;
            this.svalue = value;
            DBCreate();
        }       

        private void DBCreate() {
            try {
                string WC = "insert into SysOption (" + getAttributes() + ") values (" + getValues() + ")";
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("SysOption .DBcreate exception:" + ex.Message);
            }
        }

        private void DBUpdate() {
            try {
                string WC = "update SysOption set name='"+this.name+"', value='"+this.svalue+"' where DB="+this.DB.ToString()+" and name='"+this.name+"'";
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("SysOption .DBcreate exception:" + ex.Message);
            }
        }



        public void Clear() {
            this.DB = 0;
            this.name = "";
            this.svalue = "";
        }

        public SysOption(SqlConnection cn) {
            this.cn = cn;
        }
    
    
    }
}
