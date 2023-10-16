using WebApiGames.DTO.Rol;

namespace WebApiGames.DTO.Usuario
{
    public class UsuarioViewDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<RolViewDTO> Roles { get; set; }
    }
}
