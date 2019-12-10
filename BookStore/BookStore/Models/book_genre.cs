using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class book_genre
    {
        //public long Id { get; set; }
        public long BookId { get; set; }
        public Book book { get; set; }
        public long GenreId { get; set; }
        public Genre genre { get; set; }
    }
}
