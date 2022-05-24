using System;
using Microsoft.EntityFrameworkCore;
using Direct.Parser.Database.Models;

namespace Direct.Parser.Database
{
    public class DirectParserContex : DbContext
    {
        public DirectParserContex(DbContextOptions<DirectParserContex> options) : base(options) { }
        public DbSet<Ad> Ads { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ad>().HasKey(u => new { u.Id });
        }
    }
}
