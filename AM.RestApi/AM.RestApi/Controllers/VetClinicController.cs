using System.Linq;
using System.Web.Http;
using AM.RestApi.Identities;
using AM.RestApi.Model;
using AM.VetData.Data;

namespace AM.RestApi.Controllers
{
    [RoutePrefix("api/v1/VetClinic")]
    public class VetClinicController : ApiController
    {
        // GET: api/VetClinic
        [Route("")]
        public object Get()
        {
            VetDataContext context = new VetDataContext();

            int clinicID = (User as ClinicPrincipal).ClinicID;
            var clinic = context.Clinics.Where(x => x.ID == clinicID).Select(c => new ClinicViewModel
            {
                ID = c.ID,
                Name = c.Name,
                Address1 = c.Address1,
                Address2 = c.Address2,
                City = c.City,
                StateProvince = c.StateProvince,
                PostalCode = c.PostalCode,
                WebAddress = c.WebAddress,
                Phone = c.Phone,
                Email = c.EmailAddress,
                Fax = c.Fax,
                Logo = c.Logo,
                DefaultCulture = c.DefaultCultureCode,
                PrimaryColor = c.PrimaryColor,
                SecondaryColor = c.SecondaryColor,
                SpeciesSupported=c.ClinicSpecies.Select(x=> new SpeciesViewModel
                {
	                ID = x.ID,
					ClinicID = x.ClinicID,
					SpeciesID = x.SpeciesID,
					Logo = x.Logo,
					PrimaryDisplay = x.PrimaryDisplay
                }).ToList()

            }).FirstOrDefault();

            return clinic;
        }
		
		[Route("~/api/v1/VetClinic/Authorize")]
        [HttpGet]
        public void Authorize()
        {

        }
    }
}
