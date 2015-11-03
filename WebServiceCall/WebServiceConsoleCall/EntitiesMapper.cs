using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace WebServiceConsoleCall
{
    public static class Mapper
    {

        
        public static string EncodePassword(string originalPassword) {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            // Conver the original password to bytes; then create the hash
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            // Bytes to string
            return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(encodedBytes), "-", "").ToLower();
        }

        static string GetMd5Hash(MD5 md5Hash, string input) {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        
        public static List<AccountCampaign> MapAccountCampaign(string[] StringArray)
        {
            List<AccountCampaign> EntityListObject = new List<AccountCampaign>(StringArray.Count());
            foreach (string strObject in StringArray)
            {
                AccountCampaign EntityObject = new AccountCampaign();
                foreach (string R in strObject.Split('|'))
                {
                    if (R.StartsWith("AccountId:", StringComparison.CurrentCulture))
                        EntityObject.AccountId = Convert.ToInt32(R.Substring("AccountId:".Length));

                    if (R.StartsWith("CampainName:", StringComparison.CurrentCulture))
                        EntityObject.CampainName = R.Substring("CampainName:".Length);

                    if (R.StartsWith("CampaingnCallNumber:", StringComparison.CurrentCulture))
                        EntityObject.CampaingnCallNumber = R.Substring("CampaingnCallNumber:".Length);

                    if (R.StartsWith("ForwardedNumber:", StringComparison.CurrentCulture))
                        EntityObject.ForwardedNumber = R.Substring("ForwardedNumber:".Length);

                    if (R.StartsWith("CampaignProvider:", StringComparison.CurrentCulture))
                        EntityObject.CampaignProvider = R.Substring("CampaignProvider:".Length);

                    if (R.StartsWith("CampaignStatus:", StringComparison.CurrentCulture))
                        EntityObject.CampaignStatus = Convert.ToInt32(R.Substring("CampaignStatus:".Length));

                    if (R.StartsWith("Type:", StringComparison.CurrentCulture))
                        EntityObject.Type = R.Substring("Type:".Length);
                }
                EntityListObject.Add(EntityObject);
            }
            return EntityListObject;
        }

        public static List<LPAccount> MapLPAccount(string[] StringArray)
        {
            List<LPAccount> EntityListObject = new List<LPAccount>(StringArray.Count());
            foreach (string strObject in StringArray)
            {
                LPAccount EntityObject = new LPAccount();
                foreach (string R in strObject.Split('|'))
                {
                    if (R.StartsWith("AccountId:", StringComparison.CurrentCulture))
                        EntityObject.AccountId = Convert.ToInt32(R.Substring("AccountId:".Length));

                    if (R.StartsWith("FirstName:", StringComparison.CurrentCulture))
                        EntityObject.FirstName = R.Substring("FirstName:".Length);

                    if (R.StartsWith("LastName:", StringComparison.CurrentCulture))
                        EntityObject.LastName = R.Substring("LastName:".Length);

                    if (R.StartsWith("CompanyName:", StringComparison.CurrentCulture))
                        EntityObject.CompanyName = R.Substring("CompanyName:".Length);

                    if (R.StartsWith("Email:", StringComparison.CurrentCulture))
                        EntityObject.Email = R.Substring("Email:".Length);

                    if (R.StartsWith("Address:", StringComparison.CurrentCulture))
                        EntityObject.Address = R.Substring("Address:".Length);

                    if (R.StartsWith("City:", StringComparison.CurrentCulture))
                        EntityObject.City = R.Substring("City:".Length);

                    if (R.StartsWith("State:", StringComparison.CurrentCulture))
                        EntityObject.State = R.Substring("State:".Length);

                    if (R.StartsWith("Zip:", StringComparison.CurrentCulture))
                        EntityObject.Zip = R.Substring("Zip:".Length);

                    if (R.StartsWith("Phone:", StringComparison.CurrentCulture))
                        EntityObject.Phone = R.Substring("Phone:".Length);

                    if (R.StartsWith("LoginUsername:", StringComparison.CurrentCulture))
                        EntityObject.LoginUsername = R.Substring("LoginUsername:".Length);

                    if (R.StartsWith("LoginPassword:", StringComparison.CurrentCulture)){
                        EntityObject.LoginPassword = R.Substring("LoginPassword:".Length);
                    }

                    if (R.StartsWith("Type:", StringComparison.CurrentCulture))
                        EntityObject.Type = R.Substring("Type:".Length);
                }
                EntityListObject.Add(EntityObject);
            }
            return EntityListObject;
        }

        public static List<LPCDR> MapLPCDR(string[] StringArray)
        {
            List<LPCDR> EntityListObject = new List<LPCDR>(StringArray.Count());
            LPCDR lpcdr;
            foreach (string strObject in StringArray)
            {
                lpcdr = new LPCDR();
                foreach (string R in strObject.Split('|'))
                {
                    string[] elts = R.Split(':');
                    Tracer.AddWord(elts[0]);

                    if (R.StartsWith("CallID:", StringComparison.CurrentCulture))
                        lpcdr.CallID = Convert.ToInt32(R.Substring("CallID:".Length));

                    else if (R.StartsWith("CallingNumber:", StringComparison.CurrentCulture))
                        lpcdr.CallingNumber = R.Substring("CallingNumber:".Length);

                    else if (R.StartsWith("CalledNumber:", StringComparison.CurrentCulture))
                        lpcdr.CalledNumber = R.Substring("CalledNumber:".Length);

                    else if (R.StartsWith("CampaignNumber:", StringComparison.CurrentCulture))
                        lpcdr.CampaignNumber = R.Substring("CampaignNumber:".Length);

                    else if (R.StartsWith("TimestampOfCall:", StringComparison.CurrentCulture))
                        lpcdr.TimestampOfCall = R.Substring("TimestampOfCall:".Length);

                    else if (R.StartsWith("Duration:", StringComparison.CurrentCulture))
                        lpcdr.Duration = Convert.ToInt32(R.Substring("Duration:".Length));

                    else if (R.StartsWith("Charge:", StringComparison.CurrentCulture))
                        lpcdr.Charge = Convert.ToSingle(R.Substring("Charge:".Length));

                    else if (R.StartsWith("IVRFile:", StringComparison.CurrentCulture))
                        lpcdr.IVRFile = R.Substring("IVRFile:".Length);

                    else if (R.StartsWith("DOB:", StringComparison.CurrentCulture))
                        lpcdr.DOB = R.Substring("DOB:".Length);

                    else if (R.StartsWith("PrimaryPhone:", StringComparison.CurrentCulture))
                        lpcdr.PrimaryPhone = R.Substring("PrimaryPhone:".Length);

                    else if (R.StartsWith("AccountId:", StringComparison.CurrentCulture))
                        lpcdr.AccountId = Convert.ToInt32(R.Substring("AccountId:".Length));

                    else if (R.StartsWith("Source:", StringComparison.CurrentCulture))
                        lpcdr.Source = R.Substring("Source:".Length);

                    else if (R.StartsWith("Keyword:", StringComparison.CurrentCulture))
                        lpcdr.Keyword = R.Substring("Keyword:".Length);

                    else if (R.StartsWith("IsSourceCampaign:", StringComparison.CurrentCulture)) {
                        lpcdr.IsSourceCampaign = (R.Substring("IsSourceCampaign:".Length) == "1");
                        if (!lpcdr.IsSourceCampaign) {
                            lpcdr.IsSourceCampaign = lpcdr.IsSourceCampaign;
                        }
                    }
                }
                EntityListObject.Add(lpcdr);
            }

            return EntityListObject;
        }
    }

}
