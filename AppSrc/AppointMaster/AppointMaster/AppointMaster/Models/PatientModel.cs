using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Models
{
    public class PatientModel
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int? SpeciesID { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
