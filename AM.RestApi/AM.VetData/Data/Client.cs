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
    
    public partial class Client
    {
        public Client()
        {
            this.Appointments = new HashSet<Appointment>();
            this.Patients = new HashSet<Patient>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DefaultCultureCode { get; set; }
    
        public virtual Culture Culture { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
