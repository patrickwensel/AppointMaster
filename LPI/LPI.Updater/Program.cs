using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LPI.DataBase.Data;
using NLog;
using Newtonsoft.Json;

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
            Console.WriteLine("UpdateReferrals Start");
            int successCount = 0;
            int errorCount = 0;
            using (HttpClient client = new HttpClient())
            {
                LPIContext context = new LPIContext();
                context.CommandTimeout = 1000;
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DentalAPIBaseAddress"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("apikey", ConfigurationManager.AppSettings["DentalAPIKey"]);

                foreach (Account account in accounts)
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/referrals/GetReferralsByAccountID", account.ID.ToString());

                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> jsonReferralReturnDatas = response.Content.ReadAsStringAsync();
                        List<ReferralReturnData> referralReturnDatas = JsonConvert.DeserializeObject<List<ReferralReturnData>>(jsonReferralReturnDatas.Result);

                        foreach (ReferralReturnData item in referralReturnDatas)
                        {
                            Patient patient = context.Patients.FirstOrDefault(x => x.ID == item.PersonID && x.AccountID == item.AccountID);
                            if (patient != null)
                            {
                                Console.WriteLine("update Patient with ID=" + patient.ID);
                                patient.ReferredBy = item.ReferredByID;

                                response = await client.PostAsJsonAsync("api/referrals/GetTreatmentWithStatus2ByPatientId", item.PersonID);
                                Task<string> jsonReferralTreatmentReturnDatas = response.Content.ReadAsStringAsync();
                                List<DentalProcedure> dentalProcedureToAdd = JsonConvert.DeserializeObject<List<DentalProcedure>>(jsonReferralTreatmentReturnDatas.Result);
                                foreach (DentalProcedure dentalProcedure in dentalProcedureToAdd)
                                {
                                    var existingDentalProcedure = (from dp in context.DentalProcedures
                                        where dp.ID == dentalProcedure.ID
                                        select dp).FirstOrDefault();

                                    if (existingDentalProcedure == null)
                                    {
                                        Console.WriteLine("- add new DetalProcedure");
                                        context.AddToDentalProcedures(dentalProcedure);
                                    }
                                }
                                try
                                {
                                    context.SaveChanges();
                                    successCount++;
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex);
                                    errorCount++;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("UpdateReferrals Complete");
            Console.WriteLine(string.Format("success: {0} error: {1}", successCount, errorCount));
            Console.ReadKey();
        }
    }

    public class ReferralReturnData
    {
        public int AccountID { get; set; }
        public string PersonID { get; set; }
        public string ReferredByID { get; set; }
    }
}
