using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                return;
            }

            ShowViewModel<CheckInViewModel>();
        }
    }
}
