using AppointMaster.Models;
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

        private string _selectedState;
        public string SelectedState
        {
            get { return _selectedState; }
            set
            {
                _selectedState = value;
                RaisePropertyChanged(() => SelectedState);
            }
        }

        private string _selectedBreed;
        public string SelectedBreed
        {
            get { return _selectedBreed; }
            set
            {
                _selectedBreed = value;
                RaisePropertyChanged(() => SelectedBreed);
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

        private string _patientName;
        public string PatientName
        {
            get { return _patientName; }
            set
            {
                _patientName = value;
                RaisePropertyChanged(() => PatientName);
            }
        }

        //private string _patientBreed;
        //public string PatientBreed
        //{
        //    get { return _patientBreed; }
        //    set
        //    {
        //        _patientBreed = value;
        //        RaisePropertyChanged(() => PatientBreed);
        //    }
        //}

        private string _breed;
        public string Breed
        {
            get { return _breed; }
            set
            {
                _breed = value;
                RaisePropertyChanged(() => Breed);
            }
        }

        private string _patientGender;
        public string PatientGender
        {
            get { return _patientGender; }
            set
            {
                _patientGender = value;
                RaisePropertyChanged(() => PatientGender);
            }
        }

        private DateTime _patientBirth;
        public DateTime PatientBirth
        {
            get { return _patientBirth; }
            set
            {
                _patientBirth = value;
                RaisePropertyChanged(() => PatientBirth);
            }
        }

        //Check
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

        public ObservableCollection<string> GenderList { get; set; }

        public ObservableCollection<string> TitleList { get; set; }

        public ObservableCollection<string> StateList { get; set; }

        public ObservableCollection<string> BreedList { get; set; }

        public ObservableCollection<PatientInfo> PatientList { get; set; }

        public ObservableCollection<PatientInfo> SelectedPatientList { get; set; }

        public MvxCommand ShowCheckInCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CheckInViewModel>());
            }
        }

        public RegistrationViewModel()
        {
            PatientBirth = DateTime.Now;

            BreedList = new ObservableCollection<string>();
            SelectedBreed = "Dog";
            BreedList.Add("Dog");
            BreedList.Add("Cat");

            GenderList = new ObservableCollection<string>();
            PatientGender = "Male";
            GenderList.Add("Male");
            GenderList.Add("Female");

            TitleList = new ObservableCollection<string>();
            SelectedTitle = "Mr.";
            TitleList.Add("Mr.");
            TitleList.Add("Mrs.");

            StateList = new ObservableCollection<string>();
            SelectedState = "MD";
            StateList.Add("MD");
            StateList.Add("AZ");
            StateList.Add("AL");
            StateList.Add("AK");

            PatientList = new ObservableCollection<PatientInfo>();
            PatientList.Add(new PatientInfo { PatientName = "Fido", Image = "dog.png", IsChecked = false });
            PatientList.Add(new PatientInfo { PatientName = "Buddy", Image = "dog.png", IsChecked = false });
            PatientList.Add(new PatientInfo { PatientName = "Jasper", Image = "cat.png", IsChecked = false });

            SelectedPatientList = new ObservableCollection<PatientInfo>();
        }
    }

    //public class TitleModel
    //{
    //    public string Title { get; set; }
    //}

    //public class StateModel
    //{
    //    public string State { get; set; }
    //}
}
