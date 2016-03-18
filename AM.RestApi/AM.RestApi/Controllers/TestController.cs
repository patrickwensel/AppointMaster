using AM.RestApi.Model;
using AM.VetData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AM.RestApi.Controllers
{
    [RoutePrefix("api/v1/VetTestAppointment")]
    public class TestController : ApiController
    {
        VetDataContext context = new VetDataContext();

        // GET: api/Test
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Test/5

        [Route("{appointmentCode}")]
        public object Get(string appointmentCode)
        {
            var appointment = context.Appointments.Where(x => x.Code.ToLower() == appointmentCode.ToLower() && x.CheckedIn == false).Select(x =>
                  new AppointmentModel
                  {
                      ID = x.ID,
                      ClinicID = x.ClinicID,
                      Time = x.Time,
                      ClientID = x.ClientID,
                      CheckedIn = x.CheckedIn.Value,
                      Client = new ClientModel
                      {
                          Title = x.Client.Title,
                          FirstName = x.Client.FirstName,
                          LastName = x.Client.LastName,
                          Address = x.Client.Address,
                          City = x.Client.City,
                          StateProvince = x.Client.StateProvince,
                          PostalCode = x.Client.PostalCode,
                          Phone = x.Client.Phone,
                          Email = x.Client.Email,
                          DefaultCultureCode = x.Client.DefaultCultureCode
                      },
                      Patients = x.AppointmentPatients.Select(m => new PatientModel
                      {
                          ID = m.ID,
                          ClientID = m.Patient.ID,
                          SpeciesID = m.Patient.SpeciesID,
                          Name = m.Patient.Name,
                          Breed = m.Patient.Breed,
                          Birthdate = m.Patient.Birthdate,
                          Gender = m.Patient.Gender

                      }).ToList()

                  }).FirstOrDefault();

            return appointment;
        }

        // POST: api/Test
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Test/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Test/5
        public void Delete(int id)
        {
        }
    }
}
