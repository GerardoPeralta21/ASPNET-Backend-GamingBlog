using System.ComponentModel.DataAnnotations;

namespace WebApiGames.Entidades
{
    public class BlogHeader
    {
        public int Id { get; set; }
        public int BlogId { get; set; } // Required foreign key property
        [Required]
        public string Title { get; set; }
        public Blog Blog { get; set; } = null!; // Required reference navigation to principal
    }
}
 