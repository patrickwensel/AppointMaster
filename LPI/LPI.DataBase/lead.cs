using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    
    public class Lead {

        public enum OriginalStatus {
            Original,                       //0
            NotValid,                       //1
            FirstAndLastNameNotUnique,      //2
            InComingNumberNotUnique,        //3    
            EmailAddressNotUnique,          //4
            DuplicateMatching,              //5
            PatientIdAlreadyExists,         //6
            PatientIsNotNew,                //7
            PrimaryPhoneNumberNotUnique,    //8
            Undef   =1000
        }


        public enum MatchingStatus{
            NoNatch,            //0
            NormalMatch,        //1
            AlternativeMatch    //2

        }
        
        public enum DentalNeed {
            CheckupAndCleaning,          //0
            UnexplainedPain,             //1
            FindaDentistforaChild,       //2
            Braces,                      //3 
            BrokenTooth,                 //4
            Cavity,                      //5
            CrownorCap,                  //6
            DentalImplants,              //7
            Dentures,                    //8
            GumDisease,                  //9 
            RootCanal,                   //10
            TeethWhitening,              //11
            ToothExtraction,             //12
            Toothache,                   //13
            WisdomTeeth,                 //14
            Invisalign,                  //15
            OralAppliance,               //16
            Retainer,                    //17
            EarlyDetection,              //18
            TMJ,                         //19
            FullMouthReconstruction,     //20
            CosmeticDentistry,           //21
            SleepApnea,                  //22 --> old OTHER
            VeneersCosmetic,             //23
            DarkGums,                    //24
            Other                        //25 was 22 10/13/2014
        }                                

        public enum DentalCareIsFor {
            Me,
            MyChild,
            MySpousePartner,
            MyParent,
            Other,
        }

        public enum NewPatient {
            NewPatient,
            ReturningPatient
        }

        public enum PreferredAppointmentTime {
            WhateverGetsMeInFastest,
            Morning,
            Afternoon,
            ContactMeToArrange,
        }

        public enum InsurancePlanBudget {
            ContactMeToArrange,
            SelfPay,
            AnyDentist,
            HMO,
            PPO,
            StateAid,
            NotSure,
            EmployerProviderPlan,
            DiscountPlan,
            OtherPlan,
            StatePlanMedicareMedicaid
        }

        public int ID;
        public int DB;
        public string campaignID;
        public string inComingNumber;
        public string PrimaryPhone;
        public DateTime timeStamp;
        public int durationInMinutes;
        
        public bool newPatient;
        public DateTime birthDate;
        public string patientID;
        public string fileUrl;
        private SqlConnection cn;

        //Section for Form generated lead
        //The phone number is stored as "incoming number"

        public DentalNeed dentalNeed;
        public NewPatient dentalCareIsFor;
        public PreferredAppointmentTime preferredAppointmentTime;
        public InsurancePlanBudget insurancePlanBudget;
        public string firstName;
        public string lastName;
        public string email;

        public string source;
        public string medium;
        public string term;
        public string content;
        public string campaign;
        public string referred_by;
        public string alterPrimaryPhone;
        public string alterName;
        public string alterFirstName;
        public OriginalStatus original;
        public MatchingStatus matchingStatus;


        public string EnumToString(string basic) {
            string s = "";
            for (int i = 0; i < basic.Length; i++) {
                char c = basic[i];
                if (basic[i].ToString().ToUpper() == basic[i].ToString()) {
                    if (i > 0)
                        s += " ";
                }
                s += basic[i];
            }
            return s;
        }


        public string DentalNeedToString() {
            return EnumToString(this.dentalNeed.ToString());
        }

        public string DentalCareIsForToString() {
            return EnumToString(this.dentalCareIsFor.ToString());
        }

        public string PreferredAppointmentTimeToString() {
            return EnumToString(this.preferredAppointmentTime.ToString());
        }
        public string InsurancePlanBudgetToString() {
            return EnumToString(this.insurancePlanBudget.ToString());
        }
        


        public static bool exist(SqlConnection cn, int DB, int campaignId, string incomingNumber, DateTime atTime) {
            try {
                string WC = "select DB from lead where DB=" + DB.ToString();
                WC += " and campaignId=" + campaignId.ToString();
                WC += " and ='" + incomingNumber+"'";
                WC += " and year(timeStamp)=" + atTime.Year.ToString();
                WC += " and month(timeStamp)=" + atTime.Year.ToString();
                WC += " and day(timeStamp)=" + atTime.Year.ToString();
                WC += " and datepart(hour,timeStamp )=" + atTime.Hour.ToString();
                
                SqlCommand command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                SafeDbReader DbReader = new SafeDbReader( command.ExecuteReader());
                bool ActExist = DbReader.Read();
                DbReader.Close();
                DbReader.Dispose();
                return ActExist;
            } catch (Exception ex) {
                throw new Exception("Lead.exist exception:" + ex.Message);
            }
        }

        public string getAttributes() {
            //      0  1  2          3              4          5               6          7         8       9        10          11               12                        13                   14         15        16     17            18      19      20    21       22        23           24        25                 26         27              28
            return "ID,DB,campaignID,inComingNumber,timeStamp, durationMinutes,newPatient,patientId,fileURL,birthday,dentalNeed, dentalCareIsFor, preferredAppointmentTime, insurancePlanBudget, firstName, lastName, email, PrimaryPhone, source, medium, term, content, campaign, referred_by, original, alterPrimaryPhone, alterName, alterFirstName, matchingStatus";
        }
        public void checkForNullString() {
            checkNullStr(ref this.patientID);
            checkNullStr(ref this.campaignID);
            checkNullStr(ref this.inComingNumber);
            checkNullStr(ref this.PrimaryPhone);
            checkNullStr(ref this.lastName);
            checkNullStr(ref this.firstName);
            checkNullStr(ref this.email);
            checkNullStr(ref this.fileUrl);
            
            checkNullStr(ref this.source);
            checkNullStr(ref this.medium);
            checkNullStr(ref this.term);
            checkNullStr(ref this.content);
            checkNullStr(ref this.campaign);
            checkNullStr(ref this.referred_by);
            checkNullStr(ref this.alterPrimaryPhone);
            checkNullStr(ref this.alterName);
            checkNullStr(ref this.alterFirstName);

        }

        public void fillFromReader(SafeDbReader DbReader) {
            try {
                this.ID = DbReader.GetInt32(0);
                this.DB = DbReader.GetInt16(1);
                this.campaignID = DbReader.GetInt32(2).ToString().Trim();
                this.inComingNumber = DbReader.GetString(3).Trim();
                this.timeStamp = DbReader.GetDateTime(4);
                this.durationInMinutes = DbReader.GetInt16(5);
                this.newPatient = (DbReader.GetInt16(6) == 1);
                this.patientID = DbReader.GetString(7).Trim();
                this.fileUrl = DbReader.GetString(8).Trim();
                try {
                    this.birthDate = DbReader.GetDateTime(9);
                } catch {
                    this.birthDate = new DateTime(1900, 1, 1);
                }
                this.dentalNeed = (DentalNeed )DbReader.GetInt16(10);
                try {
                    this.dentalCareIsFor = (NewPatient)DbReader.GetInt16(11);
                } catch {
                    this.dentalCareIsFor = NewPatient.NewPatient;
                }
                this.preferredAppointmentTime = (PreferredAppointmentTime )DbReader.GetInt16(12);
                this.insurancePlanBudget = (InsurancePlanBudget)DbReader.GetInt16(13);
                this.firstName = DbReader.GetString(14).Trim();
                this.lastName = DbReader.GetString(15).Trim();
                this.email = DbReader.GetString(16).Trim();
                this.PrimaryPhone = DbReader.GetString(17).Trim();
                this.source = DbReader.GetString(18).Trim();
                this.medium = DbReader.GetString(19).Trim();
                this.term = DbReader.GetString(20).Trim();
                this.content = DbReader.GetString(21).Trim();
                this.campaign = DbReader.GetString(22).Trim();
                this.referred_by = DbReader.GetString(23).Trim();
                
                if (DbReader.reader.FieldCount > 24) {
                    this.original = (OriginalStatus)DbReader.GetByte(24);
                    if (DbReader.reader.FieldCount > 25) {
                        this.alterPrimaryPhone = DbReader.GetString(25).Trim();
                        if (DbReader.reader.FieldCount > 26) {
                            this.alterName = DbReader.GetString(26).Trim();
                            this.alterFirstName = DbReader.GetString(27).Trim();

                            if (!DbReader.IsDBNull(28))
                                this.matchingStatus = (MatchingStatus)DbReader.GetInt16(28);
                            else
                                this.matchingStatus = MatchingStatus.NoNatch;
                        }
                    }
                }

                checkForNullString();
            } catch (Exception ex) {
                throw new Exception("fillFromReader ex:"+ex.Message);
            }
        }

        public bool DBGet(int DB, int campaignId, string incomingNumber, DateTime atTime, bool withHours) {
            try {
                incomingNumber = Util.phoneDigits(incomingNumber);
                string WC = "select " + getAttributes() + " from lead where DB=" + DB.ToString();
                WC += " and campaignId='" + campaignId.ToString() + "'";
                WC += " and inComingNumber='" + incomingNumber + "'";
                WC += " and year(timeStamp)=" + atTime.Year.ToString();
                WC += " and month(timeStamp)=" + atTime.Month.ToString();
                WC += " and day(timeStamp)=" + atTime.Day.ToString();
                if (withHours)
                    WC += " and datepart(hour,timeStamp )=" + atTime.Hour.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                if (DbReader.Read()) {
                    fillFromReader(DbReader);

                    DbReader.Close();
                    DbReader.Dispose();
                    command.Dispose();
                    return true;
                }
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }

        public bool DBGet(int ID) {
            try {
                string WC = "select " + getAttributes() + " from lead where ID=" + ID.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                if (DbReader.Read()) {
                    fillFromReader(DbReader);

                    DbReader.Close();
                    DbReader.Dispose();
                    command.Dispose();
                    return true;
                }
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }


        public bool DBGetFromBean(int ID) {
            try {
                string WC = "select " + getAttributes() + " from leadbean where ID=" + ID.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                if (DbReader.Read()) {
                    fillFromReader(DbReader);

                    DbReader.Close();
                    DbReader.Dispose();
                    command.Dispose();
                    return true;
                }
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }



        public bool DBSearch(int DB, string whereClause) {
            try {
                string WC = "select " + getAttributes() + " from lead where DB=" + DB.ToString();
                WC += " and " + whereClause;
                SqlCommand command = new SqlCommand(WC, cn);
                SafeDbReader DbReader = new SafeDbReader(command.ExecuteReader());
                if (DbReader.Read()) {
                    fillFromReader(DbReader);

                    DbReader.Close();
                    DbReader.Dispose();
                    command.Dispose();
                    return true;
                }
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();
                return false;
            } catch (Exception ex) {
                return false;
            }
        }



        public bool DBDelete(int DB, int campaignId, string incomingNumber, DateTime atTime, bool withHours) {
            try {
                string WC = "delete from lead where DB=" + DB.ToString();
                WC += " and campaignId='" + campaignId.ToString() + "'";
                WC += " and inComingNumber='" + incomingNumber + "'";
                WC += " and year(timeStamp)=" + atTime.Year.ToString();
                WC += " and month(timeStamp)=" + atTime.Month.ToString();
                WC += " and day(timeStamp)=" + atTime.Day.ToString();
                if (withHours)
                    WC += " and datepart(hour,timeStamp )=" + atTime.Hour.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                return (nbRow>0);
            } catch (Exception ex) {
                return false;
            }
        }

        public bool DBDelete(int ID) {
            try {
                string WC = "delete from lead where ID=" + ID.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                return (nbRow > 0);
            } catch (Exception ex) {
                return false;
            }
        }

        private void checkNullStr(ref string s) {
            if (s == null)
                s = "";
        }




        private string getValues() {
            checkForNullString();

            string s = this.ID.ToString() + ",";
            s += DB.ToString() + ",";
            s += "'" + this.campaignID + "',";
            s += "'" + inComingNumber + "'" + ",";
            s += "'" + timeStamp.ToString() + "'" + ",";
            s += this.durationInMinutes.ToString() + ",";
            if (newPatient)
                s += "1,";
            else
                s += "0,";

            s += "'" + patientID.Replace("'", " ") + "'" + ",";
            s += "'" + fileUrl.Replace("'", " ") + "'" + ",";
            s += "'" + birthDate.ToShortDateString() + "'" + ",";


            s += ((int)dentalNeed).ToString() + ",";
            s += ((int)dentalCareIsFor).ToString() + ",";
            s += ((int)preferredAppointmentTime).ToString() + ",";
            s += ((int)insurancePlanBudget).ToString() + ",";
            s += "'" + firstName.Replace("'", " ") + "'" + ",";
            s += "'" + lastName.Replace("'", " ") + "'" + ",";
            s += "'" + email.Replace("'", " ") + "'" + ",";
            s += "'" + this.PrimaryPhone.Replace("'", " ") + "',";

            s += "'" + this.source.Replace("'", " ") + "',";
            s += "'" + this.medium.Replace("'", " ") + "',";
            s += "'" + this.term.Replace("'", " ") + "',";
            s += "'" + this.content.Replace("'", " ") + "',";
            s += "'" + this.campaign.Replace("'", " ") + "',";
            s += "'" + this.referred_by.Replace("'", " ") + "',";
            s += ((int)this.original).ToString() + ",";
            s += "'" + this.alterPrimaryPhone.Replace("'", " ") + "',";
            s += "'" + this.alterName.Replace("'", " ") + "',";
            s += "'" + this.alterFirstName.Replace("'", " ") + "',";
            s += ((int)this.matchingStatus).ToString();
            return s;
        }

        public void DBCreate() {
            DBCreate(true);
        }

        public void DBCreate(bool AttributeNewId) {
            try {
                if (this.birthDate == DateTime.MinValue)
                    this.birthDate = new DateTime(1900, 1, 1);

                string WC = "";
                SqlCommand command = null;
                if (AttributeNewId) {
                    int newId = 0;
                    WC = "select dbo.NewLeadId()";
                    command = new SqlCommand(WC, cn);
                    SqlDataReader r = command.ExecuteReader();
                    if (r.Read()) {
                        newId = r.GetInt32(0);
                    } else {
                        newId = 1;
                    }
                    r.Close();
                    r.Dispose();
                    r = null;
                    command.Dispose();
                    command = null;


                    WC = "update unique_id set last_id=@NEWID where table_name='LEAD'".Replace("@NEWID", newId.ToString());
                    command = new SqlCommand(WC, cn);
                    int nbRows = command.ExecuteNonQuery();

                    command.Dispose();
                    command = null;

                    this.ID = newId;
                }

                WC = "insert into lead (" + getAttributes() + ") values (" + getValues() + ")";
                command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                command.Dispose();
                command = null;

                
            } catch (Exception ex) {
                throw new Exception("Campaign .DBcreate exception:" + ex.Message);
            }
        }

        public void DBUpdatePatientId(string Id) {
            try {
                /*string WC = "update lead set  patientId='" + Id + "' where ";
                WC += "DB=" + this.DB.ToString();
                WC += " and campaignId=" + this.campaignID.ToString();
                WC += " and inComingNumber='" + this.inComingNumber + "'";
                WC += " and year(timeStamp)=" + this.timeStamp.Year.ToString();
                WC += " and month(timeStamp)=" + this.timeStamp.Month.ToString();
                WC += " and day(timeStamp)=" + this.timeStamp.Day.ToString();
                WC += " and datepart(hour,timeStamp )=" + this.timeStamp.Hour.ToString();
                WC += " and datepart(minute,timeStamp )=" + this.timeStamp.Minute.ToString();
                WC += " and datepart(second,timeStamp )=" + this.timeStamp.Second.ToString();
                WC += " and firstname='" + this.firstName + "'";
                WC += " and lastName='" + this.lastName + "'";
                WC += " and source='" + this.source + "'";
                WC += " and PrimaryPhone='" + this.PrimaryPhone + "'";
                WC += " and referred_by='" + this.referred_by + "'";
                WC += " and term='" + this.term + "'";
                WC += " and content='" + this.content + "'";
                WC += " and medium='" + this.medium + "'";
                WC += " and email='" + this.email + "'";
                WC += " and durationMinutes=" + this.durationInMinutes.ToString();
                if (newPatient)
                    WC += " and newPatient=1" ;
                else
                    WC += " and newPatient=0";
                WC += " and fileURL='" + this.fileUrl + "'";
                WC += " and year(birthDay)=" + this.birthDate.Year.ToString();
                WC += " and month(birthDay)=" + this.birthDate.Month.ToString();
                WC += " and day(birthDay)=" + this.birthDate.Day.ToString();
                WC += " and dentalNeed=" + ((int)this.dentalNeed).ToString();
                WC += " and dentalCareIsFor=" + ((int)this.dentalCareIsFor).ToString();
                WC += " and preferredAppointmentTime=" + ((int)this.preferredAppointmentTime).ToString();
                WC += " and insurancePlanBudget=" + ((int)this.insurancePlanBudget).ToString();
                WC += " and campaign='" + this.campaign + "'";
                WC += " and referred_by='" + this.referred_by + "'";
                */

                string WC = "update lead set  patientId='" + Id + "' where ID=" + this.ID.ToString();
                
                
                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                if (nbRow != 1) {
                    bool debug = true;
                }
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("Campaign .DBcreate exception:" + ex.Message);
            }
        }

        public void DBUpdateOriginalStatus(Lead.OriginalStatus status) {
            try {
                string WC = "update lead set  original=" + ((int)status).ToString() + " where ID=" + this.ID.ToString();

                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                if (nbRow != 1) {
                    bool debug = true;
                }
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("Campaign.DBUpdateOriginalStatus exception:" + ex.Message);
            }
        }


        public void DBUpdateMatchingStatus(Lead.MatchingStatus status) {
            try {
                string WC = "update lead set  MatchingStatus=" + ((int)status).ToString() + " where ID=" + this.ID.ToString();

                SqlCommand command = new SqlCommand(WC, cn);
                int nbRow = command.ExecuteNonQuery();
                if (nbRow != 1) {
                    bool debug = true;
                }
                command.Dispose();
                command = null;
            } catch (Exception ex) {
                throw new Exception("Campaign.DBUpdateMatchingStatus exception:" + ex.Message);
            }
        }


        public void Clear() {
            this.DB = 0;
            this.campaignID = "";
            this.inComingNumber = "";
            this.PrimaryPhone = "";
            this.timeStamp = DateTime.MinValue;
            this.durationInMinutes = 0;
            this.newPatient = true;
            this.birthDate = DateTime.MinValue;
            this.patientID = "";
            fileUrl = "";
            this.source = "";
            this.medium = "";
            this.term = "";
            this.content = "";
            this.campaign = "";
            this.referred_by = "";
            this.alterPrimaryPhone = "";
            this.alterName = "";
            this.alterFirstName = "";
            this.ID = 0;
            this.email = "";
        }


        /// <summary>
        /// Compare 'lead' to the current instance
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        public bool IsLeadDifferent(Lead lead) {
            return ((lead.dentalCareIsFor == this.dentalCareIsFor) &
            (lead.dentalNeed == this.dentalNeed) &
            (lead.preferredAppointmentTime == this.preferredAppointmentTime) &
            (lead.insurancePlanBudget == this.insurancePlanBudget) &
            (lead.firstName == this.firstName) &
            (lead.lastName == this.lastName) &
            (lead.email == this.email) &
            (lead.inComingNumber == this.inComingNumber));
        }

        /// <summary>
        /// Indicating if the lead is Valid, meaning can be taken into consideration.
        /// </summary>
        /// <returns></returns>
        public bool IsValid() {
            try {
                if (!string.IsNullOrEmpty(this.fileUrl)) {
                    if ((this.fileUrl.ToLower().StartsWith("http://")) | (this.fileUrl.ToLower().StartsWith("https://")))
                        if (this.durationInMinutes < 30)
                            return false;
                    if ((!Util.isValidPhoneNumber(this.inComingNumber)) & (!Util.isValidPhoneNumber(this.PrimaryPhone)))
                        return false;
                }
                return true;
            } catch (Exception ex) {
                throw new Exception("Lead.IsValid ex:"+ex.Message);
            }
        }


        private bool IsValidNameFirstName(string nameOrFirstName) {
            if (!string.IsNullOrEmpty(nameOrFirstName)) {
                return (nameOrFirstName.ToLower().Trim() != "none");
            }
            return false;
        }

        public OriginalStatus IsOriginal() {
            return IsOriginal(null);
        }

        
        /// <summary>
        /// Checks if the lead is ORIGINAL: it must have a unic Valid differentiator : names, phone, or email 
        /// </summary>
        /// <returns></returns>
        public OriginalStatus IsOriginal(SqlTransaction trans) {
            string WC = "";
            try {
                if (!IsValid())
                    return OriginalStatus.NotValid;

                SqlCommand cmd;
                ///*** 1  Check do not share valid first & last name
                if ((IsValidNameFirstName(this.firstName)) & (IsValidNameFirstName(this.lastName))) {
                    WC = "select count(*) from lead where rtrim(firstname)='" + this.firstName.Replace("'", "''") + "' and rtrim(lastName)='" + this.lastName.Replace("'", "''") + "' and DB="+this.DB.ToString();
                    //WC += " and timestamp<='" + this.timeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    if (trans == null)
                        cmd = new SqlCommand(WC, this.cn);
                    else
                        cmd = new SqlCommand(WC, this.cn, trans);
                    cmd.CommandTimeout = 300;
                    int count = (int)cmd.ExecuteScalar();
                    cmd.Cancel();
                    cmd.Dispose();
                    if (count > 0)
                        return OriginalStatus.FirstAndLastNameNotUnique; 
                }

                ///*** 2  Check do not share valid inComingNumber
                if (!string.IsNullOrEmpty(this.inComingNumber)) {
                    if (Util.isValidPhoneNumber(this.inComingNumber)) {
                        WC = "select count(*) from lead where inComingNumber='" + this.inComingNumber + "' and DB=" + this.DB.ToString() + " and Original=0";
                        //WC += " and timestamp<='" + this.timeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        if (trans == null)
                            cmd = new SqlCommand(WC, this.cn);
                        else
                            cmd = new SqlCommand(WC, this.cn, trans);
                        cmd.CommandTimeout = 300;
                        int count = (int)cmd.ExecuteScalar();
                        cmd.Cancel();
                        cmd.Dispose();
                        if (count > 0)
                            return OriginalStatus.InComingNumberNotUnique;
                    }
                }


                ///*** 3  Check do not share valid inComingNumber
                if (!string.IsNullOrEmpty(this.PrimaryPhone)) {
                    if (Util.isValidPhoneNumber(this.PrimaryPhone)) {
                        WC = "select count(*) from lead where PrimaryPhone='" + this.PrimaryPhone + "' and DB=" + this.DB.ToString() + " and Original=0";
                        //WC += " and timestamp<='" + this.timeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        if (trans == null)
                            cmd = new SqlCommand(WC, this.cn);
                        else
                            cmd = new SqlCommand(WC, this.cn, trans);
                        cmd.CommandTimeout = 300;
                        int count = (int)cmd.ExecuteScalar();
                        cmd.Cancel();
                        cmd.Dispose();
                        if (count > 0)
                            return OriginalStatus.PrimaryPhoneNumberNotUnique;
                    }
                }



                ///*** 4  Check do not share valid email
                if (!string.IsNullOrEmpty(this.email)) {
                    if (Util.isValidEmailAddress(this.email)) {
                        WC = "select count(*) from lead where email='" + this.email + "' and DB=" + this.DB.ToString();
                        //WC += " and timestamp<='" + this.timeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        if (trans == null)
                            cmd = new SqlCommand(WC, this.cn);
                        else
                            cmd = new SqlCommand(WC, this.cn, trans);
                        cmd.CommandTimeout = 300;
                        int count = (int)cmd.ExecuteScalar();
                        cmd.Cancel();
                        cmd.Dispose();
                        if (count > 0)
                            return OriginalStatus.EmailAddressNotUnique;
                    }
                }

                ///**** 5 We check if patientID possibly found is unic
                if (!string.IsNullOrEmpty(this.patientID)) {
                    if ((this.patientID != "0") & (this.patientID != "")) {
                        WC = "select count(*) from lead where patientID='" + this.patientID + "' and db=" + this.DB.ToString();
                        //WC += " and timestamp<='" + this.timeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        if (trans == null)
                            cmd = new SqlCommand(WC, this.cn);
                        else
                            cmd = new SqlCommand(WC, this.cn, trans);
                        cmd.CommandTimeout = 300;
                        int count = (int)cmd.ExecuteScalar();
                        cmd.Cancel();
                        cmd.Dispose();
                        if (count > 0) {
                            if (count == 1) {
                                Lead possibleDuplicate = new Lead(this.cn);
                                if (possibleDuplicate.DBSearch(this.DB, "patientID='" + this.patientID + "' and db=" + this.DB.ToString())) {
                                    if ((possibleDuplicate.timeStamp == this.timeStamp) &
                                        (possibleDuplicate.campaignID == this.campaignID) &
                                        (possibleDuplicate.durationInMinutes == this.durationInMinutes) &
                                        (possibleDuplicate.inComingNumber == this.inComingNumber) &
                                        (possibleDuplicate.patientID == this.patientID) &
                                        (possibleDuplicate.PrimaryPhone == this.PrimaryPhone) &
                                        (possibleDuplicate.source == this.source)) {
                                        //It's truly the same
                                    } else {
                                        return OriginalStatus.PatientIdAlreadyExists;
                                    }
                                }
                            }else
                                //>1
                                return OriginalStatus.PatientIdAlreadyExists;
                        }
                    }
                }

                return OriginalStatus.Original;
            } catch (Exception ex) {
                throw new Exception("Lead.IsOriginal ex:" + ex.Message + " WC=" + WC);
            }
        }

        /// <summary>
        /// Indicates if at least one lead has been found in the database with this patientID
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public bool FoundLeadForPatientID(string PatientId) {
            if (PatientId == "22037") {
                bool debug = true;
            }
            string WC = "select count(*) from lead where db=" + this.DB.ToString() + " and patientId='" + PatientId + "'";
            try{
                SqlCommand cmd = new SqlCommand(WC, this.cn);
                cmd.CommandTimeout = 300;
                int count = (int)cmd.ExecuteScalar();
                cmd.Cancel();
                cmd.Dispose();

                if (count > 0) {
                    if (count == 1) {
                        Lead possibleDuplicate = new Lead(this.cn);
                        if (possibleDuplicate.DBSearch(this.DB, "patientID='" + this.patientID + "' and db=" + this.DB.ToString())) {
                            if ((possibleDuplicate.timeStamp == this.timeStamp) &
                                (possibleDuplicate.campaignID == this.campaignID) &
                                (possibleDuplicate.durationInMinutes == this.durationInMinutes) &
                                (possibleDuplicate.inComingNumber == this.inComingNumber) &
                                (possibleDuplicate.patientID == this.patientID) &
                                (possibleDuplicate.PrimaryPhone == this.PrimaryPhone) &
                                (possibleDuplicate.source == this.source)) {
                                return false;
                            } else {
                                return true;
                            }

                        }
                    } else
                        //>1
                        return true;
                }



                return false;
            } catch (Exception ex) {
                throw new Exception("Lead.FoundLeadForPatientID ex:" + ex.Message + " WC=" + WC);
            }
        }



        public Lead(SqlConnection cn) {
            this.Clear();
            this.cn = cn;
        }

    }
}
