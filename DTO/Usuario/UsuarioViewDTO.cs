using WebApiGames.DTO.Rol;
using WebApiGames.DTO.Tienda;

namespace WebApiGames.DTO.Usuario
{
    public class UsuarioViewDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<RolViewDTO> Roles { get; set; }

        public String NombreTienda { get; set; }

    }
}
