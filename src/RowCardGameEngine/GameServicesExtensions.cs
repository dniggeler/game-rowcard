using System;
using AzulGameEngine.ChatHub;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RowCardGameEngine.Game;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine
{
    public static class GameServicesExtensions
    {
        const int Seed = 1;

        public static void AddServices(this IServiceCollection collection)
        {
            collection.AddLogging(builder => builder.AddConsole());

            collection.AddSingleton<IChatClient, ChatClient>();
            collection.AddSingleton<ChatThread>();
            collection.AddSingleton<GameEngine>();
            collection.AddTransient<Deck>();
            collection.AddTransient<GameBoard>();
            collection.AddSingleton<Func<GameBoard>>(ctx => ctx.GetRequiredService<GameBoard>);
            collection.AddSingleton(provider => new Random(Seed));
        }
    }
}