using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Models
{
    public class PostModel
    {
        public ClientModel ClientModel { get; set; }

        public AppointmentModel AppointmentModel { get; set; }

        public List<DisplayPatientModel> PatientList { get; set; }
    }
}
