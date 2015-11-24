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
        const int pageSize = 100;
        static int errorCount = 0;
        static int referralIndex = 0;
        static List<string> dentalProcedureIds = new List<string>();
        static LPIContext context = new LPIContext();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            context.CommandTimeout = 1000;
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
            Console.WriteLine("UpdateReferrals Start");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DentalAPIBaseAddress"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("apikey", ConfigurationManager.AppSettings["DentalAPIKey"]);

                List<Account> accounts = context.Accounts.ToList();
                dentalProcedureIds = context.DentalProcedures.Select(y => y.ID).ToList();

                foreach (Account account in accounts)
                {
                    Console.WriteLine("UpdateReferrals for account " + account.ID);
                    //UpdateReferralsForAccountId(client, account.ID).Wait(); TEST GRISHIN
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
                            foreach (ReferralReturnData referral in referralReturnDatas)
                            {
                                Console.WriteLine("[{0}] Updating Patient with ID={1}", referralIndex, referral.PersonID);

                                Patient patient = context.Patients.FirstOrDefault(x => x.ID == referral.PersonID && x.AccountID == referral.AccountID);
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
                                    bool isTreatmentGettingSuccess = UpdateTreatmentsByIds(client, treatmentIdsToAdd).Result;

                                    try
                                    {
                                        context.SaveChanges();
                                        if (isTreatmentGettingSuccess)
                                        {
                                            ConsoleLog(ConsoleLogType.Success, "Save changes");
                                        }
                                        else
                                        {
                                            errorCount++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex);
                                        errorCount++;
                                        ConsoleLog(ConsoleLogType.Error, "Cant save changes");
                                    }
                                }
                                else
                                {
                                    ConsoleLog(ConsoleLogType.Warning, string.Format("Patient {0} not found", referral.PersonID));
                                }
                                referralIndex++;
                                Console.WriteLine();
                            }
                            page++;
                        }
                        else
                        {
                            ConsoleLog(ConsoleLogType.Error, string.Format("Cant get referrals for account", account.ID));
                        }
                    }
                }
            }

            Console.WriteLine("UpdateReferrals Complete with {0} errors", errorCount);
            Console.ReadKey();
        }
        /* TEST GRISHIN
        static async Task UpdateReferralsForAccountId(HttpClient client, int accountId)
        {
            int page = 0;
            bool referralLoadComplete = false;
            while (!referralLoadComplete)
            {
                Console.WriteLine("Getting Referrals page {0}...", page);
                HttpResponseMessage response = await client.PostAsJsonAsync("api/referrals/GetReferralsByAccountID?page=" + page, accountId.ToString());
                if (response.IsSuccessStatusCode)
                {
                    Task<string> jsonReferralReturnDatas = response.Content.ReadAsStringAsync();
                    List<ReferralReturnData> referralReturnDatas = JsonConvert.DeserializeObject<List<ReferralReturnData>>(jsonReferralReturnDatas.Result);
                    referralLoadComplete = referralReturnDatas.Count < pageSize;
                    Console.WriteLine("Got {0} referrals", referralReturnDatas.Count);
                    UpdateReferrals(client, referralReturnDatas);
                    page++;
                }
                else
                {
                    ConsoleLog(ConsoleLogType.Error, string.Format("Cant get referrals for account", accountId));
                }
            }
        }

        static void UpdateReferrals(HttpClient client, List<ReferralReturnData> referralDatas)
        {
            foreach (ReferralReturnData referral in referralDatas)
            {
                Console.WriteLine("[{0}] Updating Patient with ID={1}", referralIndex, referral.PersonID);

                Patient patient = context.Patients.FirstOrDefault(x => x.ID == referral.PersonID && x.AccountID == referral.AccountID);
                if (patient != null)
                {
                    UpdatePatient(patient, referral);
                    List<string> treatmentIdsToAdd = referral.TreatmentIds.Where(x => !dentalProcedureIds.Contains(x)).ToList();
                    bool isTreatmentGettingSuccess = UpdateTreatments(client, treatmentIdsToAdd).Result;

                    try
                    {
                        context.SaveChanges();
                        if (isTreatmentGettingSuccess)
                        {
                            ConsoleLog(ConsoleLogType.Success, "Save changes");
                        }
                        else
                        {
                            errorCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        errorCount++;
                        ConsoleLog(ConsoleLogType.Error, "Cant save changes");
                    }
                }
                else
                {
                    ConsoleLog(ConsoleLogType.Warning, string.Format("Patient {0} not found", referral.PersonID));
                }
                referralIndex++;
                Console.WriteLine();
            }
        }

        static void UpdatePatient(Patient patient, ReferralReturnData referral)
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
        }*/

        static async Task<bool> UpdateTreatmentsByIds(HttpClient client, List<string> treatmentIds)
        {
            if (treatmentIds.Count > 0)
            {
                int dentalProcedureIndex = 0;
                Console.WriteLine("- Getting treatments...");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/referrals/GetTreatmentByIds", treatmentIds);
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
                    return false;
                }
            }
            return true;
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
