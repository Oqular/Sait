using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Users
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
