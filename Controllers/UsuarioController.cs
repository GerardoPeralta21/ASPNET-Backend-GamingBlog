using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiGames.DTO.Rol;
using WebApiGames.DTO.Usuario;
using WebApiGames.Entidades;

namespace WebApiGames.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<TiendasController> logger;
        private readonly IMapper mapper;

        public UsuarioController(ApplicationDbContext context, ILogger<TiendasController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioViewDTO>>> Get()
        {
            var usuarios = await context.Usuarios.Include(u => u.Tienda).Include(u => u.Roles).ToListAsync();

            var usuariosDTO = mapper.Map<List<UsuarioViewDTO>>(usuarios);

            return Ok(usuariosDTO);
            //return usuarios.Select(u => new UsuarioViewDTO
            //{
            //    Id = u.Id,
            //    Nombre = u.Nombre,
            //    Roles = u.Roles.Select(r => new RolViewDTO { Id = r.Id, Nombre = r.Nombre }).ToList()
            //}).ToList();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioViewDTO>> GetById(int id)
        {
            var usuario = await context.Usuarios.Include(u => u.Tienda).Include(u => u.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDTO = mapper.Map<UsuarioViewDTO>(usuario);

            return Ok(usuarioDTO);


            //return new UsuarioViewDTO
            //{
            //    Id = id,
            //    Nombre = usuario.Nombre,
            //    Roles = usuario.Roles.Select(r => new RolViewDTO { Id = r.Id, Nombre = r.Nombre }).ToList()
            //};
        }

        [HttpPost]
        public async Task<ActionResult> Create(UsuarioCreateDTO usuarioDto)
        {
            var existeUsuario = await context.Usuarios.AnyAsync(x => x.Nombre == usuarioDto.Nombre);

            if (existeUsuario)
            {
                return BadRequest($"Ya existe el usuario {usuarioDto.Nombre}");
            }


            var rolesExistentes = await context.Roles.Where(r => usuarioDto.Roles.Contains(r.Id)).ToListAsync();

            if (rolesExistentes.Count() != usuarioDto.Roles.Count())
            {
                int[] rolesUsuarioDto = usuarioDto.Roles.ToArray();
                var idsRolesNoExistentes = rolesUsuarioDto.Except(rolesExistentes.Select( u => u.Id).ToArray()).ToArray();
                return BadRequest($"Los siguientes id, no corresponden a roles: {string.Join(", ", idsRolesNoExistentes)}");
            }

            //var usuario = new Usuario
            //{
            //    Nombre = usuarioDto.Nombre,
            //    Roles = rolesExistentes
            //};

            var usuario = mapper.Map<Usuario>(usuarioDto);
            usuario.Roles = rolesExistentes;

            context.Add(usuario);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(UsuarioEditDTO usuarioDto, int id)
        {
            var existeUsuario = await context.Usuarios.AnyAsync(x => x.Id == id);

            if (!existeUsuario)
            {
                return NotFound();
            }
            var rolesExistentes = await context.Roles.Where(r => usuarioDto.Roles.Contains(r.Id)).ToListAsync();

            if (rolesExistentes.Count() != usuarioDto.Roles.Count())
            {
                int[] rolesUsuarioDto = usuarioDto.Roles.ToArray();
                var idsRolesNoExistentes = rolesUsuarioDto.Except(rolesExistentes.Select(u => u.Id).ToArray()).ToArray();
                return BadRequest($"Los siguientes id, no corresponden a roles: {string.Join(", ", idsRolesNoExistentes)}");
            }

            //var rolesExistentes = await context.Roles.Where(r => usuarioDto.Roles.Contains(r.Id)).ToListAsync();

            var usuarioExistente = await context.Usuarios.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);

            usuarioExistente.Nombre = usuarioDto.Nombre;

            // Elimina los roles existentes del usuario
            usuarioExistente.Roles.Clear();

            // Añade los nuevos roles al usuario
            //usuarioExistente.Roles.AddRange(rolesExistentes);
            usuarioExistente.Roles = rolesExistentes;

            try
            {
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                // maneja el error como prefieras
                throw;
            }
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
