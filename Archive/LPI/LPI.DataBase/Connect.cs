using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    public class Connect {

        public static string connectString() {
            string connectionString = DataBase.Util.GetConfigValue("connnectionstring");
            if (connectionString == "") {
                string SQLserver = DataBase.Util.GetConfigValue("server");
                string DB = "LPI";
                connectionString = @"Data Source=" + SQLserver + ";User ID=sa;Password=sa;" + "Initial Catalog=" + DB;
            }
            return connectionString;
        }

        public static SqlConnection getDefaultConnection() {
            return getConnection(DataBase.Util.GetConfigValue("server"));
        }

        public static SqlConnection getConnection(string serverName) {
            try {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = connectString(); 
                cn.Open();
                return cn;
            } catch (Exception ex) {
                throw new Exception("getConenction excep:" + ex.Message);
            }
        }
    }
}
