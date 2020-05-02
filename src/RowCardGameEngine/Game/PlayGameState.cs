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

        Either<string, IGameState> IGameState.Start()
        {
            return "Game has already started";
        }

        public Either<string, IGameState> PlayCard(long playerId, Card card)
        {
            return GameBoard
                .PushCard(card)
                .Map<IGameState>(_ => this);
        }

        public Either<string, IGameState> Setup(GameBoard gameBoard, int numberOfPlayers)
        {
            return "Game has already setup";
        }
    }
}