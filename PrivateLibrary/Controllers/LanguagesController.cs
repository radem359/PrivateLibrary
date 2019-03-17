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
    public class LanguagesController : Controller
    {
        private PrivateLibraryEntities1 db = new PrivateLibraryEntities1();

        // GET: Languages
        public ActionResult Index()
        {
            return View(db.Language.ToList());
        }

        // GET: Languages/Details/5
        [Filters.UserLanguageAuth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Language.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // GET: Languages/Create
        [Filters.AdminLanguageAuth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Language_Id,Language_Name")] Language language)
        {
            if (ModelState.IsValid)
            {
                db.Language.Add(language);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(language);
        }

        // GET: Languages/Edit/5
        [Filters.AdminLanguageAuth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Language.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Language_Id,Language_Name")] Language language)
        {
            if (ModelState.IsValid)
            {
                db.Entry(language).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(language);
        }

        // GET: Languages/Delete/5
        [Filters.AdminLanguageAuth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Language.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Language language = db.Language.Find(id);
            db.Language.Remove(language);
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
