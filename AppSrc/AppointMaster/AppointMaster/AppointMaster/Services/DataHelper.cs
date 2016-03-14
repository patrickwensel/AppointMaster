using AppointMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Ioc;
using XLabs.Platform.Services;

namespace AppointMaster.Services
{
    public class DataHelper
    {
        static DataHelper me;

        string authorization;

        DisplayAppointmentModel model;

        public Xamarin.Forms.Color SecondaryColor { get; set; }

        public Xamarin.Forms.Color PrimaryColor { get; set; }

        public string BaseAPI { get; set; }

        public ISecureStorage SecureStorage { get; set; }

        public ClinicModel Clinic { get; set; }

        public string GetAuthorization()
        {
            return authorization;
        }

        public void SetAuthorization(string value)
        {
            authorization = value;
        }

        public DisplayAppointmentModel GetSelectedAppointment()
        {
            return model;
        }

        public void SetSelectedAppointment(DisplayAppointmentModel value)
        {
            model = value;
        }

        public DataHelper()
        {
            Clinic = new ClinicModel();
            SecureStorage = Resolver.Resolve<ISecureStorage>();
        }

        public static DataHelper GetInstance()
        {
            if (me == null)
            {
                me = new DataHelper();

            }
            return me;
        }
    }
}
