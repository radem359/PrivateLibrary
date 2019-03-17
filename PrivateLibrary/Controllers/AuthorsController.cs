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
    public class AuthorsController : Controller
    {
        private PrivateLibraryEntities1 db = new PrivateLibraryEntities1();

        // GET: Authors
        public ActionResult Index()
        {
            var author = db.Author.Include(a => a.Nationality);
            return View(author.ToList());
        }

        // GET: Authors/Details/5
        [Filters.UserAuthorAuth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Author.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        [Filters.AdminAuthorAuth]
        public ActionResult Create()
        {
            ViewBag.Nationality_Id = new SelectList(db.Nationality, "Nationality_Id", "Nationality_Name");
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Author_Id,Author_Name,Nationality_Id")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Author.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Nationality_Id = new SelectList(db.Nationality, "Nationality_Id", "Nationality_Name", author.Nationality_Id);
            return View(author);
        }

        // GET: Authors/Edit/5
        [Filters.AdminAuthorAuth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Author.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nationality_Id = new SelectList(db.Nationality, "Nationality_Id", "Nationality_Name", author.Nationality_Id);
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Author_Id,Author_Name,Nationality_Id")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Nationality_Id = new SelectList(db.Nationality, "Nationality_Id", "Nationality_Name", author.Nationality_Id);
            return View(author);
        }

        // GET: Authors/Delete/5
        [Filters.AdminAuthorAuth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Author.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Author.Find(id);
            db.Author.Remove(author);
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
