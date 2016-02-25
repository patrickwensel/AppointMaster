using AppointMaster.Resources;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
