using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace WebApiGames.Filtros
{
    public class AdminRoleAuthorizationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Verificar si el rol "admin" está presente en el encabezado.
            string role = context.HttpContext.Request.Headers["rol"];

            if (string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase))
            {
                // Si el rol es "admin", permitir el acceso a la acción del controlador.
                await next();
            }
            else
            {
                // Si el rol no es "admin" o no se proporciona, devolver un error de permiso.
                context.Result = new UnauthorizedObjectResult("No tiene permiso para esta solicitud");
            }
        }
    }
}
