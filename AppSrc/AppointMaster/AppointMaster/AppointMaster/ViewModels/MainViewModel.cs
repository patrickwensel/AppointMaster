using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppointMaster.ViewModels
{
    public class MainViewModel:MvxViewModel
    {
        private string _yourNickname = "???";
        public string YourNickname
        {
            get { return _yourNickname; }
            set { _yourNickname = value; RaisePropertyChanged(() => YourNickname); }
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
    }
}
