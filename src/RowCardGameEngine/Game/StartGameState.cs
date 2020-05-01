using System;
using System.Linq;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class StartGameState : GameStateBase, IGameState
    {
        public StartGameState(Random rnd, GameBoard gameBoard) : base(rnd, gameBoard)
        {
        }

        public Either<string, (long GameId, long GameStateId)> GetId()
        {
            throw new NotImplementedException();
        }

        public new Either<string, long> AddPlayer(string playerName)
        {
            return AddPlayerNotPossible(playerName);
        }

        Either<string, IGameState> IGameState.Start(long playerId)
        {
            return "Game has already started";
        }

        public Either<string, IGameState> PlayCard(long playerId, Card card)
        {
            return GameBoard
                .SetStartingCard(card)
                .Map<IGameState>(_ => new PlayGameState(Rnd, GameBoard));
        }

        public Either<string, IGameState> Setup(GameBoard gameBoard)
        {
            return "Game has already setup";
        }

        public Either<string, IGameState> Finish()
        {
            return "Game is not yet finished";
        }
    }
}