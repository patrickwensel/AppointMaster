﻿using AppointMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Services
{
    public class DataHelper
    {
        static DataHelper me;

        string authorization;
        DisplayAppointmentModel model;

        public string BaseAPI { get; set; }

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
