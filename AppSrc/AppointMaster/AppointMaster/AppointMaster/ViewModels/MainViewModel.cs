using AppointMaster.Resources;
using MvvmCross.Core.ViewModels;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppointMaster.ViewModels
{
    public class MainViewModel:MvxViewModel
    {
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(() => Password); }
        }

        public MainViewModel()
        {
            //GetClinicInfo();
        }

        public MvxCommand ShowCheckInCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CheckInViewModel>());
            }
        }

        public MvxCommand ShowSettingsCommand
        {
            get
            {
                return new MvxCommand(() => ShowSettingsPage());
            }
        }

        public MvxCommand ShowLoginCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<LoginViewModel>());
            }
        }

        private async void ValidatedPassword()
        {
            //try
            //{
            //    byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
            //    var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
            //    byte[] hash = hasher.HashData(inputBytes);
            //    string hashPass = Convert.ToBase64String(hash);

            //    string authorization = string.Format("{0}|{1}|{2}", "VetMobile", UserName, hashPass);
            //    string authorizationBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization));

            //    string url = "http://ppgservices-001-site6.ctempurl.com/Help/Api/GET-api-v1-VetClinic-Authorize";
            //    HttpClient client = new HttpClient();
            //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Services.DataHelper.GetInstance().GetAuthorization());
            //    HttpResponseMessage response = await client.GetAsync(url);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        string responseBody = await response.Content.ReadAsStringAsync();
            //    }
            //}
            //catch (Exception)
            //{

            //}
        }

        private async void GetClinicInfo()
        {
            try
            {
                string url = "http://ppgservices-001-site6.ctempurl.com/api/v1/ClinicProperties";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Services.DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {

            }
        }

        private void ShowSettingsPage()
        {
            if (string.IsNullOrEmpty(Password))
            {
                DisplayAlert(AppResources.Enter_Password);
                return;
            }
            ShowViewModel<SettingsViewModel>();
        }

        private void DisplayAlert(string message)
        {
            MessagingCenter.Send<MainViewModel, string>(this, "DisplayAlert", message);
        }
    }
}
