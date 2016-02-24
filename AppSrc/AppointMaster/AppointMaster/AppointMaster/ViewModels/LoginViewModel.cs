using AppointMaster.Resources;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private void Login()
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

            ShowViewModel<MainViewModel>();
        }

        public void DisplayAlert(string message)
        {
            MessagingCenter.Send<LoginViewModel, string>(this, "DisplayAlert", message);
        }
    }
}
