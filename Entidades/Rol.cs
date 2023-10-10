namespace WebApiGames.Entidades
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Otras propiedades de rol

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
