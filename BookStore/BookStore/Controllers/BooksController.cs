using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksContext _context;

        public BooksController(BooksContext context)
        {
            _context = context;
            //if (_context.Book.Count() == 0)
            //{
            //    //Create a new todoitem if collection is empty
            //    //which means u cant delete all todoitems
            //    Author auth = new Author {  Name = "Mon", Surname = "Rev"};
            //    _context.Author.Add(auth);
            //    _context.Book.Add(new Book { Name = "Alchemy", author = auth/*_context.Author.FirstOrDefault(q => q.Id == 1)*/ });
            //    _context.SaveChanges();
            //}
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {

            return await _context.Book.ToListAsync();
            //return await _context.Book.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            //var book = await _context.Book.FindAsync(id);
            //var book = await _context.Book.Include(a => a.author).FindAsync(id);
            //var genres = await _context.Genre
            var book = await _context.Book.Include(a => a.author).Include(b => b.genre).ThenInclude(bg => bg.genre).FirstOrDefaultAsync(i => i.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // GET: api/Books/5/Author
        [HttpGet("{id}/authors")]
        public async Task<ActionResult<Author>> GetBookAuthor(long id)
        {
            var book = await _context.Book.Include(a => a.author).FirstOrDefaultAsync(i => i.Id == id);
            

            if (book == null)
            {
                return NotFound();
            }

            var author = book.author;

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // GET: api/Books/5/Genre
        [HttpGet("{id}/genres")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetBookGenre(long id)
        {
            var book = await _context.Book.Include(b => b.genre).ThenInclude(bg => bg.genre).FirstOrDefaultAsync(i => i.Id == id);


            if (book == null)
            {
                return NotFound();
            }

            var genreGen = book.genre;

            if (genreGen == null)
            {
                return NotFound();
            }

            var q = new List<Genre>();
            foreach (var gen in genreGen)
            {
                q.Add(gen.genre);
            }

            return q;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            //_context.Entry(book).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            //that author wouldnt change when adding same author

            //if (book.authorId != 0)
            //{
            //    var auth = await _context.Author.FindAsync(book.authorId);
            //    //if (auth == null)
            //    //{
            //    //    _context.Entry(book.author).State = EntityState.Unchanged;
            //    //}
            //    //return NotFound();
            //    //{

            //    //}

            //    //Author auth = book.author;
            //    //if (_context.Author.Find(auth.Id) == null) //.Contains(i => i.Id == auth.Id) .FirstAsync(i => i.Id == auth.Id) == null)
            //    //{
            //    //    //_context.Entry(book.author).State = EntityState.Unchanged;
            //    //    _context.Author.Add(book.author);
            //    //}
            //}
            //var auth = await _context.Author.FirstOrDefaultAsync(i => i.Id == book.authorId);
            //if (await _context.Author.FirstOrDefaultAsync(i => i.Id == book.authorId) != null)
            //{
            //    _context.Entry(book.author).State = EntityState.Unchanged;
            //}
            //--------------------------------------------------
            //if (book.genre != null)
            //{
            //    _context.Genre.Add(book.genre);
            //}

            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(long id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        private bool BookExists(long id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
