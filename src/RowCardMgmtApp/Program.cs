using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RowCardGame;

namespace RowCardMgmtApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddHttpClient("ServerAPI",
                client => client.BaseAddress = new Uri("https://www.example.com/base"));

            builder.Services.AddHttpClient(client =>
            {
                client.BaseAddress = builder.Configuration.GetServiceUri("game-service");
            });

            await builder.Build().RunAsync();
        }
    }
}
