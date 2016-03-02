using AM.RestApi.Identities;
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
    [RoutePrefix("api/v1/VetAppointment")]
    public class VetAppointmentController : ApiController
    {
        //// GET: api/VetAppointment
        //[Route("")]
        //public IEnumerable<string> Get()
        //{
        //          return new string[] { "value1", "value2" };
        //}

        [Route("")]
        public object Get()
        {
            VetDataContext context = new VetDataContext();

            int clinicID = (User as ClinicPrincipal).ClinicID;
            string date = DateTime.UtcNow.ToShortDateString();
            DateTime start = Convert.ToDateTime(string.Format("{0} 0:00:00", date));
            DateTime end = Convert.ToDateTime(string.Format("{0} 23:59:59", date));
            var appointments = context.Appointments.Where(x => x.ID == clinicID && x.Time.Value >= start && x.Time.Value <= end).Select(x =>
                 new AppointmentViewModel
                 {
                     ID = x.ID,
                     ClinicID = x.ClinicID.HasValue ? x.ClinicID.Value : 0,
                     Time = x.Time.Value,
                     ClientID = x.ClientID.Value,
                     CheckedIn = x.CheckedIn.Value,
                     Clinic = new ClinicViewModel
                     {
                         Name = x.Clinic.Name
                     },
                     Client = new ClientViewModel
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
                         DefaultCultureCode = x.Client.DefaultCultureCode,
                         Culture = new CultureViewModel
                         {
                             CountryCode = x.Client.Culture.CountryCode,
                             CultureCode = x.Client.Culture.CultureCode,
                             LanguageCode = x.Client.Culture.LanguageCode
                         }
                     },
                     Patients = x.AppointmentPatients.Select(m => new PatientViewModel
                     {
                         ID = m.ID,
                         ClientID = m.Patient.ID,
                         SpeciesID = m.Patient.SpeciesID,
                         Name = m.Patient.Name,
                         Breed = m.Patient.Breed,
                         Birthdate = m.Patient.Birthdate,
                         Gender = m.Patient.Gender

                     }).ToList()

                 });

            return appointments;
        }

        //// GET: api/VetAppointment/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/VetAppointment
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/VetAppointment/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/VetAppointment/5
        //public void Delete(int id)
        //{
        //}
    }
}
