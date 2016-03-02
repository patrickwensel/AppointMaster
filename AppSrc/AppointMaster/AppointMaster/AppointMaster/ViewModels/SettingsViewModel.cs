using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        private void Save()
        {
            if (string.IsNullOrEmpty(BaseAPIAddress))
            {
                return;
            }
        }
    }
}
