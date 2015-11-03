using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServiceConsoleCall
{
    public class LPCDR
    {
          public int CallID { get; set; }
          public string CallingNumber { get; set; }
          public string CalledNumber { get; set; }
          public string CampaignNumber { get; set; }
          public string TimestampOfCall { get; set; }
          public int Duration { get; set; }
          public float Charge { get; set; }
          public string IVRFile { get; set; }
          public string DOB { get; set; }
          public string PrimaryPhone { get; set; }
          public int AccountId {get;              set;          }
          public int TransactionId {              get;              set;          }
          public string Source { get; set; }
          public string Keyword { get; set; }
          public bool   IsSourceCampaign { get; set; }
    }

    public class AccountCampaign
    {
          public int AccountId { get; set; }
          public string CampainName { get; set; }
          public string CampaingnCallNumber { get; set; }
          public string ForwardedNumber { get; set; }
          public string CampaignProvider { get; set; }
          public int CampaignStatus { get; set; }
          public string Type { get; set; }
          public int TransactionId {get;              set;          }
    }

    public class LPAccount
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }
        public string Type { get; set; }
        public int TransactionId {get;set;}

    }
}
