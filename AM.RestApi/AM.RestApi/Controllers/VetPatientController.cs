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
    [RoutePrefix("api/v1/VetPatient")]
    public class VetPatientController : ApiController
    {
        VetDataContext context = new VetDataContext();

        [Route("{clientID}")]
        public object Get(int clientID)
        {
            int clinicID = (User as ClinicPrincipal).ClinicID;
            var patients = context.Patients.Where(x => x.ClientID == clientID).Select(x => new PatientViewModel
            {
                Name = x.Name,
                ID = x.ID,
                ClientID = x.ClientID,
                SpeciesID = x.SpeciesID,
                Breed = x.Breed,
                Birthdate = x.Birthdate,
                Gender = x.Gender,
                Logo = context.ClinicSpeciess.Where(m => m.SpeciesID == x.SpeciesID && m.Clinic.ID == clinicID).Select(m => m.Logo).FirstOrDefault(),
            });
            return patients;
        }
    }
}