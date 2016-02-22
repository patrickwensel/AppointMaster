using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AM.RestApi.Model;

namespace AM.RestApi.Controllers.Version1
{
    public class ClinicPropertiesController : ApiController
    {
        // GET: api/ClinicProperties
        public object Get()
        {
            Clinic clinic = new Clinic
            {
                Name = "Some Veterinary Clinic",
                Address1 = "123 Some Road",
                City = "Somewhereville",
                StateProvince = "US",
                PostalCode = "12345"
            };

            return clinic;
        }

        //// GET: api/ClinicProperties/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/ClinicProperties
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/ClinicProperties/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ClinicProperties/5
        //public void Delete(int id)
        //{
        //}
    }
}
