using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AM.RestApi.Controllers
{
	[RoutePrefix("api/v1/VetAppointment")]
	public class VetAppointmentController : ApiController
    {
        // GET: api/VetAppointment
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/VetAppointment/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/VetAppointment
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/VetAppointment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/VetAppointment/5
        public void Delete(int id)
        {
        }
    }
}
