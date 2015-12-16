using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dental.API.Models
{
    public class LpiDentalProcedure
    {
        public int DB;
        public string ID;
        public string patientId;
        public string apptId;
        public string code;
        public bool TreatmentPlan;
        public DateTime dateTime;
        public double amount;
    }
}