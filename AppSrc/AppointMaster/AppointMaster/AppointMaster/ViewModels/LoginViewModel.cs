using AppointMaster.Resources;
using MvvmCross.Core.ViewModels;
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

        private async void Login()
        {
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

            string url = "http://ppgservices-001-site6.ctempurl.com/api/v1/VetClinic";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "VmV0TW9iaWxlfGMwYTBmNzZkLTI2MWMtNDc0YS04ZGI5LWQ1ZjgxNWU0MDk1OHxxMzhDbXVZNWk2ZjFyZDRxUnRVdWx1UW1YT1U9 ");
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                ShowViewModel<MainViewModel>();
            }
        }

        public void DisplayAlert(string message)
        {
            MessagingCenter.Send<LoginViewModel, string>(this, "DisplayAlert", message);
        }
    }
}
