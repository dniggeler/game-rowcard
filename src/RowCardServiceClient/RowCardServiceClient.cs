using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RowCardGame
{
    public class RowCardServiceClient : IRowCardServiceClient
    {
        private const string RelativePathPart = "api/game";

        private readonly HttpClient httpClient;

        public RowCardServiceClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<long> JoinAsync(string playerName)
        {
            string addPlayerPath = $"players/{playerName}";

            var response = await httpClient.PostAsync(addPlayerPath, null);

            if (response.IsSuccessStatusCode)
            {
                return Convert.ToInt64(await response.Content.ReadAsStringAsync());
            }

            return -1;
        }

        public async Task<int> CreateGameAsync()
        {
            string gamePath = RelativePathPart;

            var response = await httpClient.PostAsync(gamePath, null);

            if (response.IsSuccessStatusCode)
            {
                return Convert.ToInt32(await response.Content.ReadAsStringAsync());
            }

            return -1;
        }

        public async Task<string> SetStartingPlayerAsync(long playerId)
        {
            string gamePath = $"{RelativePathPart}/setStartingPlayer?playerId={playerId}";

            var response = await httpClient.PostAsync(gamePath, null);

            return response.ToString();
        }

        public async Task<string> ResetAsync()
        {
            string gamePath = $"{RelativePathPart}/reset";

            var response = await httpClient.PostAsync(gamePath, null);

            return response.ToString();
        }

        public async Task<string> GetStatusAsync()
        {
            string gamePath = $"{RelativePathPart}/status";

            var response = await httpClient.GetStringAsync(gamePath, CancellationToken.None);

            return response;
        }
    }
}
