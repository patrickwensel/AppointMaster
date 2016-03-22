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
    public class AppointmentsController : Controller
    {
        private VetDataContext db = new VetDataContext();

        // GET: TestData/Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Client).Include(a => a.Clinic);
            return View(appointments.ToList());
        }

        // GET: TestData/Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: TestData/Appointments/Create
        public ActionResult Create()
        {
			ViewBag.ClientID = new SelectList((from s in db.Clients
											   select new
											   {
												   ID = s.ID,
												   FullName = s.FirstName + " " + s.LastName
											   }),
				"ID",
				"FullName",
				null);
			ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name");
            return View();
        }

        // POST: TestData/Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClinicID,ClientID,Time,CheckedIn,Code")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

			ViewBag.ClientID = new SelectList((from s in db.Clients
											   select new
											   {
												   ID = s.ID,
												   FullName = s.FirstName + " " + s.LastName
											   }),
				"ID",
				"FullName",
				appointment.ClientID);
			ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name", appointment.ClinicID);
            return View(appointment);
        }

        // GET: TestData/Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

	        ViewBag.ClientID = new SelectList((from s in db.Clients
											 select new
		        {
			        ID = s.ID,
			        FullName = s.FirstName + " " + s.LastName
		        }),
		        "ID",
		        "FullName",
				appointment.ClientID);

			//ViewBag.ClientID = new SelectList(db.Clients, "ID", "FirstName" + "LastName", appointment.ClientID);
            ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name", appointment.ClinicID);
            return View(appointment);
        }

        // POST: TestData/Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClinicID,ClientID,Time,CheckedIn,Code")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
			ViewBag.ClientID = new SelectList((from s in db.Clients
											   select new
											   {
												   ID = s.ID,
												   FullName = s.FirstName + " " + s.LastName
											   }),
				"ID",
				"FullName",
				appointment.ClientID);
			ViewBag.ClinicID = new SelectList(db.Clinics, "ID", "Name", appointment.ClinicID);
            return View(appointment);
        }

        // GET: TestData/Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: TestData/Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
