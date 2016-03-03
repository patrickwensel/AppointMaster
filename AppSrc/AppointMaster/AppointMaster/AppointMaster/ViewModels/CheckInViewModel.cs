using AppointMaster.Models;
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
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
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

        public ObservableCollection<DisplayAppointmentModel> Items { get; set; }

        public CheckInViewModel()
        {
            Items = new ObservableCollection<DisplayAppointmentModel>();
            //Items.Add(new CheckInInfoModel { Date = "9:00", Info = "John Smith with Fido and Buddy" });
            //Items.Add(new CheckInInfoModel { Date = "9:10", Info = "John Smith with Felicia" });
            //Items.Add(new CheckInInfoModel { Date = "9:20", Info = "John Doe with Sir Barks-a-lot" });
            GetAppointmentIDs();
        }

        public async void GetAppointmentIDs()
        {
            IsBusy = true;
            try
            {
                string url = "http://ppgservices-001-site6.ctempurl.com/api/v1/VetAppointment";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Services.DataHelper.GetInstance().GetAuthorization());
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
                string url = "http://ppgservices-001-site6.ctempurl.com/api/v1/VetAppointment/" + id + "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Services.DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    AppointmentModel appointment = JsonConvert.DeserializeObject<AppointmentModel>(responseBody);

                    if (appointment != null)
                    {
                        string patientName = null;
                        foreach (var patientItem in appointment.Patients)
                        {
                            patientName += string.Format("{0} and", patientItem.Name);
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
                            PatientName = string.Format("with {0}", patientName.Substring(0, patientName.Length - 3)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
