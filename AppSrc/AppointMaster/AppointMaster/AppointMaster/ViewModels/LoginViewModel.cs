using AppointMaster.Models;
using AppointMaster.Resources;
using AppointMaster.Services;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Services;

namespace AppointMaster.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(() => UserName); }
        }

        private string _password;
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

        public MvxCommand ShowSettingsCommand
        {
            get
            {
                return new MvxCommand(() => ShowSettingPage());
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

            var secureStorage = DataHelper.GetInstance().SecureStorage;

            //secureStorage.Store("UserName", Encoding.UTF8.GetBytes(UserName));
            //ShowViewModel<MainViewModel>();
            //return;

            try
            {
                byte[] bytes = secureStorage.Retrieve("BaseAPI");

                DataHelper.GetInstance().BaseAPI = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
                var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
                byte[] hash = hasher.HashData(inputBytes);
                string hashPass = Convert.ToBase64String(hash);

                string authorization = string.Format("{0}|{1}|{2}", "VetMobile", UserName, hashPass);
                string authorizationBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization));

                DataHelper.GetInstance().SetAuthorization(authorizationBase64);

                //DataHelper.GetInstance().SetAuthorization("VmV0TW9iaWxlfGMwYTBmNzZkLTI2MWMtNDc0YS04ZGI5LWQ1ZjgxNWU0MDk1OHxxMzhDbXVZNWk2ZjFyZDRxUnRVdWx1UW1YT1U9");

                string url = DataHelper.GetInstance().BaseAPI + "VetClinic";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var clinicItem = JsonConvert.DeserializeObject<ClinicModel>(responseBody);

                    DataHelper.GetInstance().Clinic = clinicItem;

                    DataHelper.GetInstance().PrimaryColor = Color.FromHex(string.Format("#{0}", clinicItem.PrimaryColor));
                    DataHelper.GetInstance().SecondaryColor = Color.FromHex(string.Format("#{0}", clinicItem.SecondaryColor));

                    secureStorage.Store("UserName", Encoding.UTF8.GetBytes(UserName));

                    ShowViewModel<MainViewModel>();
                }
                else
                {
                    DisplayAlert(AppResources.Invalid_User);
                }
            }
            catch (Exception ex)
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

        private void ShowSettingPage()
        {
            try
            {
                var secureStorage = Resolver.Resolve<ISecureStorage>();
                secureStorage.Delete("UserName");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ShowSetting();
            }
        }

        public void ShowSetting()
        {
            ShowViewModel<SettingsViewModel>();
        }
    }
}
