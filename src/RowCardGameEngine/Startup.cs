using System;
using System.Text.Json.Serialization;
using AzulGameEngine.ChatHub;
using AzulGameEngine.Game;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzulGameEngine
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            const int seed = 1;

            services.AddSignalR();
            services.AddControllers();

            // application services
            services.AddSingleton<IChatClient, ChatClient>();
            services.AddSingleton<ChatThread>();
            services.AddSingleton<GameEngine>();
            services.AddSingleton(provider => new Random(seed));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to Azul Game Engine");
                });

                endpoints.MapControllers();

                endpoints.MapHub<ChatHub.ChatHub>("/chathub");
            });
        }
    }
}
