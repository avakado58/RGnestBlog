using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
namespace RGnestBlog
{
    public partial class Startup
    {
        readonly IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()//выставляем совместимость с asp.net core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest).AddSessionStateTempDataProvider();
            services.AddMemoryCache();
            services.AddSession( option =>
            {
                option.Cookie.Name = "MyApp.Session";
                option.IdleTimeout = TimeSpan.FromSeconds(10);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            env.EnvironmentName = "Development";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.Map("/error", app => app.Run(async context => await context.Response.WriteAsync("error page")));

            #region static file
            /*app.UseDirectoryBrowser();
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("bla.html");
            app.UseStatusCodePages();
            app.UseDefaultFiles(options);
            app.UseStaticFiles();*/
            #endregion
            TryCookies(app,env);
            #region session
            /*app.UseSession();

            app.Map("/session", (app) => 
            {
                app.Run(async (context) =>
                {
                    if (context.Session.Keys.Contains("name"))
                    {
                        await context.Response.WriteAsync(context.Session.GetString("name"));
                    }
                    else
                    {
                        context.Session.SetString("name", context.Request.Cookies?["name"]);
                        await context.Response.WriteAsync("take name from cookies");
                    }
                });
            });
            app.Map("/Person", (app) =>
            {
                app.Run(async (context) =>
                {
                    if(context.Session.Keys.Contains("Person"))
                    {
                        Person person = context.Session.Get<Person>("Person");
                        await context.Response.WriteAsync($"Hello {person.Name}, your Id is {person.Id}");
                    }
                    else
                    {
                        context.Session.Set<Person>("Person", new Person { Id = 1, Name = "Victor" });
                        await context.Response.WriteAsync("Hello unknown person");
                    }
                });
            });*/
            #endregion

            var routHandler = new RouteHandler(Handler);
            var routBilder = new RouteBuilder(app, routHandler);
            routBilder.MapRoute("default", "{controller=home}/{action=index}");
            app.UseRouter(routBilder.Build());


        }
        private async Task Handler(HttpContext context)
        {
            var routDate = context.GetRouteData().Values;
            var routAction = routDate["controller"];
            var routMethod = routDate["action"];
            await context.Response.WriteAsync($"Hi RouterMidlleware \n routAction {routAction}| routMethod {routMethod}");
        }
    }
}
