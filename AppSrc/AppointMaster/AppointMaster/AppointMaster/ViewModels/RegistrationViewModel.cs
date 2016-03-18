using AppointMaster.Models;
using AppointMaster.Resources;
using AppointMaster.Services;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        private DisplaySpeciesModel _selectedSpecies;
        public DisplaySpeciesModel SelectedSpecies
        {
            get { return _selectedSpecies; }
            set
            {
                _selectedSpecies = value;
                RaisePropertyChanged(() => SelectedSpecies);
            }
        }

        private bool _isShowNotPrimarySl;
        public bool IsShowNotPrimarySl
        {
            get { return _isShowNotPrimarySl; }
            set { _isShowNotPrimarySl = value; RaisePropertyChanged(() => IsShowNotPrimarySl); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _notPrimarySpeciesName;
        public string NotPrimarySpeciesName
        {
            get { return _notPrimarySpeciesName; }
            set
            {
                _notPrimarySpeciesName = value;
                RaisePropertyChanged(() => NotPrimarySpeciesName);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = !string.IsNullOrEmpty(value) ? value.Substring(0, 1).ToUpper() + value.Substring(1) : value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = !string.IsNullOrEmpty(value) ? value.Substring(0, 1).ToUpper() + value.Substring(1) : value;
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

        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                RaisePropertyChanged(() => PostalCode);
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                if (value != null && value.Length == 10)
                {
                    _phone = string.Format("({0}) {1}-{2}", value.Substring(0, 3), value.Substring(3, 3), value.Substring(6, 4));
                }
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

        private DateTime? _patientBirth;
        public DateTime? PatientBirth
        {
            get { return _patientBirth; }
            set
            {
                _patientBirth = value;
                RaisePropertyChanged(() => PatientBirth);
            }
        }

        public MvxCommand ShowCheckInCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CheckInViewModel>());
            }
        }

        public MvxCommand ShowCancelCommand
        {
            get
            {
                return new MvxCommand(() => Cancel());
            }
        }

        public MvxCommand CompleteCommand
        {
            get
            {
                return new MvxCommand(() => Complete());
            }
        }

        public ObservableCollection<string> GenderList { get; set; }

        public ObservableCollection<string> TitleList { get; set; }

        public ObservableCollection<string> StateList { get; set; }

        public ObservableCollection<DisplayPatientModel> PatientList { get; set; }
        public ObservableCollection<PatientModel> CheckedPatientList { get; set; }
        public ObservableCollection<DisplaySpeciesModel> SpeciesList { get; set; }
        public ObservableCollection<DisplaySpeciesModel> SpeciesPrimaryList { get; set; }
        public ObservableCollection<DisplaySpeciesModel> SpeciesNotPrimaryList { get; set; }

        public event EventHandler<string> SendMessage;

        public RegistrationViewModel()
        {
            PatientBirth = DateTime.Now;

            GenderList = new ObservableCollection<string>();
            GenderList.Add("Male");
            GenderList.Add("Female");

            TitleList = new ObservableCollection<string>();
            TitleList.Add("Mr");
            TitleList.Add("Dr");
            TitleList.Add("Mrs");

            StateList = new ObservableCollection<string>();
            StateList.Add("MD");
            StateList.Add("AZ");
            StateList.Add("AL");
            StateList.Add("AK");

            PatientList = new ObservableCollection<DisplayPatientModel>();
            CheckedPatientList = new ObservableCollection<PatientModel>();

            SpeciesList = new ObservableCollection<DisplaySpeciesModel>();
            SpeciesPrimaryList = new ObservableCollection<DisplaySpeciesModel>();
            SpeciesNotPrimaryList = new ObservableCollection<DisplaySpeciesModel>();

            DataProvider.GetDataProvider().GetSpecies();

            GetAllSpecies();

            if (DataHelper.GetInstance().GetSelectedAppointment() != null)
            {
                CheckedAppointment();

                GetAllPatients();
            }
            else
            {
                SelectedTitle = "Mr.";
                SelectedState = "MD";
                PatientGender = "Male";
            }
        }

        private void GetAllSpecies()
        {
            foreach (var item in DataHelper.GetInstance().Species)
            {
                SpeciesList.Add(item);

                if (item.PrimaryDisplay)
                    SpeciesPrimaryList.Add(item);
                else
                    SpeciesNotPrimaryList.Add(item);
            }

            if (SpeciesNotPrimaryList.Count == 0)
                IsShowNotPrimarySl = false;
            else
                IsShowNotPrimarySl = true;
        }

        private async void GetAllPatients()
        {
            IsBusy = true;

            string msg = await DataProvider.GetDataProvider().GetPatients();

            IsBusy = false;

            if (msg == AppResources.OK)
            {
                foreach (var item in DataHelper.GetInstance().Patients)
                {
                    if (item.ClientID == DataHelper.GetInstance().GetSelectedAppointment().ClientID)
                    {
                        item.RegistrationID = item.ID;
                        PatientList.Add(item);
                    }
                }
                return;
            }
            DisplayAlert(msg);
        }

        private async void Complete()
        {
            try
            {
                IsBusy = true;

                List<PatientModel> lstPatient = new List<PatientModel>();

                var appointment = DataHelper.GetInstance().GetSelectedAppointment();

                PostModel model = new PostModel
                {
                    ClientModel = new ClientModel
                    {
                        ID = appointment != null ? appointment.ClientID : 0,
                        Title = SelectedTitle,
                        FirstName = FirstName,
                        LastName = LastName,
                        Address = StreetAddress,
                        City = City,
                        StateProvince = SelectedState,
                        PostalCode = PostalCode,
                        Phone = Phone,
                        Email = Email,
                        DefaultCultureCode = appointment != null ? appointment.Client.DefaultCultureCode : "en-US",
                    },

                    AppointmentModel = new AppointmentModel
                    {
                        ID = appointment != null ? appointment.ID : 0,
                        ClientID = appointment != null ? appointment.ClientID : 0,
                        ClinicID = DataHelper.GetInstance().Clinic.ID,
                        Time = DateTime.UtcNow,
                        CheckedIn = false,
                    },

                    PatientList = PatientList.Select(x => new DisplayPatientModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Breed = x.Breed,
                        Gender = x.Gender,
                        ClientID = x.ClientID,
                        SpeciesID = x.SpeciesID,
                        Birthdate = x.Birthdate,
                        IsChecked = x.IsChecked

                    }).ToList()
                };

                string msg = await DataProvider.GetDataProvider().Complate(model);
                if (msg == AppResources.OK)
                {
                    ShowViewModel<CheckedInViewModel>();
                    return;
                }

                DisplayAlert(msg);
            }
            catch (Exception ex)
            {
                DisplayAlert(AppResources.Server_Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CheckedAppointment()
        {
            DisplayAppointmentModel model = DataHelper.GetInstance().GetSelectedAppointment();

            SelectedTitle = TitleList.Where(x => x == model.Client.Title).FirstOrDefault();
            SelectedState = StateList.Where(x => x == model.Client.StateProvince).FirstOrDefault();

            FirstName = model.Client.FirstName;
            LastName = model.Client.LastName;
            StreetAddress = model.Client.Address;
            City = model.Client.City;
            PostalCode = model.Client.PostalCode;
            Phone = model.Client.Phone;
            Email = model.Client.Email;
        }

        public void DisplayAlert(string message)
        {
            if (SendMessage != null)
            {
                SendMessage(this, message);
            }
        }

        private void Cancel()
        {
            ShowViewModel<RegistrationCancelViewModel>();
        }
    }
}
