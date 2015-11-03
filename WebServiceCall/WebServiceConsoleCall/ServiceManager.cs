using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServiceConsoleCall.webReference;
using System.Security.Cryptography;

namespace WebServiceConsoleCall
{
    public class ServiceManager
    {
        private AvaVoip _AvaVoip;
        private Auth _Auth;
        private Response _Response;

        public ServiceManager()
        {
            _AvaVoip = new AvaVoip();
            _Auth = new Auth() { Username = "appointmaster", Password = this.CalculateMD5Hash("hf%#9hjhf&@") };
            _Response = new Response();

                string s="9ebfab01c16f43bc5089faf87639a150";
                string test = this.CalculateMD5Hash("sampson");



                s = "2af87d6045d1ac024148a0bf28c2d204";
                test = this.CalculateMD5Hash("isacxh2j");
        }

        public ServiceManager(string UserName, string Password): this()
        {
            _Auth.Username = UserName;
            _Auth.Password = this.CalculateMD5Hash(Password);
        }

        private string CalculateMD5Hash(string ValueToEncrypt)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(ValueToEncrypt);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2")); //replace x2 with X2 for upper case and v.v.
            }
            return sb.ToString();
        }

        public List<LPCDR> GetCDRs(CriteriaLPCDR CriteriaObject)
        {
            try 
	        {	        
                _Response = _AvaVoip.Appointmaster_GetCDRs(CriteriaObject, _Auth);

                if (!String.IsNullOrEmpty(_Response.ErrorMessage))
                    throw new Exception(_Response.ErrorMessage);
                else
                    return Mapper.MapLPCDR(_Response.List);
	        }
	        catch (Exception ex)
	        {
                throw new Exception("ServiceManager.GetCDRs exception:"+ex.Message);
	        }
        }

        public List<AccountCampaign> GetAccountCampaigns(CriteriaAccountsCampaigns CriteriaObject)
        { 
            try 
	        {	        
                _Response = _AvaVoip.Appointmaster_GetAccountCampaigns(CriteriaObject, _Auth);

                if (!String.IsNullOrEmpty(_Response.ErrorMessage))
                    throw new Exception(_Response.ErrorMessage);
                else
                    return Mapper.MapAccountCampaign(_Response.List); 
	        }
	        catch (Exception ex)
	        {
                throw new Exception("ServiceManager.GetAccountCampaigns exception:" + ex.Message);
	        }

        }

        public List<AccountCampaign> GetAccountsCampaignsByTransID(CriteriaAccountsCampaignsByTransID CriteriaObject)
        { 
            try 
	        {
                _Response = _AvaVoip.Appointmaster_GetAccountsCampaignsByTransID(CriteriaObject, _Auth);

                if (!String.IsNullOrEmpty(_Response.ErrorMessage))
                    throw new Exception(_Response.ErrorMessage);
                else
                    return Mapper.MapAccountCampaign(_Response.List); 
	        }
	        catch (Exception ex)
	        {
                throw new Exception("ServiceManager.GetAccountsCampaignsByTransID exception:" + ex.Message);
	        }
        }

        public List<LPAccount> GetLatestAccounts(CriteriaLatestAccountsList CriteriaObject)
        {
            try
            {
                _Response = _AvaVoip.Appointmaster_GetLatestAccounts(CriteriaObject, _Auth);

                if (!String.IsNullOrEmpty(_Response.ErrorMessage))
                    throw new Exception(_Response.ErrorMessage);
                else
                    return Mapper.MapLPAccount(_Response.List);
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceManager.GetLatestAccounts exception:" + ex.Message);
            }
        }

        public List<LPAccount> GetAccountsByTransID(CriteriaAccountsByTransID CriteriaObject)
        {
            try
            {
                _Response = _AvaVoip.Appointmaster_GetAccountsByTransID(CriteriaObject, _Auth);

                if (!String.IsNullOrEmpty(_Response.ErrorMessage))
                    throw new Exception(_Response.ErrorMessage);
                else
                    return Mapper.MapLPAccount(_Response.List); 
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceManager.GetAccountsByTransID exception:" + ex.Message);
            }
        }

    }

}
