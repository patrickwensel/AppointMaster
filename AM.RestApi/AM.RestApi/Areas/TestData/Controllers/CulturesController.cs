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
    public class CulturesController : Controller
    {
        private VetDataContext db = new VetDataContext();

        // GET: TestData/Cultures
        public ActionResult Index()
        {
            return View(db.Cultures.ToList());
        }

        // GET: TestData/Cultures/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culture culture = db.Cultures.Find(id);
            if (culture == null)
            {
                return HttpNotFound();
            }
            return View(culture);
        }

        // GET: TestData/Cultures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestData/Cultures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CultureCode,CountryCode,LanguageCode")] Culture culture)
        {
            if (ModelState.IsValid)
            {
                db.Cultures.Add(culture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(culture);
        }

        // GET: TestData/Cultures/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culture culture = db.Cultures.Find(id);
            if (culture == null)
            {
                return HttpNotFound();
            }
            return View(culture);
        }

        // POST: TestData/Cultures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CultureCode,CountryCode,LanguageCode")] Culture culture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(culture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(culture);
        }

        // GET: TestData/Cultures/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culture culture = db.Cultures.Find(id);
            if (culture == null)
            {
                return HttpNotFound();
            }
            return View(culture);
        }

        // POST: TestData/Cultures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Culture culture = db.Cultures.Find(id);
            db.Cultures.Remove(culture);
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
