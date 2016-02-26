using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.ViewModels
{
    public class RegistrationViewModel : MvxViewModel
    {
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

        //Check
        private string _patientSpecies;
        public string PatientSpecies
        {
            get { return _patientSpecies; }
            set
            {
                _patientSpecies = value;
                RaisePropertyChanged(() => PatientSpecies);
            }
        }

        private bool _isDog;
        public bool IsDog
        {
            get { return _isDog; }
            set
            {
                _isDog = value;
                RaisePropertyChanged(() => IsDog);
            }
        }

        private bool _isfish;
        public bool IsFish
        {
            get { return _isfish; }
            set
            {
                _isfish = value;
                RaisePropertyChanged(() => IsFish);
            }
        }

        private bool _isCat;
        public bool IsCat
        {
            get { return _isCat; }
            set
            {
                _isCat = value;
                RaisePropertyChanged(() => IsCat);
            }
        }

        private bool _isHamster;
        public bool IsHamster
        {
            get { return _isHamster; }
            set
            {
                _isHamster = value;
                RaisePropertyChanged(() => IsHamster);
            }
        }

        private bool _isBird;
        public bool IsBird
        {
            get { return _isBird; }
            set
            {
                _isBird = value;
                RaisePropertyChanged(() => IsBird);
            }
        }

        private bool _isOther;
        public bool IsOther
        {
            get { return _isOther; }
            set
            {
                _isOther = value;
                RaisePropertyChanged(() => IsOther);
            }
        }

        public ObservableCollection<TitleModel> TitleList { get; set; }

        public ObservableCollection<StateModel> StateList { get; set; }

        public ObservableCollection<PatientInfo> PatientList { get; set; }

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

            PatientList = new ObservableCollection<PatientInfo>();
            PatientList.Add(new PatientInfo { PatientName = "Fido", Image = "dog.png", IsChecked = false });
            PatientList.Add(new PatientInfo { PatientName = "Buddy", Image = "dog.png", IsChecked = false });
            PatientList.Add(new PatientInfo { PatientName = "Jasper", Image = "cat.png", IsChecked = false });
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

    public class PatientInfo: INotifyPropertyChanged
    {
        public string PatientName { get; set; }

        //public string _isChecked;
        //public string IsChecked
        //{
        //    get { return _isChecked; }
        //    set
        //    {
        //        _isChecked = value;
        //        OnPropertyChanged("IsChecked");
        //    }
        //}

        public bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        //public bool IsChecked { get; set; }

        public string Image { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
