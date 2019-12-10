using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Genre
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<book_genre> book { get; set; }
    }
}
