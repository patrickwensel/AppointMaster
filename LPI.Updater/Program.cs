using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LPI.Updater.LPIData;
using Newtonsoft.Json;
using NLog;


namespace LPI.Updater
{
    class Program
    {
        const int pageSize = 100;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                RunAsyncUpdateReferrals().Wait();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        static async Task RunAsyncUpdateReferrals()
        {
            Console.WriteLine("Update Start!");
            int referralIndex = 0;
            using (LPIContext context = new LPIContext())
            {
                context.CommandTimeout = 1000;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DentalAPIBaseAddress"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("apikey", ConfigurationManager.AppSettings["DentalAPIKey"]);

                    List<Account> accounts = context.Accounts.ToList();
                    List<string> dentalProcedureIds = context.DentalProcedures.Select(y => y.ID).ToList();

                    foreach (Account account in accounts)
                    {
                        Console.WriteLine("UpdateReferrals for account " + account.ID);
                        List<Patient> patients = context.Patients.Where(x => x.AccountID == account.ID).ToList();
                        int page = 0;
                        bool referralLoadComplete = false;
                        while (!referralLoadComplete)
                        {
                            Console.WriteLine("Getting Referrals page {0}...", page);
                            HttpResponseMessage response = await client.PostAsJsonAsync("api/referrals/GetReferralsByAccountID?page=" + page, account.ID.ToString());
                            if (response.IsSuccessStatusCode)
                            {
                                Task<string> jsonReferralReturnDatas = response.Content.ReadAsStringAsync();
                                List<ReferralReturnData> referralReturnDatas = JsonConvert.DeserializeObject<List<ReferralReturnData>>(jsonReferralReturnDatas.Result);
                                referralLoadComplete = referralReturnDatas.Count < pageSize;
                                Console.WriteLine("Got {0} referrals", referralReturnDatas.Count);
                                Console.WriteLine();

                                foreach (ReferralReturnData referral in referralReturnDatas)
                                {
                                    Console.WriteLine("[{0}] Updating Patient with ID={1}", referralIndex, referral.PersonID);

                                    Patient patient = patients.FirstOrDefault(x => x.ID == referral.PersonID);
                                    if (patient != null)
                                    {
                                        if (referral.ReferredByID != "0")
                                        {
                                            Console.WriteLine("- Updating ReferredBy {0}", referral.ReferredByID);
                                            patient.ReferredBy = referral.ReferredByID;
                                        }
                                        if (referral.ResponsiblePartyId != "0")
                                        {
                                            Console.WriteLine("- Updating ResponsiblePartyId {0}", referral.ResponsiblePartyId);
                                            patient.ResponsibleParty = referral.ResponsiblePartyId;
                                        }

                                        List<string> treatmentIdsToAdd = referral.TreatmentIds.Where(x => !dentalProcedureIds.Contains(x)).ToList();

                                        if (treatmentIdsToAdd.Count > 0)
                                        {
                                            Console.WriteLine("- Getting DetalProcedures...");
                                            int dentalProcedureIndex = 0;
                                            response = await client.PostAsJsonAsync("api/referrals/GetTreatmentsByIds", treatmentIdsToAdd);
                                            if (response.IsSuccessStatusCode)
                                            {
                                                Task<string> jsonReferralTreatmentReturnData = response.Content.ReadAsStringAsync();
                                                List<DentalProcedure> dentalProceduresToAdd = JsonConvert.DeserializeObject<List<DentalProcedure>>(jsonReferralTreatmentReturnData.Result);

                                                Console.WriteLine("- Got {0} DetalProcedures", dentalProceduresToAdd.Count);

                                                foreach (var dentalProcedureToAdd in dentalProceduresToAdd)
                                                {
                                                    Console.WriteLine("--- [{0}] Adding DetalProcedure with ID={1}", dentalProcedureIndex, dentalProcedureToAdd.ID);
                                                    context.AddToDentalProcedures(dentalProcedureToAdd);
                                                    dentalProcedureIndex++;
                                                }
                                            }
                                            else
                                            {
                                                ConsoleLog(ConsoleLogType.Error, string.Format("- Cant get Treatments "));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ConsoleLog(ConsoleLogType.Warning, string.Format("Patient not found", referral.PersonID));
                                    }
                                    referralIndex++;
                                    Console.WriteLine();
                                }
                                Console.WriteLine("Page {0} Saving...", page);
                                try
                                {
                                    context.SaveChanges();
                                    ConsoleLog(ConsoleLogType.Success, "Save changes");
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex);
                                    ConsoleLog(ConsoleLogType.Error, "Cant save changes");
                                }
                                page++;
                                Console.WriteLine();
                            }
                            else
                            {
                                ConsoleLog(ConsoleLogType.Error, string.Format("Cant get referrals for account", account.ID));
                            }
                        }
                    }
                }
            }
            Console.WriteLine("UpdateReferrals Complete!");
            Console.ReadKey();
        }

        static void ConsoleLog(ConsoleLogType type, string message)
        {
            switch (type)
            {
                case ConsoleLogType.Success:
                    message = "SUCCESS: " + message;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case ConsoleLogType.Warning:
                    message = "WARNING: " + message;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case ConsoleLogType.Error:
                    message = "ERROR: " + message;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    break;
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public enum ConsoleLogType
    {
        Success,
        Warning,
        Error
    }

    public class ReferralReturnData
    {
        public int AccountID { get; set; }
        public string PersonID { get; set; }
        public string ReferredByID { get; set; }
        public string ResponsiblePartyId { get; set; }
        public List<string> TreatmentIds { get; set; }
    }
}
