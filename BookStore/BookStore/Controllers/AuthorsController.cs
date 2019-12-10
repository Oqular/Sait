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
    public class AuthorsController : ControllerBase
    {
        private readonly BooksContext _context;

        public AuthorsController(BooksContext context)
        {
            _context = context;
            //if (_context.Author.Count() == 0)
            //{
            //    //Create a new todoitem if collection is empty
            //    //which means u cant delete all todoitems
            //    _context.Author.Add(new Author { Name = "Jhon", Surname = "Jhevords"});
            //    _context.SaveChanges();
            //}
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthor()
        {
            return await _context.Author.ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(long id)
        {
            var author = await _context.Author.Include(a => a.books).FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // GET: api/Authors/5/Books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAuthorBooks(long id)
        {

            var author = await _context.Author.Include(a => a.books).FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            var books = author.books.ToList();

            
            if (books == null)
            {
                return NotFound();
            }

            return books;
        }

        [HttpGet("{id}/books/{idb}")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAuthorBookGenres(long id, long idb)
        {

            var author = await _context.Author.Include(a => a.books).FirstOrDefaultAsync(a => a.Id == id);
            
            if (author == null)
            {
                return NotFound();
            }
            var book = author.books.FirstOrDefault(b => b.Id == idb);
            if (book == null)
            {
                NotFound();
            }
            var bookGen = await _context.Book.Include(b => b.genre).ThenInclude(bg => bg.genre).FirstOrDefaultAsync(b => b.Id == idb);
            //var book = author.books.FirstOrDefault(b => b.Id == idb);
            if (bookGen == null)
            {
                return NotFound();
            }
            var genres = bookGen.genre.ToList();
            if (genres == null)
            {
                return NotFound();
            }
            var q = new List<Genre>();
            foreach(var gen in genres)
            {
                q.Add(gen.genre);
            }

            
            
            

            return q;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(long id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> DeleteAuthor(long id)
        {
            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Author.Remove(author);
            await _context.SaveChangesAsync();

            return author;
        }

        private bool AuthorExists(long id)
        {
            return _context.Author.Any(e => e.Id == id);
        }
    }
}
