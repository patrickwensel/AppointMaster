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
    
    public partial class Patient
    {
        public Patient()
        {
            this.AppointmentPatients = new HashSet<AppointmentPatient>();
        }
    
        public int ID { get; set; }
        public int ClientID { get; set; }
        public Nullable<int> SpeciesID { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
    
        public virtual ICollection<AppointmentPatient> AppointmentPatients { get; set; }
        public virtual Species Species { get; set; }
        public virtual Client Client { get; set; }
    }
}
