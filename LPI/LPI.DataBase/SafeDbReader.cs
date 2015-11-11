using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.Linq;
using System.Text;

namespace DataBase {
    public class SafeDbReader{

        public bool IsDBNull(int index) {
            return reader.IsDBNull(index);
        }

        public byte GetByte(int index) {
            if (reader.IsDBNull(index))
                return 0;
            else
                return reader.GetByte(index);
        }

        
        public decimal GetDecimal(int index) {
            if (reader.IsDBNull(index))
                return 0;
            else
                return reader.GetDecimal(index);
        }

        public int GetInt32(int index) {
            if (reader.IsDBNull(index))
                return 0;
            else
                return reader.GetInt32(index);
        }

        public int GetInt16(int index) {
            if (reader.IsDBNull(index))
                return 0;
            else
                return reader.GetInt16(index);
        }
        public DateTime GetDateTime(int index) {
            if (reader.IsDBNull(index))
                return DateTime.MinValue;
            else
                return reader.GetDateTime(index);
        }
        public string GetString(int index) {
            if (reader.IsDBNull(index))
                return "";
            else
                return reader.GetString(index);
        }

        public bool Read() {
            return reader.Read();
        }

        public void Close() {
            reader.Close();
        }

        public void Dispose() {
            reader.Dispose();
        }


        public SqlDataReader reader;
        public SafeDbReader(SqlDataReader r){
            reader = r;
        }


    }
}
