using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.Metadata;
using WebApiGames.DTO.Blog;
using WebApiGames.DTO.BlogHeader;
using WebApiGames.Entidades;

namespace WebApiGames.Controllers
{
    [ApiController]
    [Route("api/blogHeaders")]
    public class BlogHeaderController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<TiendasController> logger;

        public BlogHeaderController(ApplicationDbContext context, ILogger<TiendasController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogHeaderViewDTO>>> Get()
        {
            var blogsHeader = await context.BlogHeaders.Include(r => r.Blog).ToListAsync();

            return blogsHeader.Select(r => new BlogHeaderViewDTO { Id = r.Id, Title = r.Title, BlogName = r.Blog.Name }).ToList();

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BlogHeaderCreateDTO blogHeader)
        {
            var existeBlogHeader = await context.BlogHeaders.AnyAsync(x => x.Title == blogHeader.Title);

            if (existeBlogHeader)
            {
                return BadRequest($"Ya existe el blog{blogHeader.Title}");
            }

            var existeBlog = await context.Blogs.AnyAsync(x => x.Id == blogHeader.BlogId);

            if (!existeBlog)
            {
                return BadRequest($"No existe el blog{blogHeader.BlogId}");
            }

            var existeBlogHeaderConElBlog = await context.BlogHeaders.AnyAsync(x => x.BlogId == blogHeader.BlogId);

            if (existeBlogHeaderConElBlog)
            {
                return BadRequest($"El blog {blogHeader.BlogId} ya tiene un blog header asociado");
            }

            var blog = await context.Blogs.FindAsync(blogHeader.BlogId);

            var blogHeaderToSave = new BlogHeader { BlogId = blogHeader.BlogId, Title = blogHeader.Title, Blog = blog };

            context.Add(blogHeaderToSave);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(BlogHeaderEditDTO blogHeader, int id)
        {
            var existe = await context.BlogHeaders.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return BadRequest("El id del blogHeader no existe");
            }

            if (blogHeader.Id != id)
            {
                return BadRequest("El id del blogHeader no coincide con el id de la url");
            }

            var existeBlog = await context.Blogs.AnyAsync(x => x.Id == blogHeader.BlogId);

            if (!existeBlog)
            {
                return BadRequest("El id del blog no existe");
            }

            var blog = await context.Blogs.FindAsync(blogHeader.BlogId);

            var blogHeaderToEdit = new BlogHeader { Id = blogHeader.Id, BlogId = blogHeader.Id, Title = blogHeader.Title, Blog = blog };

            context.Update(blogHeaderToEdit);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.BlogHeaders.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new BlogHeader() { Id = id });
            //context.Remove(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
