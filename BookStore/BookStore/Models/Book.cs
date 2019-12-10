using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<book_genre> genre { get; set; }

        public long authorId { get; set; }
        public Author author { get; set; }
    }
}
