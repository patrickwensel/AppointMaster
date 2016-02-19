using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace AM.RestApi.Identities
{
    public class ClinicPrincipal : IPrincipal
    {
        public ClinicPrincipal(IIdentity identity)
        {
            this.Identity = identity;
        }

        public IIdentity Identity { get; }
        public int ClinicID { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }

        
    }
}