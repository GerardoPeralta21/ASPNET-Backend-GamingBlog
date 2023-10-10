using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApiGames.Entidades;

namespace WebApiGames
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tienda> Tiendas{ get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Usuarios)
                .WithMany(e => e.Roles);
        }
    }
}
