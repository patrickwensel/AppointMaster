using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataBase {
    
    public enum DisplayMode {
        Production,
        TreatmentPlan,
        Combine
    }
        
    public class DentalProcedure {
        public int DB;
        public string ID;
        public string patientId;
        public string apptId;
        public string code;
        public bool   TreatmentPlan;
        public DateTime dateTime;
        public double amount;

        private SqlConnection cn;

        private string getAttributes() {
            //      0  1  2         3      4    5        6      7 
            return "DB,ID,patientId,apptId,code,dateTime,amount,TreatmentPlan";
        }

        private string getValues() {
            string sTreatmentPlan = "0";
            if (this.TreatmentPlan)
                sTreatmentPlan = "1";
            string s = DB.ToString() + ",";
            s += "'" + ID.ToString() + "',";
            s += "'" + patientId +"',";
            s += "'" + apptId +"',";
            s += "'" + code +"',";
            s += "'" + dateTime.ToString() +"',";
            s += amount.ToString() + ",";
            s += sTreatmentPlan;
            return s;
        }



        public bool DBGet(int DB, string ID) {
            try {
                string WC = "select " + getAttributes() + " from DentalProcedure where DB=" + DB.ToString() + " and ID='" + ID.ToString()+"'";
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader( command.ExecuteReader());
                if (DbReader.Read()) {
                    this.patientId= DbReader.GetString(2);
                    this.apptId= DbReader.GetString(3);
                    this.code = DbReader.GetString(4);
                    this.dateTime=DbReader.GetDateTime(5);
                    this.amount = (double)DbReader.GetDecimal(6);
                    this.TreatmentPlan = (DbReader.GetInt16(7) == 1);
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
                throw new Exception("Campaign .DBget exception:" + ex.Message);
            }
        }

        public void DBCreate() {
            try {
                string WC = "insert into DentalProcedure (" + getAttributes() + ") values (" + getValues() + ")";
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("Campaign .DBcreate exception:" + ex.Message);
            }
        }


        public void DBUpdate(string field, string newValue ) {
            try {
                string WC = "update DentalProcedure set " + field + " =" + newValue + " where ID='" + this.ID + "' and DB=" + this.DB.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("Dental DBUpdate exception:" + ex.Message);
            }
        }



        public DentalProcedure(SqlConnection cn) {
            this.cn = cn;
        }

    }
}
