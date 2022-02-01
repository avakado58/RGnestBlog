using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RGnestBlog
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        string _patern;

        public TokenMiddleware(RequestDelegate next, string patern)
        {
            _next = next;
            _patern = patern;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];
            if (token !=_patern)
            {
                context.Response.StatusCode = 403;
                //await context.Response.WriteAsync("this is invalid token");
            }
            else
            {
                await _next.Invoke(context);
            }
        }

    }
}
