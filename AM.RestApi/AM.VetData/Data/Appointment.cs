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
    
    public partial class Appointment
    {
        public Appointment()
        {
            this.AppointmentPatients = new HashSet<AppointmentPatient>();
        }
    
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public int ClientID { get; set; }
        public System.DateTime Time { get; set; }
        public Nullable<bool> CheckedIn { get; set; }
        public string Code { get; set; }
    
        public virtual ICollection<AppointmentPatient> AppointmentPatients { get; set; }
        public virtual Client Client { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}
