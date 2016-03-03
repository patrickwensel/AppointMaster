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
				new AppointmentViewModel
				{
					ID = x.ID,
					ClinicID = x.ClinicID,
					Time = x.Time,
					ClientID = x.ClientID,
					CheckedIn = x.CheckedIn.Value,
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
						DefaultCultureCode = x.Client.DefaultCultureCode
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

				}).FirstOrDefault();

			return appointments;
		}

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
