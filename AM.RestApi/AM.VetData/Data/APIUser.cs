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
    
    public partial class APIUser
    {
        public int ID { get; set; }
        public Nullable<int> ClinicID { get; set; }
        public string APIKey { get; set; }
        public string Password { get; set; }
    
        public virtual Clinic Clinic { get; set; }
    }
}
