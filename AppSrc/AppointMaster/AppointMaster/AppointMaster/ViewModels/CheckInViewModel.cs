using AppointMaster.Models;
using AppointMaster.Resources;
using AppointMaster.Services;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppointMaster.ViewModels
{
    public class CheckInViewModel : MvxViewModel
    {
        public bool IsStopTimer { get; set; }

        private bool _isDigit;
        public bool IsDigit
        {
            get { return _isDigit; }
            set { _isDigit = value; RaisePropertyChanged(() => IsDigit); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _appointmentCode;
        public string AppointmentCode
        {
            get { return _appointmentCode; }
            set
            {
                if (string.IsNullOrEmpty(value) || (value != null && System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9-]{1,4}$")))
                {
                    _appointmentCode = value;
                }
                RaisePropertyChanged(() => AppointmentCode);
            }
        }

        public MvxCommand ShowMainCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<MainViewModel>());
            }
        }

        public MvxCommand ShowRegistrationCommand
        {
            get
            {
                return new MvxCommand(() => ShowRegistration());
            }
        }

        public MvxCommand CheckInCommand
        {
            get
            {
                return new MvxCommand(() => CheckIn());
            }
        }

        public ObservableCollection<DisplayAppointmentModel> Items { get; set; }

        public event EventHandler<string> SendMessage;

        public CheckInViewModel()
        {
            Items = new ObservableCollection<DisplayAppointmentModel>();
            try
            {
                var bytes = DataHelper.GetInstance().SecureStorage.Retrieve("CheckInModel");
                string isDigit = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                if (isDigit == AppResources.Appointment_List)
                {
                    IsDigit = false;
                }
                else
                {
                    IsDigit = true;
                }
            }
            catch (Exception ex)
            {
            }

            //Appointment List Model
            if (!IsDigit)
            {
                IsStopTimer = false;
                GetAppointments();
                Device.StartTimer(new TimeSpan(0, 0, 15), () =>
                {
                    if (!IsStopTimer)
                        GetAppointments();
                    return true;
                });
            }
        }

        private async void CheckIn()
        {
            IsBusy = true;

            string msg = await DataProvider.GetDataProvider().CheckInWithCode(AppointmentCode);

            IsBusy = false;

            if (msg == AppResources.OK)
            {
                ShowViewModel<RegistrationViewModel>();
                return;
            }

            DisplayAlert(msg);
        }

        public async void GetAppointments()
        {
            IsBusy = true;

            string msg = await DataProvider.GetDataProvider().GetAppointments();

            if (msg == AppResources.OK)
            {
                Items.Clear();
                foreach (var item in DataHelper.GetInstance().Appointments)
                {
                    Items.Add(item);
                }
            }
            else
                DisplayAlert(msg);

            IsBusy = false;
        }

        public void DisplayAlert(string message)
        {
            if (SendMessage != null)
            {
                SendMessage(this, message);
            }
        }

        public void ShowCheckedIn(DisplayAppointmentModel model)
        {
            DataHelper.GetInstance().SetSelectedAppointment(model);

            ShowViewModel<RegistrationViewModel>();
        }

        private void ShowRegistration()
        {
            DataHelper.GetInstance().SetSelectedAppointment(null);
            ShowViewModel<RegistrationViewModel>();
        }
    }
}
