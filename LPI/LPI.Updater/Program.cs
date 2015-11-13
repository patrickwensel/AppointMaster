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
using Newtonsoft.Json;

namespace LPI.Updater
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static List<string> errorIds = new List<string>();

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

            List<Patient> patients = context.Patients.ToList();

            RunAsync(patients, context).Wait();
        }

        static async Task RunAsync(List<Patient> patients, LPIContext context)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DentalAPIBaseAddress"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("apikey", ConfigurationManager.AppSettings["DentalAPIKey"]);

                Console.WriteLine("Upadate {0} Patients", patients.Count);
                int index = 0;
                foreach (Patient patient in patients)
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/referrals/GetReferredByPatientID", "3913"); //patient.ID Grish1nds TEST

                    if (response.IsSuccessStatusCode)
                    {
                        var referredById = response.RequestMessage.Content.ReadAsAsync<string>().Result;
                        patient.ReferredBy = referredById;

                        response = await client.PostAsJsonAsync("api/referrals/GetTreatmentWithStatus2ByPatientId", "3913"); //patient.ID Grish1nds TEST
                        var dentalProcedureToAdd = response.RequestMessage.Content.ReadAsAsync<List<DentalProcedure>>().Result;
                        foreach (var dentalProcedure in dentalProcedureToAdd)
                        {
                            context.AddToDentalProcedures(dentalProcedure);
                        }
                        Console.WriteLine("#{0} PatientID {1} : success", index, patient.ID);
                    }
                    else
                    {
                        errorIds.Add(patient.ID);
                        Console.WriteLine("#{0} PatientID {1} : error", index, patient.ID);
                    }

                    index++;
                }
            }
            //context.SaveChanges();
        }



        ////Old Code
        //public static void UpdateReferrals()
        //{
        //    LPIContext context = new LPIContext();

        //    List<Account> accounts = context.Accounts.ToList();

        //    RunAsync(accounts).Wait();
        //}

        //static async Task RunAsync(List<Account> accounts)
        //{

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DentalAPIBaseAddress"]);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Add("apikey", ConfigurationManager.AppSettings["DentalAPIKey"]);

        //        foreach (Account account in accounts)
        //        {
        //            HttpResponseMessage response = await client.PostAsJsonAsync("api/referrals/GetReferralsByAccountID", account.ID.ToString());

        //            if (response.IsSuccessStatusCode)
        //            {

        //            }
        //        }
        //    }
        //}
    }
}
