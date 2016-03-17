using AppointMaster.Models;
using AppointMaster.Resources;
using Newtonsoft.Json;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppointMaster.Services
{
    public class ServerDataProvider : IDataProvider
    {
        public async Task<string> Login(string Password, string UserName)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    return AppResources.Enter_Clinic_ID;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    return AppResources.Enter_Password;
                }

                var secureStorage = DataHelper.GetInstance().SecureStorage;

                byte[] bytes = secureStorage.Retrieve("BaseAPI");

                DataHelper.GetInstance().BaseAPI = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
                var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
                byte[] hash = hasher.HashData(inputBytes);
                string hashPass = Convert.ToBase64String(hash);

                string authorization = string.Format("{0}|{1}|{2}", "VetMobile", UserName, hashPass);
                string authorizationBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization));

                DataHelper.GetInstance().SetAuthorization(authorizationBase64);

                string url = DataHelper.GetInstance().BaseAPI + "VetClinic";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var clinicItem = JsonConvert.DeserializeObject<ClinicModel>(responseBody);

                    DataHelper.GetInstance().Clinic = clinicItem;

                    DataHelper.GetInstance().PrimaryColor = Color.FromHex(string.Format("#{0}", clinicItem.PrimaryColor));
                    DataHelper.GetInstance().SecondaryColor = Color.FromHex(string.Format("#{0}", clinicItem.SecondaryColor));

                    secureStorage.Store("UserName", Encoding.UTF8.GetBytes(UserName));

                    return AppResources.OK;
                }
                else
                {
                    return AppResources.Invalid_User;
                }
            }
            catch (Exception ex)
            {
                return AppResources.Server_Error;
            }
        }

        public void GetSpecies()
        {
            DataHelper.GetInstance().Species.Clear();

            foreach (var item in DataHelper.GetInstance().Clinic.SpeciesSupported)
            {
                var model = new DisplaySpeciesModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    ClinicID = item.ClinicID,
                    SpeciesID = item.SpeciesID,
                    PrimaryDisplay = item.PrimaryDisplay,
                    Logo = item.Logo,
                    IsChecked = false
                };

                DataHelper.GetInstance().Species.Add(model);
            }
        }

        public async Task<string> GetAppointments()
        {
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

                    if (appointmentIDs == null)
                    {
                        return AppResources.OK;
                    }

                    foreach (var appointmentItem in DataHelper.GetInstance().Appointments)
                    {
                        if (!appointmentIDs.Contains(appointmentItem.ID))
                        {
                            DataHelper.GetInstance().Appointments.Remove(appointmentItem);
                        }
                    }

                    foreach (var itemID in appointmentIDs)
                    {
                        if (DataHelper.GetInstance().Appointments.Select(x => x.ID).Contains(itemID))
                            continue;

                        string appointmentUrl = DataHelper.GetInstance().BaseAPI + "VetAppointment/" + itemID + "";

                        HttpClient clientItem = new HttpClient();
                        clientItem.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                        HttpResponseMessage responseItem = await clientItem.GetAsync(appointmentUrl);
                        if (responseItem.IsSuccessStatusCode)
                        {
                            string responseBodyItem = await responseItem.Content.ReadAsStringAsync();

                            var appointment = JsonConvert.DeserializeObject<AppointmentModel>(responseBodyItem);
                            if (appointment != null)
                            {
                                string patientName = null;
                                foreach (var patientItem in appointment.Patients)
                                {
                                    patientName += string.Format("{0} and ", patientItem.Name);
                                }
                                DataHelper.GetInstance().Appointments.Add(new DisplayAppointmentModel
                                {
                                    ID = appointment.ID,
                                    ClientID = appointment.ClientID,
                                    ClinicID = appointment.ClinicID,
                                    Time = appointment.Time.HasValue ? appointment.Time.Value.ToLocalTime() : appointment.Time,
                                    CheckedIn = appointment.CheckedIn,
                                    Client = appointment.Client,
                                    Clinic = appointment.Clinic,
                                    Patients = appointment.Patients,
                                    PatientName = string.IsNullOrEmpty(patientName) ? null : string.Format("with {0}", patientName.Substring(0, patientName.Length - 4)),
                                });
                            }
                        }
                        else
                        {
                            return AppResources.Server_Error;
                        }
                    }

                    return AppResources.OK;
                }
                else
                {
                    return AppResources.Server_Error;
                }
            }
            catch (Exception ex)
            {
                return AppResources.Server_Error;
            }
        }

        public async Task<string> CheckInWithCode(string appointmentCode)
        {
            if (string.IsNullOrEmpty(appointmentCode))
            {
                return AppResources.Enter_Code;
            }
            try
            {
                string url = DataHelper.GetInstance().BaseAPI + "VetTestAppointment/" + appointmentCode + "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return AppResources.Server_Error;
                }
                string responseBody = await response.Content.ReadAsStringAsync();
                var appointment = JsonConvert.DeserializeObject<AppointmentModel>(responseBody);
                if (appointment == null)
                {
                    return AppResources.Invalid_Appointment_Code;
                }

                string patientName = null;
                foreach (var patientItem in appointment.Patients)
                {
                    patientName += string.Format("{0} and ", patientItem.Name);
                }

                DataHelper.GetInstance().SetSelectedAppointment(new DisplayAppointmentModel
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

                return AppResources.OK;
            }
            catch (Exception ex)
            {
                return AppResources.Server_Error;
            }
        }

        public async Task<string> GetPatients()
        {
            try
            {
                DataHelper.GetInstance().Patients.Clear();

                string url = DataHelper.GetInstance().BaseAPI + "VetPatient/" + DataHelper.GetInstance().GetSelectedAppointment().ClientID;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return AppResources.Server_Error;
                }

                int i = 0;
                string responseBody = await response.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<PatientModel>>(responseBody);
                foreach (var item in patients)
                {
                    i++;
                    DataHelper.GetInstance().Patients.Add(new DisplayPatientModel
                    {
                        ID = item.ID,
                        Name = item.Name,
                        SpeciesID = item.SpeciesID,
                        Breed = item.Breed,
                        Gender = item.Gender,
                        Logo = DataHelper.GetInstance().Species.Where(x => x.ID == item.SpeciesID).FirstOrDefault().Logo,
                        Birthdate = item.Birthdate,
                        RegistrationID = i,
                        Species = DataHelper.GetInstance().Species.Where(x => x.ID == item.SpeciesID).FirstOrDefault().Name
                    });
                }
                return AppResources.OK;
            }
            catch (Exception ex)
            {
                return AppResources.Server_Error;
            }
        }

        public async Task<string> Complate(PostModel data)
        {
            try
            {
                string p = JsonConvert.SerializeObject(data);

                string url = DataHelper.GetInstance().BaseAPI + "VetAppointment";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", DataHelper.GetInstance().GetAuthorization());
                HttpResponseMessage response = await client.PostAsync(url, new StringContent(p, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    DataHelper.GetInstance().SetSelectedAppointment(null);

                    return AppResources.OK;
                }

                return AppResources.Server_Error;
            }
            catch (Exception ex)
            {
                return AppResources.Server_Error;
            }
        }
    }
}
