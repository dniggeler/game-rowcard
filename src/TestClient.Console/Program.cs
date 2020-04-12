using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace TestClient.Console
{
    class Program
    {
        private const string ChatHubName = "chatHub";
        private const string BaseAddress = "https://localhost:44380";

        static async Task Main(string[] args)
        {
            string url = $"{BaseAddress}/{ChatHubName}";

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            await connection.StartAsync();

            connection.On<string, string>("NewMessage", (playerName, message) =>
            {
                System.Console.WriteLine($"{playerName}: {message}");
            });

            connection.On<string>("NewPlayer", (playerName) =>
            {
                System.Console.WriteLine($"New player {playerName} joined the game.");
            });

            System.Console.WriteLine($"Connection is in state '{connection.State}'");

            if (connection.State == HubConnectionState.Connected)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri($"{BaseAddress}/api/game/");

                Random rnd = new Random();

                string playerName = $"toni-{rnd.Next(100)}";

                bool isProcessing = true;
                while (isProcessing)
                {
                    Command cmd = Command.For(System.Console.ReadLine());

                    switch (cmd.ClientAction)
                    {
                        case ClientAction.Quit:
                        {
                            isProcessing = false;
                            break;
                        }
                        case ClientAction.Help:
                            PrintUsage();
                            break;
                        case ClientAction.Send:
                            await SendMessage(playerName, cmd, connection);
                            break;
                        case ClientAction.JoinGame:
                            await JoinGame(playerName, httpClient);
                            break;
                        default: continue;
                    }
                }
            }
        }

        private static async Task JoinGame(string playerName, HttpClient httpClient)
        {
            var response = await httpClient.PostAsync($"players/{playerName}",null);
        }

        private static void PrintUsage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine("s: <message> - send message to chat");
            sb.AppendLine("j: <player name> - join the game");
            sb.AppendLine("q: - quit the client");
            sb.AppendLine("h: - this message");
        }

        private static async Task SendMessage(
            string playerName, Command command, HubConnection connection)
        {
            await command.Option1
                .IfSomeAsync(msg => connection.InvokeAsync("SendMessage", playerName, msg));
        }
    }
}
