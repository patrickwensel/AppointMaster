﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VetDataContext : DbContext
    {
        public VetDataContext()
            : base("name=VetDataContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentPatient> AppointmentPatients { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicSpecies> ClinicSpeciess { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Species> Speciess { get; set; }
        public DbSet<APIUser> APIUsers { get; set; }
    }
}
