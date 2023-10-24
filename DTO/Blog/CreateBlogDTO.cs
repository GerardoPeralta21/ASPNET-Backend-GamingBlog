using System.ComponentModel.DataAnnotations;

namespace WebApiGames.DTO.Blog
{
    public class CreateBlogDTO
    {
        [Required]
        public string Name { get; set; }
        public int IdTienda { get; set; }
    }
}
