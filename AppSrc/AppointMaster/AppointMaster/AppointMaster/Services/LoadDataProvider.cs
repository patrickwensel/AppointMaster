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
            var item = DataHelper.GetInstance().Appointments.Where(x => x.Code == appointmentCode).FirstOrDefault();
            if (item != null)
            {
                return AppResources.Invalid_Appointment_Code;
            }

            DataHelper.GetInstance().SetSelectedAppointment(item);

            return AppResources.OK;
        }

        public async Task<string> Complate(PostModel data)
        {
            int id = 0;
            if (DataHelper.GetInstance().Appointments.Count > 0)
                id = DataHelper.GetInstance().Appointments.Select(x => x.ID).Max() + 1;
            DataHelper.GetInstance().Appointments.Add(new DisplayAppointmentModel
            {
                ID = id,
                ClientID = 1,
                Time = DateTime.Now,
                CheckedIn = false,
            });

            return AppResources.OK;
        }

        public async Task<string> GetAppointments()
        {
            return AppResources.OK;
        }

        public async Task<string> GetPatients()
        {
            return AppResources.OK;
        }

        public void GetSpecies()
        {
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 1,
                IsChecked = false,
                Name = "Dog",
                PrimaryDisplay = true,
                ImgLogo = "dog.png"
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 2,
                IsChecked = false,
                Name = "Cat",
                PrimaryDisplay = true
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 3,
                IsChecked = false,
                Name = "Bird",
                PrimaryDisplay = true
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 4,
                IsChecked = false,
                Name = "Fish",
                PrimaryDisplay = false
            });
            DataHelper.GetInstance().Species.Add(new DisplaySpeciesModel
            {
                ID = 5,
                IsChecked = false,
                Name = "Other",
                PrimaryDisplay = false
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
