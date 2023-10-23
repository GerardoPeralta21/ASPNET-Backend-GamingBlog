using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
            var blogs = await context.Blogs.ToListAsync();
            logger.LogWarning("Mensaje");
            Console.WriteLine("Este es un mensaje!");
            return blogs.Select(r => new BlogViewDTO { Id = r.Id, Name = r.Name }).ToList();
            
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateBlogDTO blog)
        {         
            var existeBlog = await context.Blogs.AnyAsync(x => x.Name == blog.Name);

            if (existeBlog)
            {
                return BadRequest($"Ya existe el blog{blog.Name}");
            }

            var blogToSave = new Blog{ Name = blog.Name};

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

            var BlogToEdit = new Blog { Name = blog.Name, Id = blog.Id };

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
