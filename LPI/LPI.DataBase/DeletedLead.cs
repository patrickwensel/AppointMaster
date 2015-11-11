using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    public class DeletedLead {
        public int DB;
        public string campaignID;
        public string PrimaryPhone;
        public DateTime timeStamp;

        private SqlConnection cn;

        private void checkNullStr(ref string s) {
            if (s == null)
                s = "";
        }
        public void checkForNullString() {
            checkNullStr(ref this.campaignID);
            checkNullStr(ref this.PrimaryPhone);
        }


        private string getAttributes() {
            //      0  1          2            3
            return "DB,campaignID,PrimaryPhone,timeStamp";
        }
        public void fillFromReader(SafeDbReader DbReader) {
            try {
                this.DB = DbReader.GetInt16(0);
                this.campaignID = DbReader.GetInt32(1).ToString();
                this.PrimaryPhone = DbReader.GetString(2);
                this.timeStamp = DbReader.GetDateTime(3);
                checkForNullString();
            } catch (Exception ex) {
            }
        }


        public bool DBGet(int DB, int campaignId, string PrimaryPhone, DateTime stamp, bool withHours) {
            try {
                PrimaryPhone = Util.phoneDigits(PrimaryPhone);
                string WC = "select " + getAttributes() + " from DeletedLead where DB=" + DB.ToString();
                WC += " and campaignId='" + campaignId.ToString() + "'";
                WC += " and PrimaryPhone='" + PrimaryPhone + "'";
                WC += " and year(timeStamp)=" + stamp.Year.ToString();
                WC += " and month(timeStamp)=" + stamp.Month.ToString();
                WC += " and day(timeStamp)=" + stamp.Day.ToString();
                if (withHours)
                    WC += " and datepart(hour,timeStamp )=" + stamp.Hour.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                if (DbReader.Read()) {
                    fillFromReader(DbReader);

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




        public void Clear() {
            this.DB = 0;
            this.campaignID = "";
            this.PrimaryPhone = "";
            this.timeStamp = DateTime.MinValue;
        }


        public DeletedLead(SqlConnection cn) {
            this.cn = cn;
        }

    }
}
