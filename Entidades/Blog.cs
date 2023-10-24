using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace WebApiGames.Entidades
{
    public class Blog
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public BlogHeader? Header { get; set; } // Reference navigation to dependent

        public int TiendaId { get; set; } // Required foreign key property
        public Tienda Tienda { get; set; } = null!; // Required reference navigation to principal
    }
}
