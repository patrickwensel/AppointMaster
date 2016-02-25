using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AM.RestApi.Areas.TestData.Controllers
{
    public class ClinicController : Controller
    {
        // GET: TestData/Clinic
        public ActionResult Index()
        {
            return View();
        }
    }
}