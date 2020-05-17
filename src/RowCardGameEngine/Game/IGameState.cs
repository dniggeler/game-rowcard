using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public interface IGameState
    {
        Either<string, GameBoard> GetGameBoard();

        bool CanAddPlayer => false;

        Either<string, IGameState> Start() => "Game has already started";

        Either<string, IGameState> PlayCard(long playerId, Card card) => "Game is not yet ready to play card";

        Either<string, IGameState> Setup(GameBoard gameBoard, int numberOfPlayers) => "Game has already setup";

        Either<string, IGameState> Finish() => "Game is not yet finished";
    }
}