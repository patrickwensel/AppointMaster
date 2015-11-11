using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    public class Appointment {
        
        public int DB;              //Account of the appointment
        public DateTime dateTime;   //DateTime
        public string patientId;

        private SqlConnection cn = null;

        public static bool exist(SqlConnection cn, int DB,DateTime date, string patientId) {
            try {
                string WC = "select DB from Appointment where DB=" + DB.ToString();
                WC += " and year(datetime)=" + date.Year.ToString();
                WC += " and month(datetime)=" + date.Month.ToString();
                WC += " and day(datetime)=" + date.Day.ToString();
                WC += " and patientId='" + patientId+"'";


                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                bool exist = DbReader.Read();
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return exist;
            } catch (Exception ex) {
                throw new Exception("Account.exist exception:" + ex.Message);
            }
        }

        public bool Exists(int DB, DateTime date, string patientId) {
            try {
                string WC = "select DB from Appointment where DB=" + DB.ToString();
                WC += " and year(datetime)=" + date.Year.ToString();
                WC += " and month(datetime)=" + date.Month.ToString();
                WC += " and day(datetime)=" + date.Day.ToString();
                WC += " and patientId='" + patientId + "'";


                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                bool exist = DbReader.Read();
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return exist;
            } catch (Exception ex) {
                throw new Exception("Account.exist exception:" + ex.Message);
            }
        }

        public void DBCreate() {
            try {
                string WC = "insert into appointment (" + getAttributes() + ") values (" + getValues() + ")";
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }

        private string getAttributes() {
            return "DB,dateTime,patientID";
        }

        private string getValues() {
            string s = DB.ToString() + ",";
            s += "'"+this.dateTime.ToShortDateString()+"',";
            s += "'"+this.patientId+"'";
            return s;
        }


        
        
        public Appointment(SqlConnection cn) {
            this.cn = cn;
        }
    
    
    }
}
