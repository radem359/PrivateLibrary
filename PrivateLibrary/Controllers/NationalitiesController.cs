using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class NationalitiesController : Controller
    {
        private PrivateLibraryEntities1 db = new PrivateLibraryEntities1();

        // GET: Nationalities
        public ActionResult Index()
        {
            return View(db.Nationality.ToList());
        }

        // GET: Nationalities/Details/5
        [Filters.UserNationalityAuth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = db.Nationality.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        // GET: Nationalities/Create
        [Filters.AdminNationalityAuth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nationalities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nationality_Id,Nationality_Name")] Nationality nationality)
        {
            if (ModelState.IsValid)
            {
                db.Nationality.Add(nationality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nationality);
        }

        // GET: Nationalities/Edit/5
        [Filters.AdminNationalityAuth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = db.Nationality.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        // POST: Nationalities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nationality_Id,Nationality_Name")] Nationality nationality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nationality).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nationality);
        }

        // GET: Nationalities/Delete/5
        [Filters.AdminNationalityAuth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = db.Nationality.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        // POST: Nationalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nationality nationality = db.Nationality.Find(id);
            db.Nationality.Remove(nationality);
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
