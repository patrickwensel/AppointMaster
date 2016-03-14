using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AM.RestApi.Model
{
    public class CultureModel
    {
        public string CultureCode { get; set; }
        public string CountryCode { get; set; }
        public string LanguageCode { get; set; }
    }
}