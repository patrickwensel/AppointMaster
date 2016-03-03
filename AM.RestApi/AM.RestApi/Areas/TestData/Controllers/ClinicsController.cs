using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AM.VetData.Data;
using System.IO;

namespace AM.RestApi.Areas.TestData.Controllers
{
    public class ClinicsController : Controller
    {
        private VetDataContext db = new VetDataContext();

        // GET: TestData/Clinics
        public ActionResult Index()
        {
            var clinics = db.Clinics.Include(c => c.Culture);
            return View(clinics.ToList());
        }

        // GET: TestData/Clinics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // GET: TestData/Clinics/Create
        public ActionResult Create()
        {
            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode");
            return View();
        }

        // POST: TestData/Clinics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AccountID,Name,Address1,Address2,City,StateProvince,PostalCode,WebAddress,EmailAddress,Phone,Fax,Logo,DefaultCultureCode,PrimaryColor,SecondaryColor")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var files = Request.Files["logoImage"];
                        MemoryStream target = new MemoryStream();
                        files.InputStream.CopyTo(target);
                        clinic.Logo = target.ToArray();
                    }
                }
                catch
                {

                }

                db.Clinics.Add(clinic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode", clinic.DefaultCultureCode);
            return View(clinic);
        }

        public ActionResult GetLogo(byte[] logo)
        {
            return File(logo, "image/png");
        }

        // GET: TestData/Clinics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode", clinic.DefaultCultureCode);
            return View(clinic);
        }

        // POST: TestData/Clinics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AccountID,Name,Address1,Address2,City,StateProvince,PostalCode,WebAddress,EmailAddress,Phone,Fax,Logo,DefaultCultureCode,PrimaryColor,SecondaryColor")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var files = Request.Files["logoImage"];
                        MemoryStream target = new MemoryStream();
                        files.InputStream.CopyTo(target);
                        clinic.Logo = target.ToArray();
                    }
                }
                catch
                {

                }
                db.Entry(clinic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode", clinic.DefaultCultureCode);
            return View(clinic);
        }

        // GET: TestData/Clinics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // POST: TestData/Clinics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clinic clinic = db.Clinics.Find(id);
            db.Clinics.Remove(clinic);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
