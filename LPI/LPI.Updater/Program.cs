using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Threading.Tasks;
using LPI.DataBase.Data;
using NLog;

namespace LPI.Updater
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                UpdateReferrals();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        public static void UpdateReferrals()
        {
            LPIContext context = new LPIContext();

            List<Account> accounts = context.Accounts.ToList();

            RunAsync(accounts).Wait();
        }

        static async Task RunAsync(List<Account> accounts)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DentalAPIBaseAddress"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("apikey", ConfigurationManager.AppSettings["DentalAPIKey"]);

                foreach (Account account in accounts)
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/referrals/GetReferralsByAccountID", account.ID.ToString());

                    if (response.IsSuccessStatusCode)
                    {

                    }
                }
            }
        }
    }
}
