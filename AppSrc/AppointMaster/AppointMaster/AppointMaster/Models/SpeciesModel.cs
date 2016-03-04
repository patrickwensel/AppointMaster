using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Models
{
    public class SpeciesModel
    {
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public int SpeciesID { get; set; }
        public bool PrimaryDisplay { get; set; }
        public byte[] Logo { get; set; }
    }
}
