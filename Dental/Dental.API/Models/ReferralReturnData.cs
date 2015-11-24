using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dental.API.Models
{
    public class ReferralReturnData
    {
        public int AccountID { get; set; }
        public string PersonID { get; set; }
        public string ReferredByID { get; set; }
        public string ResponsiblePartyId { get; set; }
        public List<string> TreatmentIds { get; set; }
    }
}