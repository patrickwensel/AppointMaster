using System;
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
        const int pageSize = 100;

        [HttpPost]
        public async Task<object> GetReferralsByAccountID(int page)
        {
            string jsonAccountID = await Request.Content.ReadAsStringAsync();

            string accountIDString = JsonConvert.DeserializeObject<string>(jsonAccountID);

            int accountID;
            if (!int.TryParse(accountIDString, out accountID))
            {
                return -1;
            }

            int databaseNumber = 10000 + accountID;

            DentalContext context = new DentalContext();
            context.Database.CommandTimeout = 1000;

            List<ReferralReturnData> referralReturnDatas = context.People.Where(p => p.DataBaseNumber == databaseNumber && (p.referedById != "0" || p.responsiblePartyId != "0"))
                .OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize).Select(p => new ReferralReturnData()
                 {
                     AccountID = p.DataBaseNumber - 10000,
                     PersonID = p.ID,
                     ReferredByID = p.referedById,
                     ResponsiblePartyId = p.responsiblePartyId,
                     TreatmentIds = context.Treatments.Where(x => x.DataBaseNumber == databaseNumber && x.patientId == p.ID && x.status == 2).Select(x => x.ID).ToList()
                 }).ToList();

            return referralReturnDatas;
        }

        [HttpPost]
        public async Task<object> GetTreatmentByIds()
        {
            string jsonTreatmentId = await Request.Content.ReadAsStringAsync();

            List<string> treatmentIds = JsonConvert.DeserializeObject<List<string>>(jsonTreatmentId);

            using (DentalContext context = new DentalContext())
            {
                List<LpiDentalProcedure> lpiDentalProcedures = context.Treatments.Where(x => treatmentIds.Contains(x.ID)).Select(x => new LpiDentalProcedure()
                {
                    amount = (double)x.amount,
                    apptId = "0",
                    code = x.stdTrtId,
                    dateTime = x.date,
                    DB = x.DataBaseNumber - 10000,
                    ID = x.ID,
                    patientId = x.patientId,
                    TreatmentPlan = true
                }).ToList();

                return lpiDentalProcedures;
            }
        }
    }
}

