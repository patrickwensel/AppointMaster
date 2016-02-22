using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AM.RestApi.Identities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AM.RestApi.Handlers
{
    public class WebApiKeyHandler : DelegatingHandler
    {
        public string System { get; set; }
        public string Key { get; set; }
        public string Pass { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            byte[] data = Convert.FromBase64String(authorization.Parameter);
            string decodedString = Encoding.UTF8.GetString(data);


            char[] delimiterChars = { '|' };
            string[] keyValueFromAPICall = decodedString.Split(delimiterChars);

            int numberOfVariablesInKey = keyValueFromAPICall.Count();

            if (numberOfVariablesInKey < 2)
            {
                return SendError("The API Key is not in the correct format. ", HttpStatusCode.Forbidden);
            }

            System = keyValueFromAPICall[0];

            Key = keyValueFromAPICall[1];

            if (numberOfVariablesInKey == 3)
            {
                Pass = keyValueFromAPICall[2];
            }

            if (ValidateCredentials())
            {
                return base.SendAsync(request, cancellationToken);
            }

            return SendError("The API Key is invalid. ", HttpStatusCode.Forbidden);
        }

        private bool ValidateCredentials()
        {
            switch (System)
            {
                case "VetMobile":
                    if (AuthorizeVetMobile())
                    {
                        break;
                    }
                    return false;
                case "LPI":
                    if (AuthorizeLPI())
                    {
                        break;
                    }
                    return false;
                default:
                    return false;
            }



            return true;
        }

        private bool AuthorizeVetMobile()
        {

            //Key
            //c0a0f76d-261c-474a-8db9-d5f815e40958
            //K!ll3r
            //q38CmuY5i6f1rd4qRtUuluQmXOU=
            //
            //VetMobile|c0a0f76d-261c-474a-8db9-d5f815e40958|q38CmuY5i6f1rd4qRtUuluQmXOU=
            //VmV0TW9iaWxlfGMwYTBmNzZkLTI2MWMtNDc0YS04ZGI5LWQ1ZjgxNWU0MDk1OHxxMzhDbXVZNWk2ZjFyZDRxUnRVdWx1UW1YT1U9

            if (Key != "c0a0f76d-261c-474a-8db9-d5f815e40958" && Pass != "q38CmuY5i6f1rd4qRtUuluQmXOU=")
                return false;

            IIdentity identity = new GenericIdentity("Some Veterinary Clinic");

            ClinicPrincipal myPrincipal = new ClinicPrincipal(identity) {ClinicID = 1234};

            SetPrincipal(myPrincipal);

            return true;
        }

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private bool AuthorizeLPI()
        {
            return true;
        }

        private Task<HttpResponseMessage> SendError(string error, HttpStatusCode code)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(error);
            response.StatusCode = code;

            return Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }

    }
}