using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HangMan
{
    public class HangmanContext : DbContext
    {
        public DbSet<Words> Words { get; set; }

        public HangmanContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=hangmandb;Trusted_Connection=True;");
        }
    }
}
