using Microsoft.AspNetCore.Builder;

namespace RGnestBlog
{
    public static class ErrorHandligExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandligMiddleware>();
        }
    }
}
