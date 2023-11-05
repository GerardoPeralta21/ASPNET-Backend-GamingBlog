namespace WebApiGames.DTO.Usuario
{
    public class UsuarioCreateDTO
    {
        public string Nombre { get; set; }
        public List<int> Roles { get; set; }
        public int TiendaId { get; set; } // El id de la tienda a la que pertenece el usuario

    }
}
