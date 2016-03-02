using AM.VetData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AM.RestApi.Model
{
    public class AppointmentViewModel
    {
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public int ClientID { get; set; }
        public DateTime Time { get; set; }
        public bool CheckedIn { get; set; }

        public ClientViewModel Client { get; set; }
        public ClinicViewModel Clinic { get; set; }
        public List<AppointmentPatient> AppointmentPatients { get; set; }

        public List<Patient> Patients { get; set; }
    }
}