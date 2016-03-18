using AppointMaster.Models;
using Newtonsoft.Json;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MvvmCross.Core.ViewModels;
using AppointMaster.ViewModels;
using AppointMaster.Resources;
using System.Collections.ObjectModel;

namespace AppointMaster.Services
{
    public class LoadDataProvider : IDataProvider
    {
        public async Task<string> CheckInWithCode(string appointmentCode)
        {
            if (string.IsNullOrEmpty(appointmentCode))
            {
                return AppResources.Enter_Code;
            }
            //var item = DataHelper.GetInstance().Appointments.Where(x => x.Code == appointmentCode).FirstOrDefault();
            //if (item != null)
            //{
            //    return AppResources.Invalid_Appointment_Code;
            //}

            //DataHelper.GetInstance().SetSelectedAppointment(item);

            return AppResources.Invalid_Appointment_Code;
        }

        public async Task<string> Complate(PostModel data)
        {
            if (data.AppointmentModel.ID > 0)
            {
                if (DataHelper.GetInstance().Appointments != null)
                {
                    DataHelper.GetInstance().Appointments.Remove(DataHelper.GetInstance().GetSelectedAppointment());

                    foreach (var patientItem in data.PatientList)
                    {
                        patientItem.RegistrationID = patientItem.ID;
                        var item = DataHelper.GetInstance().Patients.ToList().Where(x => x.RegistrationID == patientItem.RegistrationID).First();
                        if (item != null)
                            DataHelper.GetInstance().Patients.Remove(item);
                    }
                }

                return AppResources.OK;
            }

            int id = 1;
            if (DataHelper.GetInstance().Appointments.Count > 0)
                id = DataHelper.GetInstance().Appointments.Select(x => x.ID).Max() + 1;

            int clientId = 1;
            if (DataHelper.GetInstance().Appointments.Count > 0)
                clientId = DataHelper.GetInstance().Appointments.Select(x => x.ClientID).Max() + 1;

            DataHelper.GetInstance().Appointments.Add(new DisplayAppointmentModel
            {
                ID = id,
                ClientID = clientId,
                Time = DateTime.Now,
                CheckedIn = false,
                Client = data.ClientModel,
            });

            foreach (var item in data.PatientList)
            {
                var speciesItem = DataHelper.GetInstance().Species.Where(x => x.ID == item.SpeciesID).First();

                item.ID = id;
                item.ClientID = clientId;
                item.RegistrationID = id;
                item.Species = speciesItem.Name;
                item.Logo = speciesItem.Logo;
                DataHelper.GetInstance().Patients.Add(item);
            }

            return AppResources.OK;
        }

        public async Task<string> GetAppointments()
        {
            foreach (var item in DataHelper.GetInstance().Appointments)
            {
                string patientName = null;
                var patients = DataHelper.GetInstance().Patients.Where(x => x.ClientID == item.ClientID && x.IsChecked).ToList();
                foreach (var patientItem in patients)
                {
                    patientName += string.Format("{0} and ", patientItem.Name);
                }

                item.PatientName = string.IsNullOrEmpty(patientName) ? null : string.Format("with {0}", patientName.Substring(0, patientName.Length - 4));
            }
            return AppResources.OK;
        }

        public async Task<string> GetPatients()
        {
            foreach (var item in DataHelper.GetInstance().Patients)
            {
                if (item.ClientID == DataHelper.GetInstance().GetSelectedAppointment().ClientID)
                    item.IsChecked = false;
            }
            return AppResources.OK;
        }

        public void GetSpecies()
        {
            DataHelper.GetInstance().Species.Clear();

            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 1,
                IsChecked = false,
                Name = "Dog",
                PrimaryDisplay = true,
                Logo = Encoding.UTF8.GetBytes("dog.png")
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 2,
                IsChecked = false,
                Name = "Cat",
                PrimaryDisplay = true,
                Logo = Encoding.UTF8.GetBytes("cat.png")
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 3,
                IsChecked = false,
                Name = "Bird",
                PrimaryDisplay = true,
                Logo = Encoding.UTF8.GetBytes("bird.png")
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 4,
                IsChecked = false,
                Name = "Fish",
                PrimaryDisplay = false,
                Logo = Encoding.UTF8.GetBytes("fish.png")
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 5,
                IsChecked = false,
                Name = "Other",
                PrimaryDisplay = false,
                Logo = Encoding.UTF8.GetBytes("other.png")
            });
        }

        public async Task<string> Login(string Password, string UserName)
        {
            var clinicItem = new ClinicModel
            {
                Name = "Some Veterinary Clinic",
                City = "Somewhereville",
                StateProvince = "MD",
                PostalCode = "12345",
                Address1 = "123 Some Road",
                PrimaryColor = "#0279bd",
                SecondaryColor = "#87b12b"
            };

            DataHelper.GetInstance().Clinic = clinicItem;

            DataHelper.GetInstance().PrimaryColor = Color.FromHex(string.Format("#{0}", clinicItem.PrimaryColor));
            DataHelper.GetInstance().SecondaryColor = Color.FromHex(string.Format("#{0}", clinicItem.SecondaryColor));

            DataHelper.GetInstance().SecureStorage.Store("UserName", Encoding.UTF8.GetBytes("DemoMode"));

            return AppResources.OK;
        }
    }
}
