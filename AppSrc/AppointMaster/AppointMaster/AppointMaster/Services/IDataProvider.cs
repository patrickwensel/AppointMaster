using AppointMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Services
{
    public interface IDataProvider
    {
        Task<string> Login(string Password, string UserName);

        Task<string> GetAppointments();

        Task<string> CheckInWithCode(string appointmentCode);

        Task<string> GetPatients();

        void GetSpecies();

        Task<string> Complate(PostModel data);
    }
}
