using System;
using AzulGameEngine.ChatHub;
using AzulGameEngine.Game;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            collection.AddSingleton(provider => new Random(Seed));
        }
    }
}