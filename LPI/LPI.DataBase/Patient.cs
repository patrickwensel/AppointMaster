using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataBase {
    public class Patient {
        public int DB;
        public string ID;
        public DateTime created;
        public string name;
        public string firstName;
        public string SSN;
        public bool male;
        public DateTime birthDate;
        public string address;
        public string addressLine2;
        public string city;
        public string state;
        public string zipCode;
        public string homePhone;
        public string cellPhone;
        public string workPhone;
        public string otherPhone;
        public string email;
        
        private SqlConnection cn;

        private string getValues() {
            if (name == null) name = "";
            if (firstName == null) firstName = "";
            if (SSN == null) SSN = "";
            if (address == null) address = "";
            if (addressLine2 == null) addressLine2 = "";
            if (city == null) city = "";
            if (state == null) state = "";
            if (zipCode == null) zipCode = "";
            if (homePhone == null) homePhone = "";
            if (cellPhone == null) cellPhone = "";
            if (workPhone == null) workPhone = "";
            if (email == null) email = "";
            if (birthDate.CompareTo(new DateTime(1900, 1, 1)) < 0)
                birthDate = new DateTime(1900, 1, 1);

            
            
            
            
            
            
            string s = DB.ToString() + ",";
            s += "'" + ID.ToString() + "',";
            s += "'" + name.Replace("'", " ") + "',";
            s += "'" + firstName.Replace("'", " ") + "',";
            s += "'" + birthDate.ToShortDateString() + "',";
            s += "'" + created.ToShortDateString() + "',";

            s += "'" + SSN.Replace("'", " ") + "',";
            if (male)
                s += "1,";
            else
                s += "0,";

            s += "'" + address.Replace("'", " ") + "',";
            s += "'" + addressLine2.Replace("'", " ") + "',";
            s += "'" + city.Replace("'", " ") + "',";
            s += "'" + state.Replace("'", " ") + "',";
            s += "'" + zipCode.Replace("'", " ") + "',";
            s += "'" + homePhone.Replace("'", " ") + "',";
            s += "'" + cellPhone.Replace("'", " ") + "',";
            s += "'" + workPhone.Replace("'", " ") + "',";
            s += "'" + email.Replace("'", " ") + "'";


            return s;
        }

        
        private string getAttributes() {
            //      0  1    2    3         4         5
            return "DB,AMID,name,firstName,birthDate,created,ssn,male,address,addressline2,city,state,zipcode,homephone,cellphone,workphone,email";
        }

        public bool DBGet(int DB, string ID) {
            try {
                string WC = "select " + getAttributes() + " from Patient where DB=" + DB.ToString() + " and AMID='" + ID.ToString()+"'";
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                if (DbReader.Read()) {
                    this.name = DbReader.GetString(2);
                    this.firstName = DbReader.GetString(3);
                    this.birthDate = DbReader.GetDateTime(4);
                    this.created = DbReader.GetDateTime(5);
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
                throw new Exception("Patient .DBGETexception:" + ex.Message);
            }
        }

        public void DBCreate() {
            try {
                string WC = "insert into patient (" + getAttributes() + ") values (" + getValues() + ")";
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("Patient .DBcreate exception:" + ex.Message);
            }
        }


        public Patient(SqlConnection cn) {
            this.cn = cn;
        }


    }
}
