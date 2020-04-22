using System.Collections.Generic;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public interface IGameState
    {
        int NumberOfPlayers { get; }

        ICollection<Player> GetPlayers();

        Either<string, long> AddPlayer(string playerName);

        Either<string, (long GameId, long GameStateId)> Create();

        Either<string, long> Start();

        Either<string, FinalGameResult> Finish();
    }
}