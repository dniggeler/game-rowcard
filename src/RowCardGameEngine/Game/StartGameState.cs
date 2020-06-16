using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class StartGameState : GameStateBase, IGameState
    {
        public StartGameState(Random rnd, GameBoard gameBoard)
            : base(rnd, gameBoard)
        {
        }

        public Either<string, IGameState> PlayCard(long playerId, Card card)
        {
            return GameBoard
                .RemoveCardFromHand(playerId, card)
                .Where(v => v)
                .Bind(_ => GameBoard.SetStartingCard(card))
                .Map<IGameState>(_ => new PlayGameState(Rnd, GameBoard));
        }

        public Either<string, IGameState> Reset()
        {
            return new InitialGameState(Rnd);
        }
    }
}