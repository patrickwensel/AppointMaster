using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Services
{
    public static class DataProvider
    {
        public static IDataProvider GetDataProvider()
        {
            if (DataHelper.GetInstance().IsDemoMode)
            {
                return new LoadDataProvider();
            }
            else
            {
                return new ServerDataProvider();
            }
        }
    }
}
