using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RGnestBlog
{
    public class ErrorHandligMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandligMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);
            if(context.Response.StatusCode==403)
            {
                await context.Response.WriteAsync("access denied");
            }
            else if (context.Response.StatusCode==404)
            {
                await context.Response.WriteAsync("Not found");
            }
        }
    }
}
