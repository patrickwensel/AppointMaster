using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AM.VetData.Data;

namespace AM.RestApi.Areas.TestData.Controllers
{
    public class ClinicSpeciesController : Controller
    {
        private VetDataContext db = new VetDataContext();

        // GET: TestData/ClinicSpecies
        public ActionResult Index()
        {
            var clinicSpeciess = db.ClinicSpeciess.Include(c => c.Clinic).Include(c => c.Species);
            return View(clinicSpeciess.ToList());
        }

        // GET: TestData/ClinicSpecies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClinicSpecies clinicSpecies = db.ClinicSpeciess.Find(id);
            if (clinicSpecies == null)
            {
                return HttpNotFound();
            }
            return View(clinicSpecies);
        }

        // GET: TestData/ClinicSpecies/Create
        public ActionResult Create()
        {
            ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name");
            ViewBag.SpeciesID = new SelectList(db.Speciess, "ID", "Name");
            return View();
        }

        // POST: TestData/ClinicSpecies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClinicID,SpeciesID,PrimaryDisplay")] ClinicSpecies clinicSpecies)
        {
            if (ModelState.IsValid)
            {
                db.ClinicSpeciess.Add(clinicSpecies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name", clinicSpecies.ClinicID);
            ViewBag.SpeciesID = new SelectList(db.Speciess, "ID", "Name", clinicSpecies.SpeciesID);
            return View(clinicSpecies);
        }

        // GET: TestData/ClinicSpecies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClinicSpecies clinicSpecies = db.ClinicSpeciess.Find(id);
            if (clinicSpecies == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name", clinicSpecies.ClinicID);
            ViewBag.SpeciesID = new SelectList(db.Speciess, "ID", "Name", clinicSpecies.SpeciesID);
            return View(clinicSpecies);
        }

        // POST: TestData/ClinicSpecies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClinicID,SpeciesID,PrimaryDisplay")] ClinicSpecies clinicSpecies)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clinicSpecies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name", clinicSpecies.ClinicID);
            ViewBag.SpeciesID = new SelectList(db.Speciess, "ID", "Name", clinicSpecies.SpeciesID);
            return View(clinicSpecies);
        }

        // GET: TestData/ClinicSpecies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClinicSpecies clinicSpecies = db.ClinicSpeciess.Find(id);
            if (clinicSpecies == null)
            {
                return HttpNotFound();
            }
            return View(clinicSpecies);
        }

        // POST: TestData/ClinicSpecies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClinicSpecies clinicSpecies = db.ClinicSpeciess.Find(id);
            db.ClinicSpeciess.Remove(clinicSpecies);
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
