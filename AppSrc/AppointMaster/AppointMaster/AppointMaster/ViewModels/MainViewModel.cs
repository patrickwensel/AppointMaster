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
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value; RaisePropertyChanged(() => Name);
            }

        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value; RaisePropertyChanged(() => Address);
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;RaisePropertyChanged(() => City);
            }
        }

        private string _stateProvince;
        public string StateProvince
        {
            get
            {
                return _stateProvince;
            }
            set
            {
                _stateProvince = value; RaisePropertyChanged(() => StateProvince);
            }
        }

        private string _postalCode;
        public string PostalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                _postalCode = value; RaisePropertyChanged(() => PostalCode);
            }
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

        private void Logout()
        {
            try
            {
                DataHelper.GetInstance().BaseAPI = null;
                DataHelper.GetInstance().Appointments.Clear();
                DataHelper.GetInstance().Species.Clear();
                DataHelper.GetInstance().Patients.Clear();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ShowViewModel<LoginViewModel>();
            }
        }

        public MainViewModel()
        {
            if (DataHelper.GetInstance().Clinic != null)
            {
                Name= DataHelper.GetInstance().Clinic.Name;
                Address = DataHelper.GetInstance().Clinic.Address1;
                City = DataHelper.GetInstance().Clinic.City;
                StateProvince = DataHelper.GetInstance().Clinic.StateProvince;
                PostalCode = DataHelper.GetInstance().Clinic.PostalCode;
            }
        }
    }
}
