using System.Linq;
using System.Net.Http;
using System.Web.Http;
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
		public string Get()
		{

			VetDataContext context = new VetDataContext();

			Clinic clinic = context.Clinics.FirstOrDefault(c => c.ID == 1);

			var x = JsonConvert.SerializeObject(clinic, Formatting.Indented,
				new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});

			return x;
		}

		[Route("~/api/v1/VetClinic/Authorize")]
		[HttpGet]
		public void Authorize()
		{

		}

	}
}
