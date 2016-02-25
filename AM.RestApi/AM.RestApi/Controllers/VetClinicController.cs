using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using AM.RestApi.Identities;
using AM.RestApi.Model;
using AM.VetData.Data;
using Newtonsoft.Json;

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
            Clinic clinic = context.Clinics.FirstOrDefault(c => c.ID == clinicID);


			return clinic;
		}

		[Route("~/api/v1/VetClinic/Authorize")]
		[HttpGet]
		public void Authorize()
		{

		}

	}
}
