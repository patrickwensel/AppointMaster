using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AM.RestApi.Areas.TestData.Controllers
{
    public class TestDataController : Controller
    {
        // GET: TestData/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}