using AM.VetData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AM.RestApi.Model
{
    public class AppointmentModel
    {
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public int ClientID { get; set; }
        public DateTime Time { get; set; }
        public bool CheckedIn { get; set; }
        public ClientModel Client { get; set; }
        public List<PatientModel> Patients { get; set; }
    }
}