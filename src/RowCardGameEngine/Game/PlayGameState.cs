using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class PlayGameState : GameStateBase, IGameState
    {
        public PlayGameState(Random rnd, GameBoard gameBoard)
            : base(rnd, gameBoard)
        {
        }

        public Either<string, IGameState> PlayCard(long playerId, Card card)
        {
            return GameBoard
                .PushCard(card)
                .Map<IGameState>(_ => this);
        }
    }
}