using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AM.RestApi.Model
{
    public class AppointmentPatientViewModel
    {
        public int ID { get; set; }
        public int? AppointmentID { get; set; }
        public int? PatientID { get; set; }

        //public Appointment Appointment { get; set; }
        //public Patient Patient { get; set; }
    }
}