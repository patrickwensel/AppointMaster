//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AM.VetData.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Species
    {
        public Species()
        {
            this.Patients = new HashSet<Patient>();
            this.ClinicSpecies = new HashSet<ClinicSpecies>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<ClinicSpecies> ClinicSpecies { get; set; }
    }
}
