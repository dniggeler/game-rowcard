using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RowCardGame;

namespace RowCardMgmtApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddHttpClient<IRowCardServiceClient>(client =>
            //{
            //    client.BaseAddress = new Uri(@"https://localhost");
            //});

            await builder.Build().RunAsync();
        }
    }
}
