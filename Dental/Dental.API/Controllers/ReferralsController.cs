using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Dental.API.Controllers
{
    public class ReferralsController : ApiController
    {

        [HttpPost]
        public async Task<object> GetReferralsByAccountID()
        {
            string jsonAccountID = await Request.Content.ReadAsStringAsync();

            int accountID = Convert.ToInt32(JsonConvert.DeserializeObject<string>(jsonAccountID));

            return 1;
        }

        //public int Test()
        //{
        //    //IEnumerable<string> headerValues = request.Headers.GetValues("x");
        //    //var id = headerValues.FirstOrDefault();

        //    return 27;
        //}
    }
}

