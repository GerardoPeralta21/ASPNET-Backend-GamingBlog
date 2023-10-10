namespace WebApiGames.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Otras propiedades de usuario

        public ICollection<Rol> Roles { get; set; }
    }
}
