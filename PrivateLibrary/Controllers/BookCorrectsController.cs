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
    public class BookCorrectsController : Controller
    {
        private PrivateLibraryEntities1 db = new PrivateLibraryEntities1();

        // GET: BookCorrects
        public ActionResult Index()
        {
            var bookCorrect = db.BookCorrect.Include(b => b.LibraryUser);
            return View(bookCorrect.ToList());
        }

        // GET: BookCorrects/Details/5
        [Filters.UserBookAuthorization]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCorrect bookCorrect = db.BookCorrect.Find(id);
            if (bookCorrect == null)
            {
                return HttpNotFound();
            }

            return View(bookCorrect);
        }

        private void FillAuthors()
        {
            List<SelectListItem> authorsSelection = new List<SelectListItem>();
            foreach (Author author in db.Author)
            {
                authorsSelection.Add(new SelectListItem { Text = author.Author_Name });
            }
            ViewBag.Authors = authorsSelection;
        }

        private void FillGenres()
        {
            List<SelectListItem> genresSelection = new List<SelectListItem>();
            foreach (Genre genre in db.Genre)
            {
                genresSelection.Add(new SelectListItem { Text = genre.Genre_Name });
            }
            ViewBag.Genres = genresSelection;
        }

        private void FillLanguages()
        {
            List<SelectListItem> languagesSelection = new List<SelectListItem>();
            foreach (Language language in db.Language)
            {
                languagesSelection.Add(new SelectListItem { Text = language.Language_Name });
            }
            ViewBag.Language = languagesSelection;
        }

        // GET: BookCorrects/Create
        [Filters.AdminBookAuthorization]
        public ActionResult Create()
        {
            List<LibraryUser> libraryUser = new List<LibraryUser>();
            libraryUser.Add(null);
            foreach(var item in db.LibraryUser)
            {
                if (!item.Admin)
                {
                    libraryUser.Add(item);
                }
            }
            ViewBag.User_Id = new SelectList(libraryUser , "User_Id", "Username");
            FillAuthors();
            FillGenres();
            FillLanguages();
            return View();
        }

        // POST: BookCorrects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Book_Id,Book_Name,ISBN_Number,Book_Desription,User_Id")] BookCorrect bookCorrect)
        {
            bookCorrect.LibraryUser = null;
            if (ModelState.IsValid)
            {
                if(bookCorrect.ISBN_Number != null && bookCorrect.ISBN_Number.All(Char.IsLetter))
                {
                    ModelState.AddModelError("", "ISBN is not valid. No letters in ISBN number");
                    return View(bookCorrect);
                }
                

                if (Request.Form["ChoosenAuthors"] != null) {
                    string choosenAuthors = Request.Form["ChoosenAuthors"].ToString();
                    string[] authorsNames = choosenAuthors.Split(',');
                    BookAuthor bookAuthor = new BookAuthor();
                    foreach (var item in db.Author)
                    {
                        for (int i = 0; i < authorsNames.Length; i++)
                        {
                            if (authorsNames[i].Equals(item.Author_Name)) { 
                                bookAuthor.Author = item;
                                bookAuthor.Author_Id = item.Author_Id;
                                bookAuthor.BookCorrect = bookCorrect;
                                bookAuthor.Book_Id = bookCorrect.Book_Id;
                                db.BookAuthor.Add(bookAuthor);
                                bookCorrect.BookAuthor.Add(bookAuthor);
                            }
                        }
                    }
                }
                if (Request.Form["ChoosenGenres"] != null) {
                    string choosenGenres = Request.Form["ChoosenGenres"].ToString();
                    string[] genreNames = choosenGenres.Split(',');
                    BookGenre bookGenre = new BookGenre();
                    foreach (var item in db.Genre)
                    {
                        for (int i = 0; i < genreNames.Length; i++)
                        {
                            if (genreNames[i].Equals(item.Genre_Name))
                            {
                                bookGenre.Genre = item;
                                bookGenre.BookCorrect = bookCorrect;
                                bookGenre.Genre_Id = item.Genre_Id;
                                bookGenre.Book_Id = bookCorrect.Book_Id;
                                db.BookGenre.Add(bookGenre);
                                bookCorrect.BookGenre.Add(bookGenre);
                            }
                        }
                    }
                }

                if (Request.Form["Language"] != null) {
                    string nameOfLanguage = Request.Form["Language"].ToString();
                    BookLanguage bookLanguage = new BookLanguage();
                    foreach (var item in db.Language)
                    {
                        if (nameOfLanguage.Equals(item.Language_Name))
                        {
                            bookLanguage.BookCorrect = bookCorrect;
                            bookLanguage.Language = item;
                            bookLanguage.Language_Id = item.Language_Id;
                            bookLanguage.Book_Id = bookCorrect.Book_Id;
                            db.BookLanguage.Add(bookLanguage);
                            bookCorrect.BookLanguage.Add(bookLanguage);
                        }
                    }
                }
                db.BookCorrect.Add(bookCorrect);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_Id = new SelectList(db.LibraryUser, "User_Id", "Username", bookCorrect.User_Id);
            return View(bookCorrect);
        }

        private bool ContainsAuthor(int authorId, List<BookAuthor> authors)
        {
            foreach (BookAuthor bookAuthor in authors)
            {
                if(bookAuthor.Author_Id == authorId)
                {
                    return true;
                }
            }
            return false;
        }

        private void FillChoosenAuthors(BookCorrect book)
        {
            List<SelectListItem> choosen = new List<SelectListItem>();
            List<SelectListItem> authors = new List<SelectListItem>();
            List<Author> allAuthors = db.Author.ToList();
            List<BookAuthor> authorsFromBook = book.BookAuthor.ToList();
            if (authorsFromBook.Any())
            {
                foreach (BookAuthor choosenAuthor in authorsFromBook)
                {
                    choosen.Add(new SelectListItem { Text = choosenAuthor.Author.Author_Name, Value = choosenAuthor.Author.Author_Name });
                    
                }
                foreach (Author author in allAuthors)
                {
                    if (!ContainsAuthor(author.Author_Id, authorsFromBook))
                    {
                        authors.Add(new SelectListItem { Text = author.Author_Name, Value = author.Author_Name });
                    }
                }
            }
            else
            {
                foreach (Author author in allAuthors)
                {
                    authors.Add(new SelectListItem { Text = author.Author_Name, Value = author.Author_Name });
                }
            }
            ViewBag.Authors = authors;
            ViewBag.ChoosenAuthors = choosen;
        }

        private bool ContainsGenres(int genreId, List<BookGenre> genres)
        {
            foreach (BookGenre bookGenre in genres)
            {
                if (bookGenre.Genre_Id == genreId)
                {
                    return true;
                }
            }
            return false;
        }

        private void FillChoosenGenres(BookCorrect book)
        {
            List<SelectListItem> choosen = new List<SelectListItem>();
            List<SelectListItem> genres = new List<SelectListItem>();
            List<Genre> allGenres = db.Genre.ToList();
            List<BookGenre> genresFromBook = book.BookGenre.ToList();
            if (genresFromBook.Any())
            {
                foreach (BookGenre choosenGenre in genresFromBook)
                {
                    choosen.Add(new SelectListItem { Text = choosenGenre.Genre.Genre_Name, Value = choosenGenre.Genre.Genre_Name });
                    
                }
                foreach (Genre genre in allGenres)
                {
                    if (!ContainsGenres(genre.Genre_Id, genresFromBook))
                    {
                        genres.Add(new SelectListItem { Text = genre.Genre_Name, Value = genre.Genre_Name });
                    }
                }
            }
            else
            {
                foreach (Genre genre in allGenres)
                {
                    genres.Add(new SelectListItem { Text = genre.Genre_Name, Value = genre.Genre_Name });
                }
            }
            ViewBag.Genres = genres;
            ViewBag.ChoosenGenres = choosen;
        }

        private void FillChoosenLanguage(BookCorrect book)
        {
            List<SelectListItem> languagesSelection = new List<SelectListItem>();
            if (book.BookLanguage.Any())
                languagesSelection.Add(new SelectListItem { Text = db.Language.Find(book.BookLanguage.First().Language_Id).Language_Name }); ;
            foreach (Language language in db.Language)
            {
                if(language.Language_Name != db.Language.Find(book.BookLanguage.First().Language_Id).Language_Name)
                    languagesSelection.Add(new SelectListItem { Text = language.Language_Name });
            }
            ViewBag.Language = null;
            ViewBag.Language = languagesSelection;
        }

        // GET: BookCorrects/Edit/5
        [Filters.AdminBookAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCorrect bookCorrect = db.BookCorrect.Find(id);
            if (bookCorrect == null)
            {
                return HttpNotFound();
            }
            List<LibraryUser> libraryUser = new List<LibraryUser>();
            libraryUser.Add(bookCorrect.LibraryUser);
            foreach (var item in db.LibraryUser)
            {
                if (!item.Admin)
                {
                    libraryUser.Add(item);
                }
            }
            ViewBag.User_Id = new SelectList(libraryUser, "User_Id", "Username");
            FillChoosenAuthors(bookCorrect);
            FillChoosenGenres(bookCorrect);
            FillChoosenLanguage(bookCorrect);
            return View(bookCorrect);
        }

        // POST: BookCorrects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Book_Id,Book_Name,ISBN_Number,Book_Desription,User_Id")] BookCorrect bookCorrect)
        {
            if (ModelState.IsValid)
            {
                if (bookCorrect.ISBN_Number.All(Char.IsLetter))
                {
                    ModelState.AddModelError("", "ISBN is not valid. No letters in ISBN number");
                    return View(bookCorrect);
                }
                db.Entry(bookCorrect).State = EntityState.Modified;

                if (Request.Form["ChoosenAuthors"] != null)
                {
                    string choosenAuthors = Request.Form["ChoosenAuthors"].ToString();
                    string[] authorsNames = choosenAuthors.Split(',');
                    List<BookAuthor> authorsNew = new List<BookAuthor>();
                   
                    foreach (var item in db.Author)
                    {
                        BookAuthor bookAuthor = new BookAuthor();
                        for (int i = 0; i < authorsNames.Length; i++)
                        {
                            if (authorsNames[i].Equals(item.Author_Name))
                            {
                                bookAuthor.Author = item;
                                bookAuthor.Author_Id = item.Author_Id;
                                bookAuthor.BookCorrect = bookCorrect;
                                bookAuthor.BookAuthor_Id = bookCorrect.Book_Id;
                                authorsNew.Add(bookAuthor);
                            }
                        }
                    }
                    foreach (BookAuthor bookAuthor in db.BookAuthor)
                    { 
                        if(bookAuthor.Book_Id == bookCorrect.Book_Id)
                        {
                            bookCorrect.BookAuthor.Remove(bookAuthor);
                            db.BookAuthor.Remove(bookAuthor);
                        }
                    }

                    db.SaveChanges();
                    foreach (BookAuthor choosen in authorsNew)
                    {
                        db.BookAuthor.Add(choosen);
                        bookCorrect.BookAuthor.Add(choosen);
                    }
                }else
                {
                    foreach (BookAuthor bookAuthor in db.BookAuthor)
                    {
                        if (bookAuthor.Book_Id == bookCorrect.Book_Id)
                        {
                            bookCorrect.BookAuthor.Remove(bookAuthor);
                            db.BookAuthor.Remove(bookAuthor);
                        }
                    }
                }
                if (Request.Form["ChoosenGenres"] != null)
                {
                    string choosenGenres = Request.Form["ChoosenGenres"].ToString();
                    string[] genreNames = choosenGenres.Split(',');
                    List<BookGenre> genresNew = new List<BookGenre>();
                   
                    foreach (var item in db.Genre)
                    {
                        BookGenre bookGenre = new BookGenre();
                        for (int i = 0; i < genreNames.Length; i++)
                        {
                            if (genreNames[i].Equals(item.Genre_Name))
                            {
                                bookGenre.BookCorrect = bookCorrect;
                                bookGenre.Genre = item;
                                bookGenre.Book_Id = bookCorrect.Book_Id;
                                bookGenre.Genre_Id = item.Genre_Id;
                                genresNew.Add(bookGenre);
                            }
                        }
                    }

                    foreach(BookGenre bookGenre in db.BookGenre)
                    {
                        if(bookGenre.Book_Id == bookCorrect.Book_Id)
                        {
                            bookCorrect.BookGenre.Remove(bookGenre);
                            db.BookGenre.Remove(bookGenre);
                        }
                    }
                    db.SaveChanges();
                    foreach (BookGenre bookGenre in genresNew)
                    {
                        db.BookGenre.Add(bookGenre);
                        bookCorrect.BookGenre.Add(bookGenre);
                    }
                }else
                {
                    foreach (BookGenre bookGenre in db.BookGenre)
                    {
                        if (bookGenre.Book_Id == bookCorrect.Book_Id)
                        {
                            bookCorrect.BookGenre.Remove(bookGenre);
                            db.BookGenre.Remove(bookGenre);
                        }
                    }
                }

                if (Request.Form["Language"] != null)
                {
                    string nameOfLanguage = Request.Form["Language"].ToString();
                    List<BookLanguage> languageNew = new List<BookLanguage>();
                    foreach (var item in db.Language)
                    {
                        BookLanguage bookLanguage = new BookLanguage();
                        if (nameOfLanguage.Equals(item.Language_Name))
                        {
                            bookLanguage.Language = item;
                            bookLanguage.Book_Id = bookCorrect.Book_Id;
                            bookLanguage.BookCorrect = bookCorrect;
                            bookLanguage.Language_Id = item.Language_Id;
                            languageNew.Add(bookLanguage);
                            
                        }
                    }

                    foreach (BookLanguage bookLanguage in db.BookLanguage)
                    {
                        if (bookLanguage.Book_Id == bookCorrect.Book_Id)
                            db.BookLanguage.Remove(bookLanguage);                        
                    }
                    db.SaveChanges();
                    foreach (BookLanguage bookLanguage in languageNew)
                    {
                        db.BookLanguage.Add(bookLanguage);
                        bookCorrect.BookLanguage.Add(bookLanguage);
                    }
                }else
                {
                    foreach (BookLanguage bookLanguage in db.BookLanguage)
                    {
                        if (bookLanguage.Book_Id == bookCorrect.Book_Id)
                            db.BookLanguage.Remove(bookLanguage);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Id = new SelectList(db.LibraryUser, "User_Id", "Username", bookCorrect.User_Id);
            return View(bookCorrect);
        }

        // GET: BookCorrects/Delete/5
        [Filters.AdminBookAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCorrect bookCorrect = db.BookCorrect.Find(id);
            if (bookCorrect == null)
            {
                return HttpNotFound();
            }
            return View(bookCorrect);
        }

        // POST: BookCorrects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookCorrect bookCorrect = db.BookCorrect.Find(id);
            db.BookCorrect.Remove(bookCorrect);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Rent(int Book_Id)
        {
            int userId = (int)Session["UserId"];
            BookCorrect book = new BookCorrect();
            LibraryUser libraryUser = new LibraryUser();

            
            foreach (var item in db.LibraryUser)
            {
                if (item.User_Id == userId)
                { 
                    libraryUser = item;
                    break;
                }
            }
            foreach (var item in db.BookCorrect)
            {
                if (item.Book_Id == Book_Id)
                {
                    book = item;
                    break;
                }
                    
            }

            book.User_Id = userId;
            book.LibraryUser = libraryUser;
            libraryUser.BookCorrect.Add(book);
            db.SaveChanges();
            return RedirectToAction("Index", "BookCorrects");
        }

        [HttpPost]
        public ActionResult Return(int Book_Id)
        {
            int userId = (int)Session["UserId"];
            BookCorrect book = new BookCorrect();
            LibraryUser libraryUser = new LibraryUser();


            foreach (var item in db.LibraryUser)
            {
                if (item.User_Id == userId)
                {
                    libraryUser = item;
                    break;
                }
            }
            foreach (var item in db.BookCorrect)
            {
                if (item.Book_Id == Book_Id)
                {
                    book = item;
                    break;
                }

            }

            book.User_Id = null;
            book.LibraryUser = null;
            libraryUser.BookCorrect.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index", "BookCorrects");
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
