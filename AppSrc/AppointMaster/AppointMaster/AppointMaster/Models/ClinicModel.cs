using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Models
{
    public class ClinicModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string WebAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public byte[] Logo { get; set; }
        public string DefaultCulture { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        //public List<int> SpeciesSupported { get; set; }
    }
}
