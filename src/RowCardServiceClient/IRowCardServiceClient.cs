using System.Threading.Tasks;

namespace RowCardGame
{
    public interface IRowCardServiceClient
    {
        Task<long> JoinAsync(string playerName);

        Task<int> CreateGameAsync();

        Task<string> SetStartingPlayerAsync(long playerId);

        Task<string> ResetAsync();

        Task<string> GetStatusAsync();
    }
}