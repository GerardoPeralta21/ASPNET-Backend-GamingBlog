using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;
using WebApiGames.DTO.Blog;
using WebApiGames.Entidades;

namespace WebApiGames.Controllers
{
    [ApiController]
    [Route("api/blogs")]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<TiendasController> logger;

        public BlogController(ApplicationDbContext context, ILogger<TiendasController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogViewDTO>>> Get()
        {
            //var blogs = await context.Blogs.ToListAsync();
            var blogs = await context.Blogs.Include(b => b.Tienda).ToListAsync();
            logger.LogCritical("Esto es una prueba de loger Critical");
            logger.LogError("Esto es una prueba de loger Error");
            logger.LogWarning("Esto es una prueba de loger Warning");
            logger.LogInformation("Esto es una prueba de loger Information");
            logger.LogDebug("Esto es una prueba de loger Debug");
            logger.LogTrace("Esto es una prueba de loger Trace");
            return blogs.Select(r => new BlogViewDTO { Id = r.Id, Name = r.Name, NombreTienda = r.Tienda?.Nombre ?? "N/A" }).ToList();
            
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateBlogDTO blog)
        {         
            var existeBlog = await context.Blogs.AnyAsync(x => x.Name == blog.Name);

            if (existeBlog)
            {
                return BadRequest($"Ya existe el blog{blog.Name}");
            }

            var existeTienda = await context.Tiendas.AnyAsync(x => x.Id == blog.IdTienda);

            if (!existeTienda)
            {
                return BadRequest($"No existe la tienda que intentas asociar");
            }

            var tienda = await context.Tiendas.FindAsync(blog.IdTienda);

            var blogToSave = new Blog{ Name = blog.Name, TiendaId = blog.IdTienda, Tienda = tienda};

            context.Add(blogToSave); 
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(BlogEditDTO blog, int id)
        {
            var existe = await context.Blogs.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            if (blog.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la url");
            }

            var existeTienda = await context.Tiendas.AnyAsync(x => x.Id == blog.IdTienda);

            if (!existeTienda)
            {
                return BadRequest($"No existe la tienda que intentas asociar");
            }

            var tienda = await context.Tiendas.FindAsync(blog.IdTienda);

            var BlogToEdit = new Blog { Name = blog.Name, Id = blog.Id, TiendaId = blog.IdTienda, Tienda = tienda };

            context.Update(BlogToEdit);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Blogs.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Blog() { Id = id });
            //context.Remove(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
