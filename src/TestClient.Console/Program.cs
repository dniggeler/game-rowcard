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
                RowCardServiceClient rowCardServiceClient = new RowCardServiceClient();

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
                        case ClientAction.Autonomous:
                            await PlayAutonomousAsync(rowCardServiceClient);
                            break;

                        case ClientAction.Help:
                            PrintUsage();
                            break;

                        case ClientAction.Send:
                        {
                            await SendMessage(cmd.Option1.IfNone(""), cmd, connection);
                        }
                            break;

                        case ClientAction.JoinGame:
                        {
                            await rowCardServiceClient.JoinAsync(cmd.Option1.IfNone(""));
                        }
                            break;

                        case ClientAction.Reset:
                            await rowCardServiceClient.ResetAsync();
                            break;

                        default: continue;
                    }
                }
            }
        }

        private static async Task PlayAutonomousAsync(RowCardServiceClient rowCardServiceClient)
        {
            string playerName1 = "p1";
            string playerName2 = "p2";
            string playerName3 = "p3";
            string playerName4 = "p4";

            var pid1 = await rowCardServiceClient.JoinAsync(playerName1);
            var pid2 = await rowCardServiceClient.JoinAsync(playerName2);
            var pid3 = await rowCardServiceClient.JoinAsync(playerName3);
            var pid4 = await rowCardServiceClient.JoinAsync(playerName4);

            System.Console.WriteLine($"{playerName1}({pid1}) created");
            System.Console.WriteLine($"{playerName2}({pid2}) created");
            System.Console.WriteLine($"{playerName3}({pid3}) created");
            System.Console.WriteLine($"{playerName4}({pid4}) created");

            int gameId = await rowCardServiceClient.CreateGameAsync();
            System.Console.WriteLine($"Game({gameId}) created");

            await rowCardServiceClient.SetStartingPlayerAsync(pid1);
        }

        private static void PrintUsage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine("s: <message> - send message to chat");
            sb.AppendLine("j: <player name> - join the game");
            sb.AppendLine("q: - quit the client");
            sb.AppendLine("r: - reset the game");
            sb.AppendLine("p: - play autonomous");
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
