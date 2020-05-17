using System.Collections.Generic;
using LanguageExt;
using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Game
{
    public interface IGameEngine
    {
        IReadOnlyCollection<string> GetActionHistory();

        ICollection<Player> GetPlayers();

        Either<string, long> AddPlayer(string playerName);

        Either<string, int> Setup();

        Either<string, Unit> SetStartingCard(long playerId, Card card);

        Either<string, Unit> SetStartingPlayer(long playerId);

        Either<string, Unit> PlayCard(long playerId, Card card);
    }
}