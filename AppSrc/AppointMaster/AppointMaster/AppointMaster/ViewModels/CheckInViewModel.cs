using AppointMaster.Models;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.ViewModels
{
    public class CheckInViewModel : MvxViewModel
    {
        public ObservableCollection<CheckInInfoModel> Items { get; set; }

        public CheckInViewModel()
        {
            Items = new ObservableCollection<CheckInInfoModel>();
            Items.Add(new CheckInInfoModel { Date = "9:00", Info = "John Smith with Fido and Buddy" });
            Items.Add(new CheckInInfoModel { Date = "9:10", Info = "John Smith with Felicia" });
            Items.Add(new CheckInInfoModel { Date = "9:20", Info = "John Doe with Sir Barks-a-lot" });
        }

        public MvxCommand ShowRegistrationCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<RegistrationViewModel>());
            }
        }

        public MvxCommand ShowMainCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<MainViewModel>());
            }
        }
    }
}
