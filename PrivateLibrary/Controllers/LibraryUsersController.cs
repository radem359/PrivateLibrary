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
    public class LibraryUsersController : Controller
    {
        private PrivateLibraryEntities1 db = new PrivateLibraryEntities1();

        // GET: LibraryUsers
        [Filters.AdminUserAuthorization]
        public ActionResult Index()
        {
            return View(db.LibraryUser.ToList());
        }

        // GET: LibraryUsers/Details
        [Filters.UserLibraryUserAuth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUser libraryUser = db.LibraryUser.Find(id);
            if (libraryUser == null)
            {
                return HttpNotFound();
            }
            return View(libraryUser);
        }

        // GET: LibraryUsers/Create
        [Filters.AdminUserAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibraryUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Id,Username,Password,Admin")] LibraryUser libraryUser)
        {
            if (ModelState.IsValid)
            {
                db.LibraryUser.Add(libraryUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(libraryUser);
        }

        // GET: LibraryUsers/Edit/5
        [Filters.UserLibraryUserAuth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUser libraryUser = db.LibraryUser.Find(id);
            if (libraryUser == null)
            {
                return HttpNotFound();
            }
            return View(libraryUser);
        }

        // POST: LibraryUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Id,Username,Password,Admin")] LibraryUser libraryUser)
        {
            if (libraryUser.Admin == true)
            {
                libraryUser.BookCorrect = null;
            }
            if (ModelState.IsValid)
            {
                db.Entry(libraryUser).State = EntityState.Modified;
                db.SaveChanges();
                if (Session["Admin"].ToString().Equals("1"))
                {
                    return RedirectToAction("Index");
                }else
                {
                    return RedirectToAction("Details", new { id = libraryUser.User_Id });
                }
            }
            return View(libraryUser);
        }

        // GET: LibraryUsers/Delete/5
        [Filters.AdminUserAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUser libraryUser = db.LibraryUser.Find(id);
            if (libraryUser == null)
            {
                return HttpNotFound();
            }
            return View(libraryUser);
        }

        // POST: LibraryUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LibraryUser libraryUser = db.LibraryUser.Find(id);
            libraryUser.BookCorrect = null;
            foreach (BookCorrect bookCorrect in db.BookCorrect)
            {
                if(bookCorrect.User_Id == libraryUser.User_Id)
                {
                    bookCorrect.User_Id = null;
                }
            }
            db.LibraryUser.Remove(libraryUser);
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
