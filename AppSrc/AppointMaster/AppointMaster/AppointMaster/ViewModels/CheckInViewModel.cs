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

namespace AppointMaster.ViewModels
{
    public class CheckInViewModel : MvxViewModel
    {
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
            set { _appointmentCode = value; RaisePropertyChanged(() => AppointmentCode); }
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
                return new MvxCommand(() => ShowViewModel<RegistrationViewModel>());
            }
        }

        public ObservableCollection<DisplayAppointmentModel> Items { get; set; }

        public CheckInViewModel()
        {
            Items = new ObservableCollection<DisplayAppointmentModel>();
            //Items.Add(new DisplayAppointmentModel
            //{
            //    ID = 1000,
            //    Time = DateTime.Now,
            //    PatientName = "Fido",
            //    Client = new ClientModel
            //    {
            //        Title = "Mr.",
            //        FirstName = "first",
            //        LastName = "last",
            //        Phone = "(410)555-1212",
            //        Email = "sameone@gmail.com",
            //        Address = "123 Some Road",
            //        City = "Somewhereville",
            //        StateProvince = "MD",
            //        PostalCode = "12345",
            //    }
            //});

            try
            {
                var bytes = DataHelper.GetInstance().SecureStorage.Retrieve("CheckInModel");
                if (Encoding.UTF8.GetString(bytes, 0, bytes.Length) == AppResources.Appointment_List)
                {
                    IsDigit = false;
                    GetAppointmentIDs();
                    Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 15), () => { GetAppointmentIDs(); return true; });
                }
                else
                {
                    IsDigit = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async void GetAppointmentIDs()
        {
            IsBusy = true;
            try
            {
                string url = DataHelper.GetInstance().BaseAPI + "VetAppointment";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<int> appointmentIDs = JsonConvert.DeserializeObject<List<int>>(responseBody);

                    if (appointmentIDs != null)
                    {
                        foreach (var appointmentItem in Items)
                        {
                            if (appointmentIDs.Contains(appointmentItem.ID))
                            {
                                Items.Remove(appointmentItem);
                            }
                        }

                        foreach (var itemID in appointmentIDs)
                        {
                            await GetAppointmentByID(itemID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetAppointmentByID(int id)
        {
            try
            {
                string url = DataHelper.GetInstance().BaseAPI + "VetAppointment/" + id + "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var appointment = JsonConvert.DeserializeObject<AppointmentModel>(responseBody);

                    if (appointment != null)
                    {
                        string patientName = null;
                        foreach (var patientItem in appointment.Patients)
                        {
                            patientName += string.Format("{0} and ", patientItem.Name);
                        }
                        Items.Add(new DisplayAppointmentModel
                        {
                            ID = appointment.ID,
                            ClientID = appointment.ClientID,
                            ClinicID = appointment.ClinicID,
                            Time = appointment.Time,
                            CheckedIn = appointment.CheckedIn,
                            Client = appointment.Client,
                            Clinic = appointment.Clinic,
                            Patients = appointment.Patients,
                            PatientName = string.IsNullOrEmpty(patientName) ? null : string.Format("with {0}", patientName.Substring(0, patientName.Length - 4)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ShowCheckedIn(DisplayAppointmentModel model)
        {
            DataHelper.GetInstance().SetSelectedAppointment(model);

            ShowViewModel<RegistrationViewModel>();
        }
    }
}
