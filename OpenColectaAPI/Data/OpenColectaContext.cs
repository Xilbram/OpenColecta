using Microsoft.EntityFrameworkCore;
using OpenColectaAPI.Models;

namespace OpenColectaAPI.Data
{
    public class OpenColectaContext : DbContext
    {
        public OpenColectaContext(DbContextOptions<OpenColectaContext> options) : base(options)
        {
        }

        public DbSet<Materia> Materias { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Materia>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Materia);
        }
    }
}