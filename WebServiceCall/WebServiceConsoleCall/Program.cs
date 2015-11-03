using System;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using WebServiceConsoleCall.webReference;
using DataBase;
using System.Globalization;

namespace WebServiceConsoleCall
{
    class Program
    {
        static void doc() {
            Console.WriteLine("");
            Console.WriteLine("Practice ROI Batch usage:");
            Console.WriteLine("");
            Console.WriteLine("SYNC      ---> Reads all accounts and create new accounts that need");
            Console.WriteLine("               to be created");
            Console.WriteLine("SYNC TID  ---> Reads account by transactionId");
            //Console.WriteLine("SYNC frmTID toTID ---> Reads account from frmTID");
            //Console.WriteLine("               to totTID");
            Console.WriteLine("RESYNC to from Reads all accounts and create new accounts that need");
            Console.WriteLine("               to be created");
            Console.WriteLine("CDR [date]---> read an update all CDRs leads from Jelez for the[date]");
            Console.WriteLine("               or one month back if no[date] specified");
            Console.WriteLine("    ALL   ---> read an update all CDRs leads from Jelez since");
            Console.WriteLine("               FIRST_CDR_DATE found in c:\\LPI.INI");
            Console.WriteLine("MATCH [DB] [RESET]---> match all leads for the DB with matching");
            Console.WriteLine("               campaign/lead/patient/treatment from AM");
            Console.WriteLine("MATCH [ALL] ---> match all leads for the all accounts with matching");
            Console.WriteLine("                campaign/lead/patient/treatment from AM");
            Console.WriteLine("EMAIL address  Send A test to 'address'");
            Console.WriteLine("updateAm    --->complete all AM data for accounts not completed");
            
            
            
            Console.WriteLine("");
            Console.WriteLine("");

        }

        static void DisplayException(string message) {
            Console.WriteLine();
            Console.WriteLine("X X X X X X X X X X X X X X X X X X X X X X X X X X");
            Console.WriteLine("EXCEPTION:"+message);
            Console.WriteLine("X X X X X X X X X X X X X X X X X X X X X X X X X X");
        }

        protected static string getSQLServerName() {
            try {
                string path = "c:";//                HostingEnvironment.MapPath("~");
                string line = "";
                string server = "";
                System.IO.StreamReader file = new System.IO.StreamReader(path + "\\lpi.ini");
                while ((line = file.ReadLine()) != null) {
                    if (line.StartsWith("server=")) {
                        server = line.Substring(line.IndexOf("server=") + 7);
                    }
                }
                file.Close();
                return server;
            } catch (Exception ex) {
                throw new Exception("getSQLServerName exception:" + ex.Message);
            }
        }

        protected static string getAMServerName() {
            try {
                string path = "c:";//                HostingEnvironment.MapPath("~");
                string line = "";
                string server = "";
                System.IO.StreamReader file = new System.IO.StreamReader(path + "\\lpi.ini");
                while ((line = file.ReadLine()) != null) {
                    if (line.StartsWith("AMserver=")) {
                        server = line.Substring(line.IndexOf("AMserver=") + 9);
                    }
                }
                file.Close();
                return server;
            } catch (Exception ex) {
                throw new Exception("getSQLServerName exception:" + ex.Message);
            }
        }



        static void getLPCDR(int DB, DateTime date) {
            try {
                ServiceManager svrMgr = new ServiceManager(); //Constructor with default CREDENTIALS
               
                List<AccountCampaign> listAccountCampaign = svrMgr.GetAccountCampaigns(new CriteriaAccountsCampaigns() {
                    AccountId = DB
                });

                int TheDate = date.Year * 10000 + date.Month * 100 + date.Day;
                List<LPCDR> listLPCDR = svrMgr.GetCDRs(new CriteriaLPCDR() {
                    Date = TheDate //20110727
                });



                foreach (AccountCampaign camp in listAccountCampaign) {
                    Console.WriteLine("Campaign CampaingnCallNumber " + camp.CampaingnCallNumber+" DB=" + camp.AccountId);
                }
            } catch (Exception ex) {
                DisplayException("getLPCDR :" + ex.Message);
            }
        }

        static string equalString(string s) {
            if (s == null)
                return "";
            else
                return s;
        }

        static void createAppointMasterAccount(SqlConnection cn, Account act, string firstName, string lastName) {
            try{
                PROI_WS.accountDataRequest amAct=new WebServiceConsoleCall.PROI_WS.accountDataRequest();

                amAct.address1 = equalString( act.address);
                amAct.address2 = equalString(act.addressLine2);
                amAct.city = equalString(act.city);
                amAct.contact = equalString(act.mainContact);
                amAct.ID = act.DB+10000;
                amAct.mainPhone = equalString(act.mainPhone);
                amAct.name = equalString(act.name);

                amAct.notificationEmail = equalString(act.notificationEmail);
                amAct.software = WebServiceConsoleCall.PROI_WS.DentalSoftware.Dentrix;//act.software;
                amAct.state = "";//equalString(act.state);
                amAct.website = equalString(act.webSite);
                amAct.zip = equalString(act.zipCode);
                amAct.firstName = firstName;
                amAct.lastName = lastName;
                amAct.userName = act.userId;
                amAct.userPassword = act.password;

                

                
                PROI_WS.ROIServices rws = new WebServiceConsoleCall.PROI_WS.ROIServices();

                PROI_WS.accountDataReply reply = rws.createRoiPractice(amAct);

                bool error=false;
                if (reply.error != ""){
                    if ((reply.error.IndexOf("Error creating user")>=0) |(reply.error.IndexOf("User Already Registered")>=0)){
                        Console.WriteLine("WARNING: "+reply.error);
                        error=false;
                    }else
                        error=true;
                }
                    
                if (!error) {
                    Console.WriteLine("Account Created");

                    act.AMServer = reply.server;
                    act.DataLinkUserId = reply.dataLinkUserId;
                    act.DataLinkPassword = reply.dataLinkPassword;

                    act.DBUpdate("AMServer='" + act.AMServer + "',DataLinkUserId='" + act.DataLinkUserId + "', DataLinkPassword='" + act.DataLinkPassword + "'");

                } else {
                    Console.WriteLine("createAppointMasterAccount exception returned error on AM web service call for account:" + act.DB.ToString() + " " + reply.error);
                }
            } catch (Exception ex) {
                throw new Exception("createAppointMasterAccount Exception:"+ex.Message);
            }
        }//createAppointMasterAccount



        static void checkAccountListAreNew(SqlConnection cn, List<LPAccount> list) {
            Account traceAct = new Account(cn);
            try {
                Console.WriteLine("Total AccountsByTransID account found: " + list.Count);
                if (list.Count > 0) {
                    for (int i = 0; i < list.Count; i++) {
                        LPAccount lpAct = (LPAccount)list[i];
                        Log(" act found:" + lpAct.AccountId);
                        if (!Account.exist(cn, lpAct.AccountId)) {
                            Log(lpAct.AccountId + " " + lpAct.CompanyName + " is New trans ID= "+ lpAct.TransactionId);
                            Account newAct = new Account(cn);

                            newAct.DB = lpAct.AccountId;
                            newAct.name = lpAct.CompanyName;
                            newAct.address = lpAct.Address;
                            newAct.city = lpAct.City;
                            newAct.state = lpAct.State;
                            newAct.zipCode = lpAct.Zip;
                            newAct.userId = lpAct.LoginUsername;
                            newAct.password = lpAct.LoginPassword;
                            newAct.notificationEmail = lpAct.Email;
                            newAct.mainContact = lpAct.FirstName + " " + lpAct.LastName;
                            newAct.mainPhone = lpAct.Phone;
                            newAct.created = DateTime.Now;
                            newAct.status = Status.Active;
                            newAct.trueNewPatientOnly = true;

                            traceAct = newAct;

                            newAct.DBCreate();

                            createAppointMasterAccount(cn, newAct, lpAct.FirstName, lpAct.LastName);
                        } else {
                            /*Log(lpAct.AccountId + " " + lpAct.CompanyName + " already exists >" + lpAct.LoginPassword);
                            Account existingAct = new Account(cn);
                            if (existingAct.DBGet(lpAct.AccountId)) {
                                string updateString = "";
                                if (existingAct.password != lpAct.LoginPassword) {
                                    updateString += "password='" + lpAct.LoginPassword + "',";
                                    Log("password has changed to " + lpAct.LoginPassword);
                                }
                                if (existingAct.userId!= lpAct.LoginUsername) {
                                    updateString += "userId='" + lpAct.LoginUsername + "',";
                                    Log("LoginUsername  has changed to " + lpAct.LoginUsername);
                                }
                                if (existingAct.name != lpAct.CompanyName) {
                                    updateString += "name='" + lpAct.CompanyName + "',";
                                    Log("CompanyName  has changed to " + lpAct.CompanyName);
                                }
                                if (existingAct.notificationEmail != lpAct.Email) {
                                    updateString += "notificationEmail='" + lpAct.Email + "',";
                                    Log("Email has changed to " + lpAct.Email);
                                }
                                if (updateString.EndsWith(",")) {
                                    updateString = updateString.Remove(updateString.Length - 1);
                                }
                                if (updateString!="") {
                                    existingAct.DBUpdate(updateString);
                                }
                            }*/
                        }
                    }
                } else
                    Console.WriteLine("No account found");
            } catch (Exception ex) {
                if (traceAct!=null)
                    throw new Exception("checkAccountListAreNew exception: " + ex.ToString() + " DB=" + traceAct.DB.ToString() + " name=" + traceAct.name + " phone=" + traceAct.mainPhone + " address=" + traceAct.address);
                else
                    throw new Exception("checkAccountListAreNew exception: " + ex.ToString()+" traceCat Null");
            }
        }//checkAccountListAreNew

        static void syncAllAccounts(string[] args) {
            try {
                log = new Log(DataBase.Log.Application.AccountSync);
                using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {

                    Account act = new Account(cn);
                    ServiceManager svrMgr = new ServiceManager(); //Constructor with default CREDENTIALS
                    if (args.Length == 2) {
                        //sync transacId
                        int transactionId = Util.safeIntParse(args[1]);
                        if (transactionId >= 0) {
                            Log("Synchronizing for transactionid " + transactionId.ToString());
                            List<LPAccount> listGetAccountsByTransID = svrMgr.GetAccountsByTransID(new CriteriaAccountsByTransID() {
                                TransactionId = transactionId
                            });
                            checkAccountListAreNew(cn, listGetAccountsByTransID);
                        } else
                            Console.WriteLine("Invalid transaction ID " + args[1]);
                    } else if (args.Length == 3) {
                        //sync fromTransacId totransacId 
                        int fromTransactionId = Util.safeIntParse(args[1]);
                        int toTransactionId = Util.safeIntParse(args[2]);
                        Log("Synchronizing for transactionid " + fromTransactionId.ToString()+" to "+toTransactionId.ToString());
                        if ((fromTransactionId >= 0) & (toTransactionId >= fromTransactionId)) {
                            for (int i = fromTransactionId; i < toTransactionId; i++) {
                                List<LPAccount> listGetAccountsByTransID = svrMgr.GetAccountsByTransID(new CriteriaAccountsByTransID() {
                                    TransactionId = i
                                });
                                checkAccountListAreNew(cn, listGetAccountsByTransID);

                            }
                        } else
                            Console.WriteLine("Invalid transaction ID " + args[1] + " " + args[2]);
                    } else {
                        //new sync
                        Console.WriteLine("Account Synchronization");
                        List<LPAccount> listLatestAccounts = svrMgr.GetLatestAccounts(new CriteriaLatestAccountsList());
                        checkAccountListAreNew(cn, listLatestAccounts);
                    }

                    cn.Close();
                }

            } catch (Exception ex) {
                throw new Exception("syncAllAccounts exception:" + ex.ToString());
            }
        }


        static void reSyncAlls(string[] args) {
            try {
                if (args.Length > 0) {
                    int from = Util.safeIntParse(args[args.Length-2]);
                    int to = Util.safeIntParse(args[args.Length - 1]);

                    if ((to > from) & (from >= 0)) {
                        log = new Log(DataBase.Log.Application.AccountSync);
                        using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {

                            Account act = new Account(cn);


                            Console.WriteLine("Account RE-Synchronization");
                            ServiceManager svrMgr = new ServiceManager(); //Constructor with default CREDENTIALS

                            for (int i = from; i < to; i++) {
                                List<LPAccount> listGetAccountsByTransID = svrMgr.GetAccountsByTransID(new CriteriaAccountsByTransID() {
                                    TransactionId = i
                                });
                                checkAccountListAreNew(cn, listGetAccountsByTransID);

                            }
                            cn.Close();
                        }
                    }
                }

            } catch (Exception ex) {
                throw new Exception("syncAllAccounts exception:" + ex.ToString());
            }
        }




        static string normalizePhoneNumber(string aNumber) {
            if (aNumber.StartsWith("1"))
                return aNumber.Substring(1);
            else
                return aNumber;
        }

        /// <summary>
        /// Check for CDRs received that day. For each CDR found, checks if the account exists, if the campaigns exists or not an create the relevant record
        /// if non existing.
        /// </summary>
        /// <param name="date"></param>
        static void UpdateCDRs( DateTime date) {
            try {
                Log("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                Log("Checking CDR for " + date.ToShortDateString());
                int TheDate = date.Year * 10000 + date.Month * 100 + date.Day;
                ServiceManager svrMgr = new ServiceManager();
                List<LPCDR> listLPCDR = svrMgr.GetCDRs(new CriteriaLPCDR() {
                    Date = TheDate 
                });
                Log("Total CDR found " + listLPCDR.Count);//(LPCDR Object) are 

                DateTime cdrTimeStamp;
                using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {
                    Account account = new Account(cn);
                    Campaign camp = new Campaign(cn);
                    Lead lead = new Lead(cn);
                    DeletedLead deletedLead = new DeletedLead(cn);
                    bool leadIsNew = false;
                    string campaignIdentifier = "";
                    string campaignName = "";
                    for (int i = 0; i < listLPCDR.Count; i++) {
                        if (listLPCDR[i].AccountId == 1278) {
                            if (listLPCDR[i].PrimaryPhone.StartsWith("909948")) {
                                bool debug = true;
                            }
                        }
                    }


                    for (int i = 0; i < listLPCDR.Count; i++) {
                        leadIsNew = false;

                        cdrTimeStamp = DateTime.Parse(listLPCDR[i].TimestampOfCall);
                        //1 we check if the campaigns exist for this account
                        //if (!Campaign.exist(cn, listLPCDR[i].AccountId, listLPCDR[i].CampaignNumber)) {
                        if (listLPCDR[i].IsSourceCampaign) {
                            campaignIdentifier = "MultiSources";
                            campaignName = "Multiple-Sources Campaign";
                        } else {
                            campaignIdentifier = listLPCDR[i].CampaignNumber;
                            campaignName = "Campaign " + listLPCDR[i].CampaignNumber;
                        }

                        if (!camp.DBGet(listLPCDR[i].AccountId, campaignIdentifier)) {
                            camp.Clear();
                            camp.DB = listLPCDR[i].AccountId;
                            camp.name = campaignName;
                            camp.phoneNumber = campaignIdentifier;
                            camp.visible = true;
                            camp.DBCreate();
                            nbNewCDRCampaign++;
                            Log(" >> New Campaign(#" + nbNewCDRCampaign.ToString() + ") DB:" + listLPCDR[i].AccountId.ToString() + " from:" + listLPCDR[i].PrimaryPhone + " ID=" + camp.ID.ToString());
                        } else
                            Console.WriteLine(" Existing Campaign" + camp.ID.ToString());

                        //2 We check if the lead exists now we have the CampId
                        if (!lead.DBGet(listLPCDR[i].AccountId, camp.ID, listLPCDR[i].PrimaryPhone,  cdrTimeStamp,true)) {
                            if (!deletedLead.DBGet(listLPCDR[i].AccountId, camp.ID, listLPCDR[i].PrimaryPhone, cdrTimeStamp, true)) {
                                lead.campaignID = camp.ID.ToString();
                                try {
                                    string format = "MMddyyyy";
                                    lead.birthDate = DateTime.ParseExact(listLPCDR[i].DOB, format, CultureInfo.InvariantCulture);
                                } catch {
                                    lead.birthDate = DateTime.MinValue;
                                }
                                if (!(lead.birthDate.CompareTo(DateTime.MinValue) > 0)) {
                                    lead.birthDate = new DateTime(1900, 1, 1);
                                }
                                lead.durationInMinutes = listLPCDR[i].Duration;
                                lead.fileUrl = listLPCDR[i].IVRFile;
                                lead.inComingNumber = Util.phoneDigits(listLPCDR[i].CallingNumber);
                                lead.PrimaryPhone = Util.phoneDigits(listLPCDR[i].PrimaryPhone);
                                lead.newPatient = true;
                                lead.patientID = "";
                                lead.timeStamp = cdrTimeStamp;
                                lead.source = listLPCDR[i].Source;
                                lead.term = listLPCDR[i].Keyword;//Email Aaron 3/26/2013 Changes to fields labels
                                lead.DB = listLPCDR[i].AccountId;
                                leadIsNew = true;
                                lead.original = lead.IsOriginal();
                                nbNewCDRLeads++;
                                Log(" New Lead (#" + nbNewCDRLeads.ToString() + ") for DB:" + lead.DB.ToString() + " campaign:" + lead.campaignID + " InCmg:" + lead.inComingNumber + " PrmPhn:" + lead.PrimaryPhone + " TmStp:" + lead.timeStamp.ToString());
                            } else
                                Console.WriteLine(" Existing was manually deleted");
                        } else
                            Console.WriteLine(" Existing Lead");

                        if (leadIsNew) {
                            lead.DBCreate();
                        }
                    }
                    cn.Close();
                }
                Tracer.AddWord("");

            } catch (Exception ex) {
                DisplayException(" UpdateCDRs:" + ex.Message);
            }
        }//UpdateCDRs



        private static Lead[] loadLeadsPostedOn(SqlConnection cn, SqlTransaction useTrans, DateTime date, int limitedToDb) {
            try {
                ArrayList leads = new ArrayList();
                Lead ld=new Lead(cn);
                string LeadsFields = ld.getAttributes();
                string[] fields = LeadsFields.Split(',');
                string WC = "select " + LeadsFields;
                /*for (int i = 0; i < fields.Length; i++) {
                    WC += "l." + fields[i] + ",";
                }
                WC = WC.Remove(WC.Length - 1);*/
                WC += " from lead where original=0";
                //WC += " from lead L, campaign C  where L.DB=C.DB and L.campaignId=C.ID and L.original=0";
                //WC = "select l.DB,l.campaignID,l.inComingNumber,l.timeStamp,l.durationMinutes,l.newPatient,l.patientId,l.fileURL,l.birthday,l.dentalNeed,l.dentalCareIsFor,l.preferredAppointmentTime,l.insurancePlanBudget,l.firstName,l.lastName,l.email,l.PrimaryPhone from lead L, campaign C  where L.DB=C.DB and L.campaignId=C.ID and L.original=0";
                //if (limitedToForm) WC += " and c.phoneNumber='FormCapture' ";
                if (limitedToDb!=0)
                    WC += " and DB="+limitedToDb.ToString()+" ";
                if (date.CompareTo(new DateTime(2000,1,1))>0)
                    WC += " and day(timeStamp)=" + date.Day.ToString() + " and month(timeStamp)=" + date.Month.ToString() + " and year(timeStamp)=" + date.Year.ToString();

                WC += " order by timeStamp";

                SqlCommand command ;
                if (useTrans == null)
                    command = new SqlCommand(WC, cn);
                else
                    command = new SqlCommand(WC, cn, useTrans);

                //SqlDataReader DbReader = command.ExecuteReader();
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                while (DbReader.Read()) {
                    Lead l = new Lead(cn);
                    l.fillFromReader(DbReader);
                    leads.Add(l);
                }
                DbReader.Close();
                DbReader.Dispose();
                return (Lead[])leads.ToArray(typeof(Lead));
            } catch (Exception ex) {
                throw new Exception("Exception in loadLeadsPostedOn:" + ex.Message);
            }
        }

        private static int nbDeletedLeads = 0;
        private static int nbDeletedPatients = 0;
        private static int nbDeletedTreatments = 0;

        /// <summary>
        /// Determine based on the treatment that this patient can be considered as a new patient
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static bool genuineNewPatientFromTreatments(PROI_WS.RoiReplyPerPractice[] replys, DateTime fromLeadDate) {
            try {
                log.push();
                bool genuineNewPatient = true;
                if (replys[0].treatments != null) {
                    for (int j = 0; j < replys[0].treatments.Length; j++) {
                        if ((replys[0].treatments[j].date.CompareTo(fromLeadDate.Date) < 0) & (replys[0].treatments[j].date.CompareTo(fromLeadDate.AddMonths(-18).Date) > 0)) {
                            Log("Excluded because at least one treatment found on:" + replys[0].treatments[j].date.ToShortDateString() + " compare to lead date:" + fromLeadDate.ToShortDateString());
                            genuineNewPatient = false;
                            break;
                        }
                    }
                }
                log.pull();
                return genuineNewPatient;
            } catch (Exception ex) {
                throw new Exception("genuineNewPatientFromTreatments exception:"+ex.Message);
            }
        }//genuineNewPatientFromTreatments



        private static string DisplayPhone(string phone) {
            if (string.IsNullOrEmpty(phone))
                return "(---) --- ----";
            else
                return Util.phoneForDisplay(phone);
        }


        private static void DeleteAlltreatments(SqlConnection cn, int DB) {
            try {
                string WC = "delete from dentalProcedure where db=" + DB.ToString();
                SqlCommand cmd = new SqlCommand(WC, cn);
                int nbRow = cmd.ExecuteNonQuery();
                Log(nbRow.ToString() + " deleted for DB " + DB.ToString());
            } catch (Exception ex) {
                throw new Exception("DeleteAlltreatments ex:" + ex.Message);
            }
        }//DeleteAlltreatments

        /// <summary>
        /// Check for all FormCapture lead received that day 
        /// if non existing.
        /// </summary>
        /// <param name="date"></param>
        static void MatchLeads(SqlConnection cn,int DB, Lead[] leads, PROI_WS.ROIServices rws , bool phoneOnly, bool limitToTrueNewPatient, string excludedCsvCodes) {
            try {
                log.push();
                nbAccountScanned++;
                Lead lead = new Lead(cn);
                bool go = false;
                bool leadAlreadyIdentified = false;
                int nbError = 0;
                bool deleteAllTreatmentFirst = false;
                string ClearTrdtDbs = Util.GetConfigValue("CLEAR_TREATMENT_ON_MATCH_DBS");
                if (!string.IsNullOrEmpty(ClearTrdtDbs)) {
                    string[] dbs = ClearTrdtDbs.Split(',');
                    foreach (string sdb in dbs)
                        if (sdb == DB.ToString()) {
                            deleteAllTreatmentFirst = true;
                            break;
                        }
                }
                if (leads.Length>0)
                    if (deleteAllTreatmentFirst) {
                        Log(" Matching lead Deleting all treatments for DB=" + DB.ToString());
                        DeleteAlltreatments(cn, DB);
                    }


                bool direcmatching = true;
                for (int i = 0; i < leads.Length; i++) {
                    leadAlreadyIdentified = false;
                    direcmatching = true;
                    go = true;
                    lead = leads[i];
                    Log("");
                    if ((lead.patientID == "") | (lead.patientID == null))
                        Log("Searching Matches for Lead DB:" + lead.DB.ToString()+"."+lead.ID + " Phone Incoming:" + DisplayPhone(lead.inComingNumber) + " Phone Primary:" + DisplayPhone(lead.PrimaryPhone) + " Name:" + lead.lastName + " Stamp:" + lead.timeStamp.ToString() + " (No Patient Identified Yet).");// +" (No patient attached to this lead)"
                    else {
                        Log("Searching New Matches for Lead DB:" + lead.DB.ToString() + "." + lead.ID + " Phone Incoming:" + DisplayPhone(lead.inComingNumber) + " Phone Primary:" + DisplayPhone(lead.PrimaryPhone) + " Name:" + lead.lastName + " Stamp:" + lead.timeStamp.ToString() + " PatID:" + lead.patientID);// + " (Patient already identified)"
                        leadAlreadyIdentified = true;
                    }
                    Log("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                    
                    PROI_WS.RoiRequestPerPractice[] request = new WebServiceConsoleCall.PROI_WS.RoiRequestPerPractice[1];
                    request[0] = new WebServiceConsoleCall.PROI_WS.RoiRequestPerPractice();
                    if ((lead.patientID == "")|(lead.patientID == null)) {
                        //So far the lead has not found a matching, we try again
                        //request[0].patientBirthDate = lead.birthDate;
                        request[0].patientPhone0 = normalizePhoneNumber(lead.PrimaryPhone);//Primary is definitively the most important
                        request[0].patientPhone1 = normalizePhoneNumber(lead.inComingNumber);
                        if (!phoneOnly) {
                            request[0].patientFirstName = lead.firstName;
                            request[0].patientName = lead.lastName;
                            request[0].patientEmail0 = lead.email;
                        }

                        Log("Calling AM(1) for Phone:" + Util.phoneForDisplay(request[0].patientPhone0) + " Name:" + lead.lastName+" Fname:"+lead.firstName+" @:"+lead.email);
                    } else {
                        request[0].patientID = lead.patientID;
                        
                        Log("Calling AM(2) for patId:" + request[0].patientID);
                    }
                    request[0].LeadDate = lead.timeStamp;

                    Console.WriteLine("AM Server is " + rws.Url);

                    PROI_WS.RoiReplyPerPractice[] replys = null;

                    try {
                        //*******************************************************
                        replys = rws.getRoiPerPractice(lead.DB, request);
                        //*******************************************************
                    } catch (Exception ex2) {
                        nbError++;
                        Log("Service Call (getRoiPerPractice) Exception(#" + nbError.ToString() + "):" + ex2.Message);
                    }
                    if (replys != null) {
                        if (string.IsNullOrEmpty(replys[0].patientID)) {
                            if (!string.IsNullOrEmpty(lead.alterPrimaryPhone)) {
                                //Trying with the alternate phone
                                Log("No Match found for lead " + lead.ID + " trying with alternate phone:" + lead.alterPrimaryPhone + " alt. name=" + lead.alterName + " alt. first name=" + lead.alterFirstName);
                                request[0].patientPhone0 = normalizePhoneNumber(lead.alterPrimaryPhone);//Primary is definitively the most important
                                request[0].patientName = lead.alterName;//Primary is definitively the most important
                                request[0].patientFirstName = lead.alterFirstName;//Primary is definitively the most important
                                //*******************************************************
                                replys = rws.getRoiPerPractice(lead.DB, request);
                                //*******************************************************
                                direcmatching = false;
                            }
                        }
                    }


                    if (replys != null) {
                        log.push();
                        Patient patient = new Patient(cn);
                        if (!string.IsNullOrEmpty(replys[0].patientID)) {
                            if (!replys[0].patientID.StartsWith("NP")) {
                                if (direcmatching)
                                    lead.DBUpdateMatchingStatus(Lead.MatchingStatus.NormalMatch); 
                                else
                                    lead.DBUpdateMatchingStatus(Lead.MatchingStatus.AlternativeMatch);
                                Log("Match found for patient ID " + replys[0].patientID);
                                //We check if the patientID found is not already associated with another lead, in which we 'derate' the current lead to not original
                                if (lead.FoundLeadForPatientID(replys[0].patientID)) {
                                    Log(" >>>>>>>>>> PATIENTID already associated with an existing lead. Lead demoted");
                                    lead.DBUpdateOriginalStatus(Lead.OriginalStatus.DuplicateMatching);
                                } else {
                                    nbNewMatchedPatientFound++;

                                    if (leadAlreadyIdentified)
                                        go = true;
                                    else {
                                        if (limitToTrueNewPatient) {
                                            go = genuineNewPatientFromTreatments(replys, lead.timeStamp);
                                        } else
                                            go = true;
                                    }

                                    if (go) {
                                        if ((replys[0].patientID.IndexOf("ERROR") >= 0) | ((replys[0].error != null) & (replys[0].error != ""))) {
                                            Log("Matching Server Error Returned:" + replys[0].error);
                                        } else {
                                            //A match has been found

                                            //Checking if the lead has the patient ID
                                            if ((lead.patientID == "") | (lead.patientID == null)) {
                                                Log("New Match for this lead");
                                                lead.patientID = replys[0].patientID;
                                                lead.DBUpdatePatientId(replys[0].patientID);
                                            } else {
                                                if (lead.patientID == replys[0].patientID)
                                                    Log("Patient match already known this lead");
                                                else {
                                                    Log("Patient match (#" + replys[0].patientID + ") override already known patient this lead (#" + lead.patientID + ")");
                                                    lead.patientID = replys[0].patientID;
                                                    lead.DBUpdatePatientId(replys[0].patientID);
                                                }
                                            }

                                            //We check if the patient has been registered already
                                            if (!patient.DBGet(lead.DB, replys[0].patientID)) {
                                                Log("creating a new patient");
                                                patient.DB = lead.DB;
                                                patient.ID = replys[0].patientID;
                                                patient.firstName = replys[0].firstName;
                                                patient.name = replys[0].Name;
                                                patient.birthDate = lead.birthDate;
                                                patient.created = DateTime.Today;
                                                patient.homePhone = normalizePhoneNumber(lead.inComingNumber);
                                                patient.DBCreate();

                                                nbNewPatientFound++;
                                                if (matchedAccounts.IndexOf(lead.DB) < 0)
                                                    matchedAccounts.Add(lead.DB);
                                            } else {
                                                Log("existing patient in database");
                                            }

                                            //Now checking if treatments have been registered 
                                            // and we build the list of appointment Dates returned by AppointMaster



                                            if (replys[0].treatments != null) {


                                                nbMatchedTreatmentsFound += replys[0].treatments.Length;
                                                ArrayList apptDates = new ArrayList(replys[0].treatments.Length);

                                                Log("Found " + replys[0].treatments.Length.ToString() + "  treatments");
                                                log.push();
                                                DentalProcedure dp = new DentalProcedure(cn);
                                                DentalProcedure existingDp = new DentalProcedure(cn);
                                                bool okToInsert = true;
                                                string[] excludedCode = excludedCsvCodes.Split(',');
                                                for (int j = 0; j < replys[0].treatments.Length; j++) {
                                                    okToInsert = true;

                                                    foreach (string code in excludedCode)
                                                        if (code.ToUpper() == replys[0].treatments[j].trtCode.ToUpper())
                                                            okToInsert = false;

                                                    if (replys[0].treatments[j].date.CompareTo(lead.timeStamp.AddDays(-8)) >= 0) {

                                                        if (okToInsert) {
                                                            //For each treatment, we search if the treatment has been created already

                                                            bool Exist = dp.DBGet(lead.DB, replys[0].treatments[j].ID);
                                                            if (Exist) {
                                                                existingDp.amount = dp.amount;
                                                                existingDp.code = dp.code;
                                                                existingDp.TreatmentPlan = dp.TreatmentPlan;
                                                                existingDp.patientId = dp.patientId;
                                                                existingDp.dateTime = dp.dateTime;
                                                            }

                                                            if (replys[0].patientID != null) {
                                                                Log("--> New treatment #" + replys[0].treatments[j].ID + " " + replys[0].treatments[j].date.ToShortDateString() + " TP:" + replys[0].treatments[j].TreatmentPlan.ToString() + " " + replys[0].treatments[j].amount.ToString("C0"));
                                                                if (replys[0].patientID != "") {
                                                                    dp.DB = lead.DB;
                                                                    dp.ID = replys[0].treatments[j].ID;
                                                                    dp.amount = replys[0].treatments[j].amount;
                                                                    dp.apptId = replys[0].treatments[j].apptId;
                                                                    dp.code = replys[0].treatments[j].trtCode;
                                                                    dp.TreatmentPlan = replys[0].treatments[j].TreatmentPlan;
                                                                    if (!dp.TreatmentPlan)
                                                                        okToInsert = (replys[0].treatments[j].date.CompareTo(DateTime.Now) <= 0);
                                                                    if (okToInsert) {
                                                                        dp.dateTime = replys[0].treatments[j].date;
                                                                        bool found = false;
                                                                        for (int k = 0; k < apptDates.Count; k++) {
                                                                            if (dp.dateTime.Date.CompareTo(((Appointment)apptDates[k]).dateTime.Date) == 0) {
                                                                                found = true;
                                                                                break;
                                                                            }
                                                                        }
                                                                        if (!found) {
                                                                            Appointment appt = new Appointment(cn);
                                                                            appt.dateTime = dp.dateTime;
                                                                            appt.DB = lead.DB;
                                                                            appt.patientId = replys[0].patientID;
                                                                            apptDates.Add(appt);
                                                                        }
                                                                        dp.patientId = replys[0].patientID;
                                                                        if (Exist) {
                                                                            Log("--> Treatment Found already #" + replys[0].treatments[j].ID);
                                                                            if (existingDp.TreatmentPlan ^ dp.TreatmentPlan) {
                                                                                Log("  - Treatment Plan Status Change to -->" + dp.TreatmentPlan.ToString());
                                                                                if (dp.TreatmentPlan)
                                                                                    dp.DBUpdate("TreatmentPlan", "1");
                                                                                else {
                                                                                    dp.DBUpdate("TreatmentPlan", "0");
                                                                                    newTreatmentsValue += dp.amount;
                                                                                }
                                                                            }
                                                                            if (existingDp.amount != dp.amount) {
                                                                                Log("  - Amount to -->" + dp.amount.ToString("N"));
                                                                                dp.DBUpdate("amount", dp.amount.ToString("N").Replace(",", ""));
                                                                            }

                                                                        } else {
                                                                            dp.DBCreate();
                                                                            nbNewTreatmentsFound++;
                                                                            newTreatmentsValue += dp.amount;
                                                                            if (matchedAccounts.IndexOf(lead.DB) < 0)
                                                                                matchedAccounts.Add(lead.DB);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    } else {
                                                        Log(" -- treatment Prior Lead Date  : " + replys[0].treatments[j].date.ToShortDateString());
                                                    }
                                                }//for
                                                log.pull();

                                                //We check if we have to create appointments for these results. Appointments being based on the date only
                                                for (int j = 0; j < apptDates.Count; j++) {
                                                    Appointment appt = (Appointment)apptDates[j];
                                                    if (!Appointment.exist(cn, lead.DB, appt.dateTime, appt.patientId)) {
                                                        appt.DBCreate();
                                                    }
                                                }
                                            } else {
                                                Log("No treatments posted or planed");
                                            }
                                        }
                                    } else {
                                        //This lead has to be deleted because this is not a genuine patient
                                        //Also the associated patient if found and all treatment
                                        Log(" >>>>>>>>>>>>>>> NOT A NEW PATIENT");
                                    }
                                }
                            } else {
                                Log(" 'NP' Found in ID " + replys[0].patientID+" invalid ID");
                            }
                        } else {
                            Log("No match found");
                            lead.DBUpdateMatchingStatus(Lead.MatchingStatus.NoNatch);
                        }
                    }
                    log.pull();
                }
            } catch (Exception ex) {
                DisplayException("MatchLeads:" + ex.Message);
            }
        }//MatchLeads

        static void ResetMatches(int DB) {
            try{
                Log("Reseting matches for " + DB.ToString());
                using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {

                    SqlCommand cmd = cn.CreateCommand();
                    SqlTransaction transaction;
                    
                    // Start a local transaction.
                    transaction = cn.BeginTransaction("Reset"+DB.ToString());
                    cmd.Connection=cn;
                    cmd.Transaction = transaction;
                    try{
                        cmd.CommandText="delete from patient where db="+DB.ToString();
                        int nbPatDeleted=cmd.ExecuteNonQuery();

                        cmd.CommandText = "delete from lead where db=" + (-DB).ToString();
                        int nbOldLeadDeleted = cmd.ExecuteNonQuery();


                        cmd.CommandText="delete from dentalprocedure where db="+DB.ToString();
                        int nbDentalProcDeleted = cmd.ExecuteNonQuery();

                        cmd.CommandText = "update lead set patientid='' where db=" + DB.ToString();
                        int nbLead = cmd.ExecuteNonQuery();

                        cmd.CommandText = "update lead set original=0, DB=" + (-DB).ToString() + " where db=" + DB.ToString();// +" and original in (5,6)";
                        int nbLeadOriginalReset = cmd.ExecuteNonQuery();


                        Log(nbPatDeleted.ToString()+" patients deleted, "+nbDentalProcDeleted.ToString()+" dental procedure deleted, "+nbLead.ToString()+" leads updated");

                        SqlCommand cmd1;
                        int nbOriginal = 0;
                        Lead[] leads = loadLeadsPostedOn(cn, transaction, DateTime.MinValue, -DB);
                        Log("Re-checking " + leads.Length.ToString() + " leads");
                        Console.Write("Checking:");

                        for (int i = 0; i < leads.Length; i++) {
                            Lead l = leads[i];
                            l.DB = DB;
                            if (l.ID == 155624)
                                l.ID = l.ID;
                            l.original = l.IsOriginal(transaction);
                            if (l.original == Lead.OriginalStatus.Original) {
                                nbOriginal++;
                            }
                            string WC = "update lead set original=" + ((int)l.original).ToString() + ", DB=" + DB.ToString() + " where DB=-" + DB.ToString() + " and id=" + l.ID.ToString();
                            cmd1 = new SqlCommand(WC, cn, transaction);
                            int nbRow = cmd1.ExecuteNonQuery();
                            if (nbRow != 1)
                                nbRow = nbRow;
                            cmd1.Dispose();
                            cmd1 = null;
                            Console.Write(l.original.ToString().Substring(0, 1));
                        }

                        transaction.Commit();

                    }catch(Exception e){
                        Log("ResetMatches Commit Exception Type:"+ e.GetType());
                        Log("  Message:"+ e.Message);

                        // Attempt to roll back the transaction. 
                        try {
                            transaction.Rollback();
                        } catch (Exception ex2) {
                            // This catch block will handle any errors that may have occurred 
                            // on the server that would cause the rollback to fail, such as 
                            // a closed connection.
                            Log("ResetMatches Rollback Exception Type:"+ ex2.GetType());
                            Log("  Message:"+ ex2.Message);
                        }
                    }
                }
            } catch (Exception ex) {
                DisplayException("ResetMatches:" + ex.Message);
            }
        }//ResetMatches

        static void MatchLeadsForAccount(int DB,bool traceResult) {
            try {
                log.push();
                Log("-----------------------------------------------------------------------");
                Log("Matching Leads for account " + DB.ToString());
                Log("-----------------------------------------------------------------------");
                
                using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {
                    Account act = new Account(cn);
                    if (act.DBGet(DB)) {
                        Log("True new patient:"+act.trueNewPatientOnly.ToString());
                        log.push();
                        Lead[] leads = loadLeadsPostedOn(cn,null, DateTime.MinValue, DB);

                        if (leads.Length > 0) {
                            Log("Found " + leads.Length.ToString() + " leads");
                            nbMatchedTreatmentsFound = 0;
                            nbNewMatchedPatientFound = 0;
                            PROI_WS.ROIServices rws = new WebServiceConsoleCall.PROI_WS.ROIServices();
                            rws.Url = getAMServerName();//"https://practiceroi.appointmaster.com/PROIadmin/ROIServices.asmx";//"http://localhost/PROIAdmin/ROIServices.asmx";
                            rws.Timeout = 5*60*1000;//5 minutes timeout!!!
                            MatchLeads(cn, DB, leads, rws, false, act.trueNewPatientOnly, act.ExcludedCodes);
                            Log("Patient   match for act #" + DB.ToString() + "  total:" + nbNewMatchedPatientFound.ToString());
                            Log("Treatment match for act #" + DB.ToString() + "  total:" + nbMatchedTreatmentsFound.ToString());
                        } else {
                            Log("No Leads");
                        }
                        log.pull();
                    } else
                        Log("Could not get account " + DB.ToString());
                    cn.Close();
                }
                log.pull();
                if (traceResult)
                    traceResults(true);
            } catch (Exception ex) {
                DisplayException("UpdateCDRs:" + ex.Message);
            }
        }
        
        /// <summary>
        /// Check for all FormCapture lead received that day 
        /// if non existing.
        /// </summary>
        /// <param name="date"></param>
        static void UpdateRecentForms(DateTime date) {
            try {
                Console.WriteLine("Checking Recent Form posted for "+date.ToShortDateString());
                
                using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {
                    /*
                    Lead[] leads = loadLeadsPostedOn(cn, date, true, -1);
                    Console.WriteLine("Found " + leads.Length.ToString() );
                    MatchLeads(cn, leads,true,false);
                    
                    
                    Lead lead = new Lead(cn);
                    for (int i = 0; i < leads.Length; i++) {
                        lead = leads[i];
                        Console.WriteLine(" DB:" + lead.DB.ToString() + " phone:" + lead.inComingNumber);

                        PROI_WS.RoiRequestPerPractice[] request = new WebServiceConsoleCall.PROI_WS.RoiRequestPerPractice[1];
                        request[0] = new WebServiceConsoleCall.PROI_WS.RoiRequestPerPractice();
                        if (lead.patientID == "") {
                            //So far the lead has not found a matching, we try again
                            //request[0].patientBirthDate = lead.birthDate;
                            request[0].patientPhone0 = normalizePhoneNumber(lead.inComingNumber);
                            Console.WriteLine("Requesting AM(1) for Phone:" + request[0].patientPhone0);
                        } else {
                            request[0].patientID = lead.patientID;
                            Console.WriteLine("Requesting AM(2) for patId:" + request[0].patientID);
                        }

                        PROI_WS.ROIServices rws = new WebServiceConsoleCall.PROI_WS.ROIServices();
                        rws.Url = getAMServerName();//"https://practiceroi.appointmaster.com/PROIadmin/ROIServices.asmx";//"http://localhost/PROIAdmin/ROIServices.asmx";
                        Console.WriteLine("AM Server is " + rws.Url);

                        PROI_WS.RoiReplyPerPractice[] replys = rws.getRoiPerPractice(lead.DB, request);

                        

                        Patient patient = new Patient(cn);
                        if (replys[0].patientID != "") {
                            bool error = false;
                            if ( (replys[0].patientID.IndexOf("ERROR")>= 0) | ( (replys[0].error!=null)& (replys[0].error!="") ))  {
                                Console.WriteLine(" Error" + replys[0].error);
                            } else {

                                Console.WriteLine(" Match found pat id:" + replys[0].patientID);
                                //A match has been found

                                //Checking if the lead has the patient ID
                                if (lead.patientID == "") {
                                    lead.patientID = replys[0].patientID;
                                    lead.DBUpdatePatientId(replys[0].patientID);
                                }

                                //We check if the patient has been registered already
                                if (!patient.DBGet(lead.DB, replys[0].patientID)) {
                                    patient.DB = lead.DB;
                                    patient.ID = replys[0].patientID;
                                    patient.firstName = replys[0].firstName;
                                    patient.name = replys[0].Name;
                                    patient.birthDate = lead.birthDate;
                                    patient.created = DateTime.Today;
                                    patient.homePhone = normalizePhoneNumber(lead.inComingNumber);
                                    patient.DBCreate();
                                    Console.WriteLine("  new patient");
                                } else {
                                    Console.WriteLine("  existing patient");
                                }

                                //Now checking if treatments have been registered 
                                // and we build the list of appointment Dates returned by AppointMaster



                                if (replys[0].treatments != null) {
                                    ArrayList apptDates = new ArrayList(replys[0].treatments.Length);

                                    Console.WriteLine(" " + replys[0].treatments.Length.ToString() + "  treatments");
                                    DentalProcedure dp = new DentalProcedure(cn);
                                    for (int j = 0; j < replys[0].treatments.Length; j++) {
                                        //For each treatment, we search if the treatment has been created already
                                        if (!dp.DBGet(lead.DB, replys[0].treatments[j].ID)) {
                                            if (replys[0].patientID != null) {
                                                if (replys[0].patientID != "") {
                                                    dp.DB = lead.DB;
                                                    dp.ID = replys[0].treatments[j].ID;
                                                    dp.amount = replys[0].treatments[j].amount;
                                                    dp.apptId = replys[0].treatments[j].apptId;
                                                    dp.code = replys[0].treatments[j].trtCode;
                                                    dp.dateTime = replys[0].treatments[j].date;
                                                    bool found = false;
                                                    for (int k = 0; k < apptDates.Count; k++) {
                                                        if (dp.dateTime.Date.CompareTo(((Appointment)apptDates[k]).dateTime.Date) == 0) {
                                                            found = true;
                                                            break;
                                                        }
                                                    }
                                                    if (!found) {
                                                        Appointment appt = new Appointment(cn);
                                                        appt.dateTime = dp.dateTime;
                                                        appt.DB = lead.DB;
                                                        appt.patientId = replys[0].patientID;
                                                        apptDates.Add(appt);
                                                    }
                                                    dp.patientId = replys[0].patientID;
                                                    dp.DBCreate();
                                                }
                                            }
                                        }//else treatment exists
                                    }//for

                                    //We check if we have to create appointments for these results. Appointments being based on the date only
                                    for (int j = 0; j < apptDates.Count; j++) {
                                        Appointment appt = (Appointment)apptDates[j];
                                        if (!Appointment.exist(cn, lead.DB, appt.dateTime, appt.patientId)) {
                                            appt.DBCreate();
                                        }
                                    }
                                } else {
                                    Console.WriteLine(" No treatments");
                                }
                            }
                        } else {
                            Console.WriteLine("No match found");
                        }
                        
                    
                    }
                    */
                    cn.Close();
                }

            } catch (Exception ex) {
                DisplayException("UpdateRecentForms:" + ex.Message);
            }
        }//UpdateRecentForms


        static void test() {
            try {
                ServiceManager svrMgr = new ServiceManager(); //Constructor with default CREDENTIALS

                for (int i = 0; i < 100; i++) {
                    //List<AccountCampaign> listAccountCampaign = svrMgr.GetAccountCampaigns(new CriteriaAccountsCampaigns() {
                    List<AccountCampaign> listAccountCampaign = svrMgr.GetAccountsCampaignsByTransID(new CriteriaAccountsCampaignsByTransID() {
                        AccountId = 1113,
                        TransactionId =i
                    });
                    Console.WriteLine("Total AccountCampaign for 1113 " + i.ToString() + " Object are " + listAccountCampaign.Count.ToString());
                    if (listAccountCampaign.Count > 0) {
                        bool debug = false;
                        break;
                    }
                }
                UpdateCDRs(new DateTime(2011, 08, 25));
                UpdateCDRs(new DateTime(2011, 07, 27));

                //getLPCDR( 1113, new DateTime(2011,7,27));
                //ServiceManager svrMgr = new ServiceManager("acb", "abc"); //Constructor with User/Pwd values




                //List<LPCDR> listLPCDR = svrMgr.GetCDRs(new CriteriaLPCDR() {                    Date = 20110214                });
                List<LPCDR> listLPCDR = svrMgr.GetCDRs(new CriteriaLPCDR() {
                    Date = 20110727
                });



                Console.WriteLine("Total LPCDR Object are " + listLPCDR.Count);

                
                List<AccountCampaign> listAccountCampaignByTransID = svrMgr.GetAccountsCampaignsByTransID(new CriteriaAccountsCampaignsByTransID() {
                    TransactionId = 1
                });
                Console.WriteLine("Total AccountCampaignByTransID Object are " + listAccountCampaignByTransID.Count);

                List<LPAccount> listLatestAccounts = svrMgr.GetLatestAccounts(new CriteriaLatestAccountsList());
                Console.WriteLine("Total LatestAccounts Object are " + listLatestAccounts.Count);

                List<LPAccount> listGetAccountsByTransID = svrMgr.GetAccountsByTransID(new CriteriaAccountsByTransID() {
                    TransactionId = 3
                });
                Console.WriteLine("Total AccountsByTransID Object are " + listGetAccountsByTransID.Count);

                Console.WriteLine("Press any key to close program...");
                Console.ReadLine();

            } catch (Exception ex) {
                throw ex;
            }
        }//test

        static void readRecentsCDR(string[] args) {
            try {
                log = new Log(DataBase.Log.Application.CDRChecking);
                DateTime from = DateTime.Today;
                DateTime to= DateTime.Today;
                if (args.Length == 3) {
                    from = DateTime.Parse(args[1]);
                    to = DateTime.Parse(args[2]);
                } else if (args.Length == 2) {
                    string sDate = args[1];
                    if (sDate == "ALL") {
                        sDate = Util.GetConfigValue("FIRST_CDR_DATE");
                    } else {
                        to = DateTime.Parse(sDate);
                    }
                    from = DateTime.Parse(sDate);
                } else {
                    //by default one month back from today
                    from = DateTime.Today.AddMonths(-1);
                }

                log = new Log(DataBase.Log.Application.CDRChecking);
                Log("************************************************************************************");
                Log("CDR Checking From "+from.ToShortDateString()+" to "+to.ToShortDateString());
                Log("************************************************************************************");

                while (to.CompareTo(from) >= 0) {
                    UpdateCDRs(to);
                    to = to.AddDays(-1);
                }

                traceResults(false);

            } catch (Exception ex) {
                DisplayException("readRecentsCDR:" + ex.Message);
            }
        }


        static void MatchLeadsForAllAccounts() {
            try {
                ArrayList dbs = new ArrayList();
                int db = 0;
                using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {
                    SqlCommand command = new SqlCommand("select Db from account", cn);
                    SqlDataReader DbReader = command.ExecuteReader();
                    while (DbReader.Read()) {
                        db = DbReader.GetInt32(0);
                        dbs.Add(db);
                    }
                    DbReader.Close();
                    DbReader.Dispose();
                    cn.Close();
                }
                for (int i=0;i<dbs.Count;i++){
                    db = (int)dbs[i];
                    Console.WriteLine("----------------------- MATCHING LEADS FOR ACT #" + db.ToString());
                    MatchLeadsForAccount(db,false);
                }
                traceResults(true);

            } catch (Exception ex) {
                DisplayException("MatchLeadsForAllAccounts:" + ex.Message);
            }
        }

        static bool paramFound(string param, string[] args) {
            foreach (string s in args)
                if (param.ToUpper() == s.ToUpper())
                    return true;
            return false;
        }

        static void matchLeads(string[] args) {
            try {
                int DB = 0;
                if (args.Length > 1) {
                    DB= Util.safeIntParse(args[1]);
                    if (DB > 0) {
                        log = new Log(DataBase.Log.Application.MatchingAccount);
                        if (paramFound("RESET", args))
                            ResetMatches(DB);
                        MatchLeadsForAccount(DB,true);
                    } else {
                        if (args[1].ToUpper() == "ALL") {
                            log = new Log(DataBase.Log.Application.MatchingALL);
                            MatchLeadsForAllAccounts();
                        } else
                            Console.WriteLine("DB>0 or 'ALL'");
                    }
                } else
                    Console.WriteLine("Argument missing (DB)");
            } catch (Exception ex) {
                DisplayException("matchLeads:" + ex.Message);
            }
        }//matchLeads

        static public string getContentForEmail() {
            string ma = "";
            for (int i = 0; i < matchedAccounts.Count; i++) {
                if (ma != "")
                    ma += ",";
                ma += " #"+((int)matchedAccounts[i]).ToString();
            }
            System.TimeSpan duration = DateTime.Now.Subtract(start);


            string s = "";
            s += "<body style='font-family:Arial;font-size:12px;'>";
            s += "<p><small>Server:" + DataBase.Util.GetConfigValue("HOST_SERVER") + "</small>";
            s += "<p><small>- - - - - - - THIS IS AN AUTOMATIC NOTIFICATION. PLEASE DO NOT REPLY - - - - - - - </small>";
            s += "<p>We are happy to report that we have found new matches on your database for <b><big>" + matchedAccounts.Count.ToString() + "</big></b>/" + nbAccountScanned.ToString() + " account(s).<br>Please find below the detail:<p>";
            s += "<table style='font-size:12px; border-size:1px' border=1 cellpadding=10 >";
            s += "<tr><th style='background-color:#E0E0E0'>Account(s) Matching</th><td>" + ma + "</td></tr>";
            s += "<tr><th style='background-color:#E0E0E0'>Number of New Patients Found</th><td align=right>" + nbNewPatientFound.ToString() + "</td></tr>";
            s += "<tr><th style='background-color:#E0E0E0'>Number of New Treatments Found</th><td align=right>" + nbNewTreatmentsFound.ToString() + "</td></tr>";
            s += "<tr><th style='background-color:#E0E0E0'>$ Value of New Matches For This Batch</th><td align=right><b>" + String.Format("{0:C}", newTreatmentsValue) + "</b></td></tr>";
            s += "</table>";
            s += "<small><br><br>Matching done on <b>" + DateTime.Now.ToString()+"</b>";
            s += "<br>Matching process duration " + duration.ToString();
            s += "<br>";
            s += "<br>";
            s += "<a href='http://practiceRoi.appointmaster.com/lpilogs/"+System.IO.Path.GetFileName(log.filename)+"'>Detail Log For This Macthing Process</a>";
            s += "<br>";
            s += "<br>";
            s += "</small><br><br>Thank you";
            s += "<br><br>LAMP-Tech Team";
            s += "</body>";
            return s;
        }

        static public string getCDRContentForEmail() {
            System.TimeSpan duration = DateTime.Now.Subtract(start);
            string s = "";
            s += "<body style='font-family:Arial;font-size:12px;'>";
            s += "<p><small>Server:" + DataBase.Util.GetConfigValue("HOST_SERVER") + "</small>";
            s += "<p><small>- - - - - - - THIS IS AN AUTOMATIC NOTIFICATION. PLEASE DO NOT REPLY - - - - - - - </small>";
            s += "<p>We are happy to report that a new CDR sync has been processed";
            s += "<table style='font-size:12px; border-size:1px' border=1 cellpadding=10 >";
            s += "<tr><th style='background-color:#E0E0E0'>New Campaigns found</th><td>" + nbNewCDRCampaign.ToString() + "</td></tr>";
            s += "<tr><th style='background-color:#E0E0E0'>New Leads found</th><td align=right>" + nbNewCDRLeads.ToString() + "</td></tr>";
            s += "</table>";

            s += "<br><br>Matching process duration " + duration.ToString();
            s += "<br>";
            s += "<br>";
            s += "<a href='http://practiceRoi.appointmaster.com/lpilogs/" + System.IO.Path.GetFileName(log.filename) + "'>Detail Log For This CDR Process</a>";
            s += "<br>";
            s += "<br>";
            s += "</small><br><br>Thank you";
            s += "<br><br>LAMP-Tech Team";
            s += "</body>";
            return s;
        }



        private static void logResults(){
            Log("");
            Log("");
            Log("MATCHES SUMMARY");
            Log("-----------------");
            Log("Nb New Patient(s) Found:" + nbNewPatientFound.ToString());
            Log("Nb New Treatmt(s) Found:" + nbNewTreatmentsFound.ToString());
            Log("New Value Found        :" + newTreatmentsValue.ToString("C0"));
            Log("New CDR Leads          :" + nbNewCDRLeads.ToString());
            Log("New CDR Campaigns      :" + nbNewCDRCampaign.ToString());
            
        }

        private static void traceResults(bool forMatching) {
            logResults();
            Log("");

            if (Util.GetConfigValue("EMAIL_SENDING") == "ALL") {
                string subject = "New matches found for X" + matchedAccounts.Count.ToString() + " LPI account(s)";
                int nbemail = 0;
                Email e = new Email(0);
                if (forMatching)
                    e.content = getContentForEmail();
                else {
                    e.content = getCDRContentForEmail();
                    subject = "New CDR Process Result (for "+DateTime.Today.ToShortDateString()+")";
                }
                if  (((nbNewPatientFound > 0) | (nbNewTreatmentsFound > 0) | (nbNewCDRLeads > 0) | (nbNewCDRCampaign>0))){
                    for (int i=0;i<10;i++){
                        string email=Util.GetConfigValue("NOTIFY_EMAIL_"+i.ToString());
                        if (Util.isValidEmailAddress(email)) {
                            if (e.sendTo("doNotReply@appointmaster.com", "LPI Admin", email, Util.GetConfigValue("NOTIFY_EMAIL_NAME_" + i.ToString()), subject, true, log.filename))
                                nbemail++;
                        }
                    }
                    /*
                    if (e.sendTo("doNotReply@appointmaster.com", "LPI Admin", "cgissinger@appointmaster.com", "Christophe", subject, true, log.filename))
                        nbemail++;
                    if (e.sendTo("doNotReply@appointmaster.com", "LPI Admin", "aaron.drew@localpatient.com", "Aaron", subject, true, log.filename))
                        nbemail++;
                    if (e.sendTo("doNotReply@appointmaster.com", "LPI Admin", "mnorton@mrcgroup.net", "Mike", subject, true, log.filename))
                        nbemail++;
                     */ 
                }
                Log("Nb emails Sent:" + nbemail.ToString());
            }
        }


        static DateTime start = DateTime.Now;
        static ArrayList matchedAccounts = new ArrayList();
        static int nbAccountScanned = 0;
        static int nbNewPatientFound = 0;
        static int nbNewTreatmentsFound = 0;

        static int nbNewCDRCampaign = 0;
        static int nbNewCDRLeads = 0;
        static double newTreatmentsValue = 0;

        static int nbNewMatchedPatientFound = 0;
        static int nbMatchedTreatmentsFound = 0;

        static Log log = null;


        static void Log(string message) {
            try {
                message = DateTime.Now.Hour.ToString().PadLeft(2) + ":" + DateTime.Now.Minute.ToString().PadLeft(2) + ":" + DateTime.Now.Second.ToString().PadLeft(2)+"  " + message;
                //Console.WriteLine(message);
                if (log!=null)
                    log.WriteLine(message);
            } catch (Exception ex) {
                Console.WriteLine("Log exception "+ex.Message);
            }
        }

        static void TestEmail(string[] args) {
            try{
                if (args.Length == 2) {
                    string email = args[1];
                    if (Util.isValidEmailAddress(email)) {
                        Email em = new Email(0);
                        em.content = "This is a test generated from PracticeRoi Server " + Util.GetConfigValue("server");
                        if (!em.sendTo(email, email, "Test email from PracticeRoi")) {
                            Console.WriteLine("Not Ok:" + em.error);
                        } else
                            Console.WriteLine("Sent Ok");
                    } else
                        throw new Exception("invalid email:" + email);
                } else
                    throw new Exception("Not enough parameter");
            } catch (Exception ex) {
                throw new Exception("TestEmail exception " + ex.Message);
            }
        }

        private static void UpdateAccountAmData() {
            try {
                ArrayList dbs = new ArrayList();
                int db = 0;
                using (SqlConnection cn = Connect.getConnection(getSQLServerName())) {
                    SqlCommand command = new SqlCommand("select Db from account where DataLinkUserId='' or DataLinkPassword=''", cn);
                    SqlDataReader DbReader = command.ExecuteReader();
                    while (DbReader.Read()) {
                        db = DbReader.GetInt32(0);
                        dbs.Add(db);
                    }
                    DbReader.Close();
                    DbReader.Dispose();


                    Console.WriteLine("Found " + dbs.Count.ToString() + " accounts not affected to AM. Checking association");
                    for (int i = 0; i < dbs.Count; i++) {
                        db = (int)dbs[i];
                        Console.WriteLine("----------------------- ACT #" + db.ToString());

                        PROI_WS.ROIServices rws = new WebServiceConsoleCall.PROI_WS.ROIServices();
                        PROI_WS.accountDataReply reply = rws.getPracticeSyncData("1"+dbs[i].ToString());
                        if (reply.error == "") {
                            try {
                                Account act = new Account(cn, db);
                                string updateStr = "DataLinkPassword='" + reply.dataLinkPassword + "'";
                                updateStr += ",DataLinkUserId='" + reply.dataLinkUserId + "'";
                                updateStr += ",AMServer ='" + reply.server + "'";
                                act.DBUpdate(updateStr);
                            } catch (Exception ex2) {
                                Console.WriteLine("Loop Exception:" + ex2.Message);
                            }
                        } else {
                            Console.WriteLine("Error returned from server:" + reply.error);
                            if (reply.error.IndexOf("Cannot find") >= 0) {
                                Console.WriteLine("Trying create Account");
                                Account act = new Account(cn, db);
                                createAppointMasterAccount(cn, act, "", "");
                                Console.WriteLine("Account created");
                            }
                        }
                    }
                    traceResults(true);
                    cn.Close();
                }
            } catch (Exception ex) {
                DisplayException("MatchLeadsForAllAccounts:" + ex.Message);
            }
        }

        static void Main(string[] args) {
            try{

                //if (DataBase.Util.GetConfigValue("HOST_SERVER").ToUpper().IndexOf("LOCALHOST") >= 0)test();
                if (args.Length > 0) {
                    if (args[0].ToLower() == "sync") {
                        syncAllAccounts(args);
                    } else if (args[0].ToLower() == "resync") {
                        reSyncAlls(args);
                    } else if (args[0].ToLower() == "cdr") {
                        readRecentsCDR(args);
                    } else if (args[0].ToLower() == "match") {
                        matchLeads(args);
                    } else if (args[0].ToLower() == "email") {
                        TestEmail(args);
                    } else if (args[0].ToLower() == "updateam") {
                        UpdateAccountAmData();
                    } else
                        doc();
                    
                } else {
                    doc();
                }
            }catch (Exception ex){
                throw new Exception("WebServiceConsoleCall exception:"+ex.ToString());
            }
        }
    }

   
}
//match 1151 reset
//CDR 4/1/2013
//CDR  9/1/2013  9/5/2013
//resync 50 100
//Match 1170 reset
//Match 1271 reset