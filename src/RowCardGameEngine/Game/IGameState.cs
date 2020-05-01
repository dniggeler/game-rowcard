using System.Collections.Generic;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public interface IGameState
    {
        int NumberOfPlayers { get; }

        Either<string, GameBoard> GetGameBoard();

        ICollection<Player> GetPlayers();

        Either<string, long> AddPlayer(string playerName);

        Either<string, IGameState> Start();

        Either<string, IGameState> PlayCard(long playerId, Card card);

        Either<string, IGameState> Setup(GameBoard gameBoard);

        Either<string, IGameState> Finish();
    }
}