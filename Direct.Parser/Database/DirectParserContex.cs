using Microsoft.EntityFrameworkCore;
using Direct.Parser.Database.Models;

namespace Direct.Parser.Database
{
    public class DirectParserContex : DbContext
    {
        public DirectParserContex(DbContextOptions<DirectParserContex> options) : base(options) { }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<AdImage> AdImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ad>().HasKey(u => new { u.Id });
            modelBuilder.Entity<AdImage>().HasKey(u => new {u.ImageHash});
        }
    }
}
