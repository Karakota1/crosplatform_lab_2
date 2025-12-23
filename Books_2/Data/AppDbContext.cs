using Books_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Books_2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<FilmScreening> FilmScreenings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cinema>()
                .HasMany(t => t.FilmScreenings)
                .WithOne(p => p.Cinema)
                .HasForeignKey(p => p.CinemaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
