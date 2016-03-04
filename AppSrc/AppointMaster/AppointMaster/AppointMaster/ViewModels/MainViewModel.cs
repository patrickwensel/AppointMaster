using AppointMaster.Resources;
using AppointMaster.Services;
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
        private string _clinicName;
        public string ClinicName
        {
            get { return _clinicName; }
            set { _clinicName = value;RaisePropertyChanged(() => ClinicName); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; RaisePropertyChanged(() => Address); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; RaisePropertyChanged(() => City); }
        }

        private string _stateProvince;
        public string StateProvince
        {
            get { return _stateProvince; }
            set { _stateProvince = value; RaisePropertyChanged(() => StateProvince); }
        }

        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; RaisePropertyChanged(() => PostalCode); }
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

        public MainViewModel()
        {
            ClinicName = DataHelper.GetInstance().Clinic.Name;
            Address = DataHelper.GetInstance().Clinic.Address1;
            City = DataHelper.GetInstance().Clinic.City;
            StateProvince = DataHelper.GetInstance().Clinic.StateProvince;
            PostalCode = DataHelper.GetInstance().Clinic.PostalCode;
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
