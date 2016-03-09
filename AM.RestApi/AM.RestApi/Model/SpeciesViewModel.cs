using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AM.RestApi.Model
{
	public class SpeciesViewModel
	{
		public int ID { get; set; }
        public string Name { get; set; }
        public int ClinicID { get; set; }
		public int SpeciesID { get; set; }
		public bool PrimaryDisplay { get; set; }
		public byte[] Logo { get; set; }
	}
}