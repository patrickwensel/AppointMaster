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
    public class ClientsController : Controller
    {
        private VetDataContext db = new VetDataContext();

        // GET: TestData/Clients
        public ActionResult Index()
        {
            var clients = db.Clients.Include(c => c.Culture);
            return View(clients.ToList());
        }

        // GET: TestData/Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: TestData/Clients/Create
        public ActionResult Create()
        {
            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode");
            return View();
        }

        // POST: TestData/Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,FirstName,LastName,Address,City,StateProvince,PostalCode,Phone,Email,DefaultCultureCode")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode", client.DefaultCultureCode);
            return View(client);
        }

        // GET: TestData/Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode", client.DefaultCultureCode);
            return View(client);
        }

        // POST: TestData/Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,FirstName,LastName,Address,City,StateProvince,PostalCode,Phone,Email,DefaultCultureCode")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DefaultCultureCode = new SelectList(db.Cultures, "CultureCode", "CountryCode", client.DefaultCultureCode);
            return View(client);
        }

        // GET: TestData/Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: TestData/Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
