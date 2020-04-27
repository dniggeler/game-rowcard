using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public interface IGameState
    {
        int NumberOfPlayers { get; }

        Either<string, (long GameId, long GameStateId)> GetId();

        Either<string, GameBoard> GetGameBoard();

        ICollection<Player> GetPlayers();

        Either<string, long> AddPlayer(string playerName);

        Either<string, long> Start();

        IGameState Setup(GameBoard gameBoard);

        Either<string, FinalGameResult> Finish();
    }
}