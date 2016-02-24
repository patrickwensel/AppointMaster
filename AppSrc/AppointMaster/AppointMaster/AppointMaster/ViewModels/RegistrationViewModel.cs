using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.ViewModels
{
    public class RegistrationViewModel : MvxViewModel
    {
        //private int _selectedPageIndex;
        //public int SelectedPageIndex
        //{
        //    get { return _selectedPageIndex; }
        //    set
        //    {
        //        if (value == _selectedPageIndex) return;
        //        _selectedPageIndex = value;
        //        RaisePropertyChanged(() => SelectedPageIndex);
        //    }
        //}

        private string _selectedTitle;
        public string SelectedTitle
        {
            get { return _selectedTitle; }
            set
            {
                _selectedTitle = value;
                RaisePropertyChanged(() => SelectedTitle);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }


        private string _streetAddress;
        public string StreetAddress
        {
            get { return _streetAddress; }
            set
            {
                _streetAddress = value;
                RaisePropertyChanged(() => StreetAddress);
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged(() => City);
            }
        }

        private StateModel _selectedState;
        public StateModel SelectedState
        {
            get { return _selectedState; }
            set
            {
                _selectedState = value;
                RaisePropertyChanged(() => SelectedState);
            }
        }

        private string _zip;
        public string Zip
        {
            get { return _zip; }
            set
            {
                _zip = value;
                RaisePropertyChanged(() => Zip);
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                RaisePropertyChanged(() => Phone);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        public ObservableCollection<TitleModel> TitleList { get; set; }

        public ObservableCollection<StateModel> StateList { get; set; }

        public MvxCommand ShowCheckInCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CheckInViewModel>());
            }
        }

        public RegistrationViewModel()
        {
            TitleList = new ObservableCollection<TitleModel>();
            TitleList.Add(new TitleModel { Title = "Mr." });
            TitleList.Add(new TitleModel { Title = "Mrs." });

            StateList = new ObservableCollection<StateModel>();
            StateList.Add(new StateModel { State = "MD" });
            StateList.Add(new StateModel { State = "AZ" });
            StateList.Add(new StateModel { State = "AL" });
            StateList.Add(new StateModel { State = "AK" });
        }
    }

    public class TitleModel
    {
        public string Title { get; set; }
    }

    public class StateModel
    {
        public string State { get; set; }
    }
}
