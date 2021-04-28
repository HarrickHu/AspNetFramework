using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Asp.NetWebAPI.Models;

namespace Asp.NetWebAPI.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private readonly AspNetWebAPIContext db = new AspNetWebAPIContext();

        // GET: api/Books
        [Route("")]
        [HttpGet]
        public IQueryable<BookDto> GetBooks()
        {
            var books = from b in db.Books
                        select new BookDto
                        {
                            Id = b.Id,
                            Title = b.Title,
                            AuthorName = b.Author.Name
                        };
            return books;
        }

        // GET: api/Books/5
        [Route("{id:int}", Name = "GetBookById")]
        [HttpGet]
        [ResponseType(typeof(BookDetailDto))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            var book = await db.Books.Include(b => b.Author).Select(b =>
                new BookDetailDto()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name,
                    Genre = b.Genre
                }).SingleOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        // PUT: api/Books/5
        [Route("{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(BookDto))]
        public async Task<IHttpActionResult> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            await db.SaveChangesAsync();

            // New code:
            // Load author name
            db.Entry(book).Reference(x => x.Author).Load();

            var dto = new BookDto()
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.Author.Name
            };

            return CreatedAtRoute("DefaultApi", new { id = book.Id }, dto);
        }

        // DELETE: api/Books/5
        [Route("{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return Ok(book);
        }

        // GET /api/authors/1/books
        [Route("~/api/authors/{authorId:int}/books")]
        [HttpGet]
        public IEnumerable<BookDetailDto> GetByAuthorId(int authorId)
        {
            var books = db.Books.Include(b => b.Author)
                .Where(predicate: b => b.AuthorId == authorId)
                .Select(b =>
                new BookDetailDto()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name,
                    Genre = b.Genre
                });

            return books;
        }

        // GET /api/authors/Jane Austen/books
        [Route("~/api/authors/{authorName}/books")]
        [HttpGet]
        public IEnumerable<BookDetailDto> GetByAuthorName(string authorName)
        {
            var books = db.Books.Include(b => b.Author)
                .Where(predicate: b => b.Author.Name == authorName)
                .Select(b =>
                new BookDetailDto()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name,
                    Genre = b.Genre
                });

            return books;
        }

        /// <summary>
        /// 测试自定义约束类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("NonZero/{id:nonzero}")]
        public async Task<IHttpActionResult> GetBookByNonZero(int id)
        {
            var book = await db.Books.Include(b => b.Author).Select(b =>
                new BookDetailDto()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name,
                    Genre = b.Genre
                }).SingleOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// 可选 URI 参数和默认值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Locale/{id:int?}")]
        public async Task<IHttpActionResult> GetBooksByLocale(int id = 1) {
            var book = await db.Books.Include(b => b.Author).Select(b =>
                new BookDetailDto()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name,
                    Genre = b.Genre
                }).SingleOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }
    }
}