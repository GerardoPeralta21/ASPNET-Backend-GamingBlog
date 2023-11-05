namespace WebApiGames.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Otras propiedades de usuario

        public ICollection<Rol> Roles { get; set; }

        public int TiendaId{ get; set; } // Required foreign key property
        public Tienda Tienda{ get; set; } = null!; // Required reference navigation to principal
    }
}
