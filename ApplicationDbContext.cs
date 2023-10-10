using Microsoft.EntityFrameworkCore;
using WebApiGames.Entidades;

namespace WebApiGames
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tienda> Tiendas{ get; set; }
    }
}
