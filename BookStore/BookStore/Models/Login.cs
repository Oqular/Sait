using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }

    }
}

//add-migration BookStore.Models.BooksContext
//update-database
