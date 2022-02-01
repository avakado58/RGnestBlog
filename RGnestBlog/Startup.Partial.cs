using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace RGnestBlog
{
    public partial class Startup
    {
        private void TryCookies(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.Items["text"] = "<h1> Привет Мир</h1>";
                context.Items.Add("two", "<h1> Привет Мир Two</h1>");
                if(context.Request.Cookies.ContainsKey("name"))
                {
                    await next.Invoke();
                }
                else
                {
                    context.Response.Cookies.Append("name", "victor", new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(1)
                    });
                    await next.Invoke();
                }
                
            });
            app.Map("/hello", app =>
            {
                app.Run(async (context) =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(context.Items["text"].ToString());
                });
            });
            app.Map("/hi", app =>
            {
                app.Run(async (context) =>
                {
                    object itemValue;
                    if (context.Items.TryGetValue("two", out itemValue))
                    {
                        context.Response.ContentType = "text/html; charset=utf-8";
                        await context.Response.WriteAsync(itemValue.ToString());
                    }

                });
            });
            /*app.Run(async (context) =>
            {
                if (context.Request.Cookies.ContainsKey("name"))
                {
                    //context.Response.Cookies.Delete("name");
                    await context.Response.WriteAsync($"name in cookies {context.Request.Cookies["name"]} ");
                }
                else
                {

                    await context.Response.WriteAsync($"Cookies \"name\" not faund ");
                }
            });*/
        }

        private void AddOwnMidlleware(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseErrorHandling();
            app.UseToken("555555");
            app.UseMiddleware<RoutingMiddleware>();
        }

        private void MapUseRun(IApplicationBuilder app, IWebHostEnvironment env)
        {
            int x = 2;
            int y = 4;
            //маршрут для url /index
            app.Map("/index", (app) =>
            {
                app.Run(async (content) =>
                {
                    await content.Response.WriteAsync("<h1>Hello</h1>");
                });
            });
            //маршрут для url /about
            app.Map("/about", (app) =>
             {
                 app.Run(async (content) =>
                 {
                     await content.Response.WriteAsync("<div style=\"font - size: 36px; font - family: monospace; color: #cd66cc\">Its Wendsday my dude</div>");
                 });
             });
            //вложенный метод для home(/home/tab и home/select)
            app.Map("/home", home =>
            {
                home.Map("/tab", (app) =>
                {
                    app.Run(async (content) => await content.Response.WriteAsync("tab"));
                });
                home.Map("/select", (app) =>
                {
                    app.Run(async (content) => await content.Response.WriteAsync("select"));
                });
            });
            //map с проверкой условия, принемает делегат с входным httpcontext и выходным bool
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("id") && context.Request.Query["id"] == "5";
            }, (app) =>
            {
                app.Run(async content =>
                {
                    await content.Response.WriteAsync("is 5 yeees");
                });
            });
            //вызов следующего midlware с помошью делегата next
            app.Use(async (context, next) =>
            {
                x = y * x;
                await next.Invoke();
                x *= 2;
                await context.Response.WriteAsync($"<h1>x={x}</h1>");
            });
            app.Run(async (context) =>
            {
                x *=2;
                await System.Threading.Tasks.Task.FromResult(0);
            });
        }

        private void TryEndpoints(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hiii MapGet /");
                });
                endpoints.MapGet("/trytwo", async context =>
                {
                    await context.Response.WriteAsync("Hiii MapGet /trytwo");
                });
                

            });
        }

    }
}
