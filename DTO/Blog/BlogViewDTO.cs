using System.ComponentModel.DataAnnotations;

namespace WebApiGames.DTO.Blog
{
    public class BlogViewDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
