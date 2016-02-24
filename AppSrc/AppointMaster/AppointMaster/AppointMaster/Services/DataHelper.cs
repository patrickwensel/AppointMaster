using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Services
{
    public class DataHelper
    {
        static DataHelper me;

        string authorization;

        public string GetAuthorization()
        {
            return authorization;
        }

        public void SetAuthorization(string value)
        {
            authorization = value;
        }

        public static DataHelper GetInstance()
        {
            if (me == null)
            {
                me = new DataHelper();

            }
            return me;
        }
    }
}
