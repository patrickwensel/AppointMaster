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
using XLabs.Ioc;
using XLabs.Platform.Services;

namespace AppointMaster.ViewModels
{
    public class MainViewModel:MvxViewModel
    {
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
                return new MvxCommand(() => ShowViewModel<SettingsViewModel>());
            }
        }

        public MvxCommand LogoutCommand
        {
            get
            {
                return new MvxCommand(() => Logout());
            }
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

        private void Logout()
        {
            //var secureStorage = Resolver.Resolve<ISecureStorage>();
            //secureStorage.Delete("UserName");
            //secureStorage.Delete("BaseAPI");
            ShowViewModel<LoginViewModel>();
        }

        private void DisplayAlert(string message)
        {
            MessagingCenter.Send<MainViewModel, string>(this, "DisplayAlert", message);
        }
    }
}
