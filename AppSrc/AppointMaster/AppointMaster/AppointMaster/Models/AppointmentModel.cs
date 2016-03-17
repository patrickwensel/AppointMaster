using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Models
{
    public class AppointmentModel
    {
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public int ClientID { get; set; }
        public DateTime? Time { get; set; }
        public string Code { get; set; }
        public bool CheckedIn { get; set; }
        public ClientModel Client { get; set; }
        public ClinicModel Clinic { get; set; }
        public List<PatientModel> Patients { get; set; }
    }

    public class DisplayAppointmentModel : AppointmentModel
    {
        public string PatientName { get; set; }
    }
}
