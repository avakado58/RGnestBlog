using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RGnestBlog
{
    public class RoutingMiddleware
    {
        private readonly RequestDelegate _next;
        public RoutingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();
            if(path =="/index")
            {
                await context.Response.WriteAsync("index page");
            }
            else if (path =="/about")
            {
                await context.Response.WriteAsync("about page");
            }
            else
            {
                 context.Response.StatusCode = 404;
            }
            await _next.Invoke(context);
        }
    }
}
