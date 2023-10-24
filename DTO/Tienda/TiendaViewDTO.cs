using WebApiGames.DTO.Blog;
using WebApiGames.DTO.Rol;

namespace WebApiGames.DTO.Tienda
{
    public class TiendaViewDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<BlogViewDTO> Blogs{ get; set; }
    }
}
