using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Models
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=saitynai;Trusted_Connection=True;");
        //    }
        //    //base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<book_genre>().HasKey(bg => new { bg.BookId, bg.GenreId });
            modelBuilder.Entity<book_genre>().HasOne(bg => bg.book).WithMany(b => b.genre).HasForeignKey(bg => bg.BookId);
            modelBuilder.Entity<book_genre>().HasOne(bg => bg.genre).WithMany(g => g.book).HasForeignKey(bg => bg.GenreId);

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<BookStore.Models.Book> Book { get; set; }

        public DbSet<BookStore.Models.Genre> Genre { get; set; }

        public DbSet<BookStore.Models.Author> Author { get; set; }

        public DbSet<BookStore.Models.Users> Users { get; set; }
    }
}
