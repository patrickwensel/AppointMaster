using AppointMaster.Resources;
using MvvmCross.Core.ViewModels;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppointMaster.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        public bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(() => UserName); }
        }

        public string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(() => Password); }
        }

        public MvxCommand LoginCommand
        {
            get
            {
                return new MvxCommand(() => Login());
            }
        }

        public LoginViewModel()
        {
            UserName = "c0a0f76d-261c-474a-8db9-d5f815e40958";
            Password = "K!ll3r";
        }

        private async void Login()
        {
            IsBusy = true;

            if (string.IsNullOrEmpty(UserName))
            {
                DisplayAlert(AppResources.Enter_Clinic_ID);
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                DisplayAlert(AppResources.Enter_Password);
                return;
            }

            ShowViewModel<MainViewModel>();
            return;

            try
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
                var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
                byte[] hash = hasher.HashData(inputBytes);
                string hashPass = Convert.ToBase64String(hash);

                string authorization = string.Format("{0}|{1}|{2}", "VetMobile", UserName, hashPass);
                string authorizationBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization));

                Services.DataHelper.GetInstance().SetAuthorization(authorizationBase64);

                string url = "http://ppgservices-001-site6.ctempurl.com/api/v1/VetClinic";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authorizationBase64);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ShowViewModel<MainViewModel>();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public void DisplayAlert(string message)
        {
            MessagingCenter.Send<LoginViewModel, string>(this, "DisplayAlert", message);
        }
    }
}
