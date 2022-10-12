using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;

namespace ADO.NET_Homework_3.Model
{
    internal class MyDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MyDbContext()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

        public DbSet<Sage> Sages { get; set; }
        public DbSet<Book> Books { get; set; }

        public IEnumerable<BookSages> BookSage
        {
            get
            {
                List<BookSages> list = new();

                foreach (var book in Books)
                    foreach (var sage in Sages)
                        list.Add(new(book.Id, sage.Id));

                return list;
            }
        }
    }
}
