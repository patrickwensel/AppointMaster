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

        ISecureStorage secureStorage;

        public LoginViewModel()
        {
            secureStorage = DataHelper.GetInstance().SecureStorage;

            UserName = "c0a0f76d-261c-474a-8db9-d5f815e40958";
            Password = "K!ll3r";

            try
            {
                byte[] bytes = secureStorage.Retrieve("DemoMode");
                bool isDemoMode = Convert.ToBoolean(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                DataHelper.GetInstance().IsDemoMode = isDemoMode;
            }
            catch (Exception ex)
            {

            }
        }

        private async void Login()
        {
            IsBusy = true;

            string msg =await DataProvider.GetDataProvider().Login(Password,UserName);

            if (msg == AppResources.OK)
            {
                ShowViewModel<MainViewModel>();
            }
            else
            {
                DisplayAlert(msg);
            }

            IsBusy = false;
        }

        public void DisplayAlert(string message)
        {
            MessagingCenter.Send<LoginViewModel, string>(this, "DisplayAlert", message);
        }

        private void ShowSettingPage()
        {
            try
            {
                DataHelper.GetInstance().SecureStorage.Delete("UserName");
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
