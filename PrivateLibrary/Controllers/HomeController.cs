using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class HomeController : Controller
    {

        private PrivateLibraryEntities1 db = new PrivateLibraryEntities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            Session["Admin"] = "0";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LibraryUser model)
        {
            if (ModelState.IsValid)
            {
                var details = (from userlist in db.LibraryUser
                               where userlist.Username == model.Username && userlist.Password == model.Password
                               select new
                               {
                                   userlist.User_Id,
                                   userlist.Username,
                                   userlist.Admin,
                                   userlist.BookCorrect
                               }).ToList();
                if (details.FirstOrDefault() != null)
                {
                    Session["UserId"] = details.FirstOrDefault().User_Id;
                    Session["Username"] = details.FirstOrDefault().Username;
                    if (details.FirstOrDefault().Admin)
                    {
                        Session["Admin"] = "1";
                        return RedirectToAction("Index", "LibraryUsers");
                    }
                    else
                    {
                        Session["Admin"] = "2";
                        return RedirectToAction("Details", "LibraryUsers", new {@id = details.FirstOrDefault().User_Id });
                    }
                }
            }
            return View(model);
        }


        public ActionResult Welcome()
        {
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationViewModel model)
        {
            LibraryUser user = new LibraryUser { Username = model.Username, Admin = false, Password = model.Password, BookCorrect = null };

            foreach (LibraryUser u in db.LibraryUser.ToList())
            {
                if (u.Username.Equals(model.Username))
                {
                    ModelState.AddModelError("", "User with that username already exists.");
                    return View(model);
                }
            }

            if (ModelState.IsValid)
            {
                if (model.Password.Length < 6)
                {
                    ModelState.AddModelError("", "Password must have at least 6 characters");
                    return View(model);
                }

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "The password and confirmation password do not match.");
                    return View(model);
                }
                db.LibraryUser.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Userdetails()
        {
            return RedirectToAction("Welcome", "Home");
        }

        public ActionResult Logoff()
        {
            Session["UserId"] = null;
            Session["Username"] = null;
            Session["Admin"] = "0";
            return RedirectToAction("Index", "BookCorrects");
        }

        }
}