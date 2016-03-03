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
	public class AppointmentPatientsController : Controller
	{
		private VetDataContext db = new VetDataContext();

		// GET: TestData/AppointmentPatients
		public ActionResult Index()
		{
			var appointmentPatients = db.AppointmentPatients.Include(a => a.Appointment).Include(a => a.Patient);
			return View(appointmentPatients.ToList());
		}

		// GET: TestData/AppointmentPatients/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			AppointmentPatient appointmentPatient = db.AppointmentPatients.Find(id);
			if (appointmentPatient == null)
			{
				return HttpNotFound();
			}
			return View(appointmentPatient);
		}

		// GET: TestData/AppointmentPatients/Create
		public ActionResult Create()
		{
			ViewBag.AppointmentID = new SelectList((from s in db.Appointments
													select new
													{
														ID = s.ID,
														FullName = s.Client.FirstName + " " + s.Client.LastName + " - " + s.Time.ToString()
													}),
				"ID",
				"FullName",
				null);

			ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name");
			return View();
		}

		// POST: TestData/AppointmentPatients/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,AppointmentID,PatientID")] AppointmentPatient appointmentPatient)
		{
			if (ModelState.IsValid)
			{
				db.AppointmentPatients.Add(appointmentPatient);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.AppointmentID = new SelectList((from s in db.Appointments
													select new
													{
														ID = s.ID,
														FullName = s.Client.FirstName + " " + s.Client.LastName + " - " + s.Time.ToString()
													}),
				"ID",
				"FullName",
				appointmentPatient.AppointmentID);

			ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", appointmentPatient.PatientID);
			return View(appointmentPatient);
		}

		// GET: TestData/AppointmentPatients/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			AppointmentPatient appointmentPatient = db.AppointmentPatients.Find(id);
			if (appointmentPatient == null)
			{
				return HttpNotFound();
			}
			ViewBag.AppointmentID = new SelectList((from s in db.Appointments
				select new
				{
					ID = s.ID,
					FullName = s.Client.FirstName + " " + s.Client.LastName + " - " + s.Time.ToString()
				}),
				"ID",
				"FullName",
				appointmentPatient.AppointmentID);
			ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", appointmentPatient.PatientID);
			return View(appointmentPatient);
		}

		// POST: TestData/AppointmentPatients/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,AppointmentID,PatientID")] AppointmentPatient appointmentPatient)
		{
			if (ModelState.IsValid)
			{
				db.Entry(appointmentPatient).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.AppointmentID = new SelectList((from s in db.Appointments
				select new
				{
					ID = s.ID,
					FullName = s.Client.FirstName + " " + s.Client.LastName + " - " + s.Time.ToString()
				}),
				"ID",
				"FullName",
				appointmentPatient.AppointmentID);
			ViewBag.PatientID = new SelectList(db.Patients, "ID", "Name", appointmentPatient.PatientID);
			return View(appointmentPatient);
		}

		// GET: TestData/AppointmentPatients/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			AppointmentPatient appointmentPatient = db.AppointmentPatients.Find(id);
			if (appointmentPatient == null)
			{
				return HttpNotFound();
			}
			return View(appointmentPatient);
		}

		// POST: TestData/AppointmentPatients/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			AppointmentPatient appointmentPatient = db.AppointmentPatients.Find(id);
			db.AppointmentPatients.Remove(appointmentPatient);
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
