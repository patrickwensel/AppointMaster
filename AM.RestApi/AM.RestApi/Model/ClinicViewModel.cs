using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace AM.RestApi.Model
{
    public class ClinicViewModel
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
        //public Image Logo { get; set; }
        public byte[] Logo { get; set; }
        public string DefaultCulture { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public List<SpeciesViewModel> SpeciesSupported { get; set; } 
    }
}