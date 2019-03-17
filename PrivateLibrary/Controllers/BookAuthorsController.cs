using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class BookAuthorsController : ApiController
    {
        private PrivateLibraryEntities1 db = new PrivateLibraryEntities1();

        // GET: api/BookAuthors
        public IQueryable<BookAuthor> GetBookAuthor()
        {
            return db.BookAuthor;
        }

        // GET: api/BookAuthors/5
        [ResponseType(typeof(BookAuthor))]
        public IHttpActionResult GetBookAuthor(int id)
        {
            BookAuthor bookAuthor = db.BookAuthor.Find(id);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return Ok(bookAuthor);
        }

        // PUT: api/BookAuthors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBookAuthor(int id, BookAuthor bookAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookAuthor.BookAuthor_Id)
            {
                return BadRequest();
            }

            db.Entry(bookAuthor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookAuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BookAuthors
        [ResponseType(typeof(BookAuthor))]
        public IHttpActionResult PostBookAuthor(BookAuthor bookAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BookAuthor.Add(bookAuthor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bookAuthor.BookAuthor_Id }, bookAuthor);
        }

        // DELETE: api/BookAuthors/5
        [ResponseType(typeof(BookAuthor))]
        public IHttpActionResult DeleteBookAuthor(int id)
        {
            BookAuthor bookAuthor = db.BookAuthor.Find(id);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            db.BookAuthor.Remove(bookAuthor);
            db.SaveChanges();

            return Ok(bookAuthor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookAuthorExists(int id)
        {
            return db.BookAuthor.Count(e => e.BookAuthor_Id == id) > 0;
        }
    }
}