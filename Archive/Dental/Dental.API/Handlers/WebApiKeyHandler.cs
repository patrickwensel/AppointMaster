using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Dental.API.Handlers
{
    public class WebApiKeyHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            IEnumerable<string> headerValues;
            var apikey = string.Empty;
            var keyFound = request.Headers.TryGetValues("apikey", out headerValues);
            if (keyFound)
            {
                apikey = headerValues.FirstOrDefault();
            }
            else
            {
                return SendError("You can't use the API without the key.", HttpStatusCode.Forbidden);
            }
            if (string.IsNullOrWhiteSpace(apikey))
            {
                return SendError("You can't use the API without the key.", HttpStatusCode.Forbidden);
            }
            char[] delimiterChars = {'|'};
            string[] keyValueFromAPICall = apikey.Split(delimiterChars);

            if (keyValueFromAPICall.Count() != 2)
            {
                return SendError("The API Key is not in the correct format. ", HttpStatusCode.Forbidden);
            }
            string keyFromAPICall = keyValueFromAPICall[0];
            string valueFromAPICall = keyValueFromAPICall[1];

            string valueFromConfig = ConfigurationManager.AppSettings[keyFromAPICall];
            if (!string.IsNullOrEmpty(valueFromConfig))
            {
                if (valueFromConfig == valueFromAPICall)
                {
                    return base.SendAsync(request, cancellationToken);
                }
                return SendError("The API Key is invalid. ", HttpStatusCode.Forbidden);
            }
            return SendError("The API Key is invalid. ", HttpStatusCode.Forbidden);
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