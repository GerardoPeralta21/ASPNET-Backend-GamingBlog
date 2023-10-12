using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiGames.Entidades;

namespace WebApiGames.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<TiendasController> logger;

        public UsuarioController(ApplicationDbContext context, ILogger<TiendasController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            //return await context.Usuarios.ToListAsync();
            return await context.Usuarios.Include(u => u.Roles).ToListAsync();

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            //throw new NotImplementedException();
            var usuario = await context.Usuarios.Include(u => u.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            var existeTienda = await context.Usuarios.AnyAsync(x => x.Nombre == usuario.Nombre);

            if (existeTienda)
            {
                return BadRequest($"Ya existe la tienda {usuario.Nombre}");
            }

            // Verifica si los roles existen
            var rolesExistentes = await context.Roles.Where(r => usuario.Roles.Select(ur => ur.Id).Contains(r.Id)).ToListAsync();
            var rolesNoExistentes = usuario.Roles.Where(ur => !rolesExistentes.Any(re => re.Id == ur.Id)).ToList();

            if (rolesNoExistentes.Any())
            {
                return BadRequest($"Los siguientes roles no existen: {string.Join(", ", rolesNoExistentes.Select(r => r.Id))}");
            }

            usuario.Roles = rolesExistentes;

            context.Add(usuario);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Usuario usuario, int id)
        {
            var existe = await context.Usuarios.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            if (usuario.Id != id)
            {
                return BadRequest("El id del usuario no coincide con el id de la url");
            }

            // Verifica si los roles existen
            var rolesExistentes = await context.Roles.Where(r => usuario.Roles.Select(ur => ur.Id).Contains(r.Id)).ToListAsync();
            var rolesNoExistentes = usuario.Roles.Where(ur => !rolesExistentes.Any(re => re.Id == ur.Id)).ToList();

            if (rolesNoExistentes.Any())
            {
                return BadRequest($"Los siguientes roles no existen: {string.Join(", ", rolesNoExistentes.Select(r => r.Id))}");
            }

            // Carga el usuario existente desde la base de datos
            var usuarioExistente = await context.Usuarios.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);

            // Elimina los roles existentes del usuario
            usuarioExistente.Roles.Clear();

            // Añade los nuevos roles al usuario
            var roles = await context.Roles.Where(r => usuario.Roles.Select(ur => ur.Id).Contains(r.Id)).ToListAsync();
            usuarioExistente.Roles = roles;

            // Actualiza el nombre del usuario
            usuarioExistente.Nombre = usuario.Nombre;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // maneja el error como prefieras
                throw;
            }

            return Ok();
            //Objeto de ejemplo
//            {
//                "id": 6,
//                "nombre": "Kim Domenech",
//                "roles": [{ "id": 3 }]
//            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Usuarios.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Usuario() { Id = id });
            //context.Remove(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
