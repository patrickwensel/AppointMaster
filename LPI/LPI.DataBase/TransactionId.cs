using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    
    
    
    public class TransactionId {

        public enum Service {
            GetLatestAccounts,
            GetAccountCampaigns,
            GetCDRs
        }

        public string ID;
        public Service service;
        public DateTime CalledOn;


        private SqlConnection cn = null;


        public void register() {
        }


        
        public TransactionId(SqlConnection cn) {
            this.cn = cn;
        }


    }
}
