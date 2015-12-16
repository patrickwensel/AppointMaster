using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    public class Partner {

        public int      Id;
        public string   Name;
        public string   Address;
        public string   Address2;
        public string   City;
        public string   State;
        public string   Zip;
        public string   Phone;
        public string   Email;
        public string   MainContact;


        public SqlConnection cn = null;

        private string getAttributes() {
            return "Id,name,Address,Address2,City,State,Zip,Phone,Email,Maincontact";
        }

        private void GetDataFromRead(SafeDbReader DbReader) {
            try {
                Id = DbReader.GetInt32(0);
                Name = DbReader.GetString(1);
                Address = DbReader.GetString(2);
                Address2 = DbReader.GetString(3);
                City = DbReader.GetString(4);
                State = DbReader.GetString(5);
                Zip = DbReader.GetString(6);
                Phone = DbReader.GetString(7);
                Email = DbReader.GetString(8);
                MainContact = DbReader.GetString(9);
            } catch (Exception ex) {
                throw new Exception("GetDataFromRead exception:" + ex.Message);
            }
        }//GetDataFromRead

        public bool DBGet(int TheId) {
            try {
                string WC = "select " + getAttributes() + " from partner where id=" + TheId.ToString();
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
                    SqlCommand command = new SqlCommand("Select max(id) from partner", cn);
                    SqlDataReader r = command.ExecuteReader();
                    int max = 0;
                    if (r.Read()) {
                        if (!r.IsDBNull(0))
                            max = r.GetInt32(0);
                        max++;
                    }
                    r.Close();
                    r.Dispose();
                    command.Dispose();
                    command = null;
                    this.Id = max;

                    string WC = "insert into partner (" + getAttributes() + ") values (" + getValues() + ")";
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
                string WC = "update partner set " + nativeUpdateFields + " where id=" + this.Id.ToString();
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
                s = "Id=" + Id + ",";
                s += "Name='" + Name + "',";
                s += "Address='" + Address + "',";
                s += "Address2='" + Address2 + "',";
                s += "City='" + City + "',";
                s += "State='" + State + "',";
                s += "Zip='" + Zip + "',";
                s += "Phone='" + Phone + "',";
                s += "Email='" + Email + "',";
                s += "MainContact='" + MainContact + "'";
                DBUpdate(s);
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }

        public void DBDelete() {
            try {
                string WC = "delete partner where id=" + this.Id.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("DBcreate exception:" + ex.Message);
            }
        }


        public bool DBSearch(string userName) {
            try {
                string WC = "select " + getAttributes() + " from partner where username='" + userName + "'";
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
            try {
                string s = "";
                s = Id.ToString() + ",";
                s += "'" + Name + "',";
                s += "'" + Address + "',";
                s += "'" + Address2 + "',";
                s += "'" + City + "',";
                s += "'" + State + "',";
                s += "'" + Zip + "',";
                s += "'" + Phone + "',";
                s += "'" + Email + "',";
                s += "'" + MainContact + "'";
                return s;
            } catch (Exception ex) {
                throw new Exception("AppliUser.getValues exception:" + ex.Message);
            }
        }

        public Partner[] GetAllPartnersIdAndName() {
            try {
                ArrayList list = new ArrayList();
                string WC = "select ID,Name from partner order by Name";
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                while (DbReader.Read()) {
                    Partner p = new Partner(cn);
                    p.Id = DbReader.GetInt32(0);
                    p.Name = DbReader.GetString(1);
                    list.Add(p);
                }
                DbReader.Close();
                DbReader.Dispose();
                command.Cancel();
                command.Dispose();
                return (Partner[])list.ToArray(typeof(Partner));
            } catch (Exception ex) {
                throw new Exception("GetAllPartners ex:" + ex.Message);
            }
        }

        public string MasterKey {
            get {
                return SysOption.GetValue(cn, 0, "MASTER_KEY_" + this.Id.ToString(),"");
            }
            set {
                SysOption.SetValue(cn,0,"MASTER_KEY_" + this.Id.ToString(),(string)value);
            }
        }


        public Partner(SqlConnection cn) {
            this.cn = cn;
        }

        public Partner(SqlConnection cn, int ID)
            : this(cn) {
            if (!this.DBGet(ID)){
                throw new Exception("Cannot find Partner " + Id.ToString());
            }
        }

    }
}
