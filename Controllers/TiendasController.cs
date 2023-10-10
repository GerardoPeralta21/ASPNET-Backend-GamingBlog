using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiGames.Entidades;
using WebApiGames.Filtros;

namespace WebApiGames.Controllers
{
    [ApiController]
    [Route("api/tiendas")]
    public class TiendasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<TiendasController> logger;

        public TiendasController(ApplicationDbContext context, ILogger<TiendasController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(AdminRoleAuthorizationFilter))]
        public async Task<ActionResult<List<Tienda>>> Get()
        {
            return await context.Tiendas.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Tienda tienda)
        {

            var existeTienda = await context.Tiendas.AnyAsync(x => x.Nombre == tienda.Nombre);

            if (existeTienda)
            {
                return BadRequest($"Ya existe la tienda {tienda.Nombre}");
            }

            context.Add(tienda);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Tienda tienda, int id)
        {
            //var entityType = typeof(Tienda);
            //var existe = await context.FindAsync(entityType, id);

            //if (existe is null)
            //{
            //    return BadRequest("Not found");
            //}

            var existe = await context.Tiendas.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            if (tienda.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la url");
            }
            context.Update(tienda);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //var existe = await context.AnyAsync(context => context.Id == id);


            //var entityType = typeof(Tienda);
            //var autor = await context.FindAsync(entityType, id);

            //if (autor is null)
            //{
            //    return BadRequest("Not found");
            //}

            var existe = await context.Tiendas.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Tienda() { Id = id});   
            //context.Remove(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
