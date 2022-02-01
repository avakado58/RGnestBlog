using Microsoft.AspNetCore.Builder;

namespace RGnestBlog
{
    public static class TokenExtensions
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string patern)
        {
            return builder.UseMiddleware<TokenMiddleware>(patern);
        }
    }
}
