using MvvmCross.Core.ViewModels;
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
    public class SettingsViewModel : MvxViewModel
    {
        public string _baseAPIAddress;
        public string BaseAPIAddress
        {
            get { return _baseAPIAddress; }
            set { _baseAPIAddress = value; RaisePropertyChanged(() => BaseAPIAddress); }
        }

        public MvxCommand ShowCheckInCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CheckInViewModel>());
            }
        }

        public MvxCommand SaveCommand
        {
            get
            {
                return new MvxCommand(() => Save());
            }
        }

        ISecureStorage secureStorage;

        public SettingsViewModel()
        {
            try
            {
                BaseAPIAddress = "http://ppgservices-001-site6.ctempurl.com/api/v1/";

                secureStorage = Resolver.Resolve<ISecureStorage>();

                var bytes =secureStorage.Retrieve("BaseAPI");

                BaseAPIAddress = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
            }
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(BaseAPIAddress))
            {
                MessagingCenter.Send<SettingsViewModel, string>(this, "DisplayAlert", "");
                return;
            }
          
            try
            {
                secureStorage.Store("BaseAPI", Encoding.UTF8.GetBytes(BaseAPIAddress));
                Services.DataHelper.GetInstance().BaseAPI = BaseAPIAddress;
                secureStorage.Retrieve("UserName");

                ShowViewModel<MainViewModel>();
            }
            catch (Exception)
            {
                ShowViewModel<LoginViewModel>();
            }
        }
    }
}
