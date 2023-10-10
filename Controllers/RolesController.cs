﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<List<Rol>>> Get()
        {
            return await context.Roles.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Rol rol)
        {
            logger.LogInformation("Estoy en el controlador de crear");
            var existeTienda = await context.Roles.AnyAsync(x => x.Nombre == rol.Nombre);

            if (existeTienda)
            {
                return BadRequest($"Ya existe la tienda {rol.Nombre}");
            }

            context.Add(rol);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Rol rol, int id)
        {
            var existe = await context.Roles.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            if (rol.Id != id)
            {
                return BadRequest("El id del rol no coincide con el id de la url");
            }
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

            context.Remove(new Rol() { Id = id });
            //context.Remove(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}