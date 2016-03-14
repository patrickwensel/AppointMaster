using AM.RestApi.Identities;
using AM.RestApi.Model;
using AM.VetData.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AM.RestApi.Controllers
{
    [RoutePrefix("api/v1/VetAppointment")]
    public class VetAppointmentController : ApiController
    {
        VetDataContext context = new VetDataContext();
        //// GET: api/VetAppointment
        //[Route("")]
        //public IEnumerable<string> Get()
        //{
        //          return new string[] { "value1", "value2" };
        //}

        [Route("")]
        public object Get()
        {
            int clinicID = (User as ClinicPrincipal).ClinicID;
            string date = DateTime.UtcNow.ToShortDateString();
            DateTime start = Convert.ToDateTime(string.Format("{0} 0:00:00", date));
            DateTime end = Convert.ToDateTime(string.Format("{0} 23:59:59", date));
            var appointmentIDs = context.Appointments.Where(x => x.ClinicID == clinicID && x.Time >= start && x.Time <= end && x.CheckedIn == false).Select(x => x.ID);
            return appointmentIDs;
        }

        //// GET: api/VetAppointment/5
        [Route("{appointmentID}")]
        public object Get(int appointmentID)
        {
            var appointments = context.Appointments.Where(x => x.ID == appointmentID).Select(x =>
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

            return appointments;
        }

        // POST: api/VetAppointment
        [Route("")]
        public void Post([FromBody]PostModel value)
        {
            Client client = new Client
            {
                ID = value.ClientModel.ID,
                Title = value.ClientModel.Title,
                FirstName = value.ClientModel.FirstName,
                LastName = value.ClientModel.LastName,
                Address = value.ClientModel.Address,
                City = value.ClientModel.City,
                StateProvince = value.ClientModel.StateProvince,
                PostalCode = value.ClientModel.PostalCode,
                Phone = value.ClientModel.Phone,
                Email = value.ClientModel.Email,
                DefaultCultureCode = value.ClientModel.DefaultCultureCode
            };

            Appointment appointment = new Appointment
            {
                ID = value.AppointmentModel.ID,
                ClientID = client.ID,
                ClinicID = value.AppointmentModel.ClinicID,
                Time = value.AppointmentModel.Time,
                CheckedIn = value.AppointmentModel.CheckedIn
            };
            List<Patient> lstPatient = new List<Patient>();
            lstPatient = value.PatientList.Select(x => new Patient
            {
                ID = x.ID,
                ClientID = client.ID,
                SpeciesID = x.SpeciesID,
                Name = x.Name,
                Breed = x.Breed,
                Gender = x.Gender,
                Birthdate = x.Birthdate

            }).ToList();

            if (value.ClientModel.ID > 0)
            {
                appointment.CheckedIn = true;
                context.Entry(client).State = EntityState.Modified;
                context.Entry(appointment).State = EntityState.Modified;
                foreach (var item in lstPatient)
                {
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            else
            {
                context.Clients.Add(client);
                context.SaveChanges();

                context.Appointments.Add(appointment);
                context.SaveChanges();

                foreach (var item in lstPatient)
                {
                    context.Patients.Add(item);
                    context.SaveChanges();

                    context.AppointmentPatients.Add(new AppointmentPatient
                    {
                        AppointmentID = appointment.ID,
                        PatientID = item.ID
                    });
                    context.SaveChanges();
                }
            }
        }

        //// PUT: api/VetAppointment/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/VetAppointment/5
        //public void Delete(int id)
        //{
        //}
    }

    public class PostModel
    {
        public ClientModel ClientModel { get; set; }

        public AppointmentModel AppointmentModel { get; set; }

        public List<PatientModel> PatientList { get; set; }
    }
}
