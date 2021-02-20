using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RowCardGame
{
    public class RowCardServiceClient : IRowCardServiceClient
    {
        private readonly HttpClient httpClient;
        private readonly string baseAddress;

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
            string gamePath = "game";

            var response = await httpClient.PostAsync(gamePath, null);

            if (response.IsSuccessStatusCode)
            {
                return Convert.ToInt32(await response.Content.ReadAsStringAsync());
            }

            return -1;
        }

        public async Task<string> SetStartingPlayerAsync(long playerId)
        {
            string gamePath = $"game/setStartingPlayer?playerId={playerId}";

            var response = await httpClient.PostAsync(gamePath, null);

            return response.ToString();
        }

        public async Task<string> ResetAsync()
        {
            string gamePath = "game/reset";

            var response = await httpClient.PostAsync(gamePath, null);

            return response.ToString();
        }
    }
}
