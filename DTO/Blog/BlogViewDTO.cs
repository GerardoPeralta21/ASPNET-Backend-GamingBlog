using System.ComponentModel.DataAnnotations;
using WebApiGames.DTO.Tienda;

namespace WebApiGames.DTO.Blog
{
    public class BlogViewDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string NombreTienda { get; set; }
    }
}
