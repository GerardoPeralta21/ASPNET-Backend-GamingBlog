using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiGames.DTO.Rol;
using WebApiGames.Entidades;
using WebApiGames.Filtros;

namespace WebApiGames.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<TiendasController> logger;

        public RolesController(ApplicationDbContext context, ILogger<TiendasController> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<RolViewDTO>>> Get()
        {
            //return await context.Roles.ToListAsync();
            ////return await context.Roles.Include(u => u.Usuarios).ToListAsync();
            var roles = await context.Roles.ToListAsync();
            return roles.Select(r => new RolViewDTO { Id = r.Id, Nombre = r.Nombre }).ToList();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RolViewDTO>> GetById(int id)
        {
            var rol= await context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (rol == null)
            {
                return NotFound();
            }

            return new RolViewDTO { Id = rol.Id, Nombre = rol.Nombre };
        }

        [HttpPost]
        public async Task<ActionResult> Create(RolCreateDTO rolDto)
        {
            var existeRol = await context.Roles.AnyAsync(x => x.Nombre == rolDto.Nombre);

            if (existeRol)
            {
                return BadRequest($"Ya existe el rol {rolDto.Nombre}");
            }

            var rol = new Rol { Nombre = rolDto.Nombre };

            context.Add(rol);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(RolEditDTO rolDto, int id)
        {
            if (rolDto.Id != id)
            {
                return BadRequest("El id del rol no coincide con el id de la url");
            }

            var existeRol = await context.Roles.AnyAsync(x => x.Id == id);

            if (!existeRol)
            {
                return NotFound();
            }

            var rol = new Rol { Id = rolDto.Id, Nombre = rolDto.Nombre };

            context.Update(rol);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Roles.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            // Buscar si hay un usuario que tenga asociado el rol del id
            var tieneUsuario = await context.Usuarios.AnyAsync(u => u.Roles.Any(r => r.Id == id));

            if (tieneUsuario)
            {
                return BadRequest("No se puede eliminar el rol porque está asociado a un usuario.");
            }

            context.Remove(new Rol() { Id = id });
            //context.Remove(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
