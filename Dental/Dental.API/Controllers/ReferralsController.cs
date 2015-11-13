﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Dental.DataLayer.Data;
using Dental.API.Models;

namespace Dental.API.Controllers
{
    public class ReferralsController : ApiController
    {
        [HttpPost]
        public async Task<object> GetReferredByPatientID()
        {
            string jsonPersontID = await Request.Content.ReadAsStringAsync();

            string personID = JsonConvert.DeserializeObject<string>(jsonPersontID);

            DentalContext context = new DentalContext();

            var person = context.People.FirstOrDefault(x => x.ID == personID);
            if (person != null)
            {
                return person.referedById;
            }

            return -1;
        }

        [HttpPost]
        public async Task<object> GetTreatmentWithStatus2ByPatientId()
        {
            string jsonPersontID = await Request.Content.ReadAsStringAsync();

            string personID = JsonConvert.DeserializeObject<string>(jsonPersontID);

            DentalContext context = new DentalContext();

            var treatment = context.Treatments.Where(x => x.patientId == personID && x.status == 2).ToList();
            List<LpiDentalProcedure> lpiDentalProcedure = new List<LpiDentalProcedure>();
            foreach (var item in treatment)
            {
                lpiDentalProcedure.Add(new LpiDentalProcedure()
                {
                    amount = (double)item.amount,
                    apptId = "0",//? Grish1nds TEST
                    code = item.stdTrtId,
                    dateTime = item.date,
                    DB = item.DataBaseNumber,
                    ID = item.ID,
                    patientId = item.patientId,
                    TreatmentPlan = false//? Grish1nds TEST
                });
            }
            return lpiDentalProcedure;
        }

        //public int Test()
        //{
        //    //IEnumerable<string> headerValues = request.Headers.GetValues("x");
        //    //var id = headerValues.FirstOrDefault();

        //    return 27;
        //}






        ////Old Code
        //[HttpPost]
        //public async Task<object> GetReferralsByAccountID()
        //{
        //    string jsonAccountID = await Request.Content.ReadAsStringAsync();

        //    string accountID =JsonConvert.DeserializeObject<string>(jsonAccountID);

        //    DentalContext context =new DentalContext();

        //    var person = context.People.FirstOrDefault(x => x.ID == accountID);
        //    if (person != null)
        //    {
        //        return person.referedById;
        //    }

        //    return -1;
        //}


    }
}

