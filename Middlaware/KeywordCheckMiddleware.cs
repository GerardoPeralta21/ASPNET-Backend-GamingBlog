using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebApiGames.Middlaware
{
    public class KeywordCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public KeywordCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Aquí verificarás el encabezado "palabraClave" en la solicitud.
            string keyword = context.Request.Headers["palabraClave"];

            if (string.Equals(keyword, "salchicha", StringComparison.OrdinalIgnoreCase))
            {
                // Si la palabra clave es "salchicha", continúa con la solicitud normal.
                await _next(context);
            }
            else
            {
                // Si la palabra clave no es "salchicha", devuelve un error.
                context.Response.StatusCode = 403; // Puedes usar el código de estado que prefieras.
                await context.Response.WriteAsync("Palabra clave incorrecta");
            }
        }
    }
}
