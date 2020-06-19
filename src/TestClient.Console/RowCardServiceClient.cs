using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace TestClient.Console
{
    public class RowCardServiceClient
    {
        private readonly string baseAddress;

        public RowCardServiceClient()
        {
            baseAddress = "https://localhost:44380/api/";
        }

        public async Task<long> JoinAsync(string playerName)
        {
            string addPlayerPath = $"players/{playerName}";

            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{baseAddress}");

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

            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{baseAddress}");

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

            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{baseAddress}");

            var response = await httpClient.PostAsync(gamePath, null);

            return response.ToString();
        }

        public async Task<string> ResetAsync()
        {
            string gamePath = "game/reset";

            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{baseAddress}");

            var response = await httpClient.PostAsync(gamePath, null);

            return response.ToString();
        }
    }
}
