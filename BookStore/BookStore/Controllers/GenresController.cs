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
    public class GenresController : ControllerBase
    {
        private readonly BooksContext _context;

        public GenresController(BooksContext context)
        {
            _context = context;
            //if (_context.Genre.Count() == 0)
            //{
            //    //Create a new todoitem if collection is empty
            //    //which means u cant delete all todoitems
            //    _context.Genre.Add(new Genre { Name = "Action" });
            //    _context.Genre.Add(new Genre { Name = "Horror" });
            //    _context.Genre.Add(new Genre { Name = "Comedy" });
            //    _context.SaveChanges();
            //}
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenre()
        {
            return await _context.Genre.ToListAsync();
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(long id)
        {
            var genre = await _context.Genre.Include(g => g.book).ThenInclude(bg => bg.book).FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        // GET: api/Genres/5/Books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetGenreBooks(long id)
        {
            var genre = await _context.Genre.Include(g => g.book).ThenInclude(bg => bg.book).FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            var books = genre.book.ToList();
            if (books == null)
            {
                return NotFound();
            }
            var q = new List<Book>();
            foreach (var boo in books)
            {
                q.Add(boo.book);
            }


            return q;
        }

        // GET: api/Genres/5/Books/author
        [HttpGet("{id}/books/{ida}")]
        public async Task<ActionResult<Author>> GetGenreBookAuthor(long id, long ida)
        {
            var genre = await _context.Genre.Include(g => g.book).ThenInclude(bg => bg.book).FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            var bookGen = genre.book.FirstOrDefault(b => b.BookId == ida);
            if (bookGen == null)
            {
                NotFound();
            }

            var books = bookGen.book;
            if (books == null)
            {
                return NotFound();
            }

            var authorid = books.authorId;
            

            var author = await _context.Author.FirstOrDefaultAsync(a => a.Id == authorid);

            if (author == null)
            {
                NotFound();
            }
            return author;
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(long id, Genre genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            _context.Genre.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Genre>> DeleteGenre(long id)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genre.Remove(genre);
            await _context.SaveChangesAsync();

            return genre;
        }

        private bool GenreExists(long id)
        {
            return _context.Genre.Any(e => e.Id == id);
        }
    }
}
