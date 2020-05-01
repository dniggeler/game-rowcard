using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class InitialGameState : GameStateBase, IGameState
    {
        public InitialGameState(Random rnd)
            :base(rnd)
        {
        }

        public Either<string, IGameState> Start(long playerId)
        {
            return "Game has not yet setup";
        }

        public Either<string, IGameState> PlayCard(long playerId, Card card)
        {
            return "Game is not yet ready to play card";
        }

        public Either<string, IGameState> Setup(GameBoard gameBoard)
        {
            if (NumberOfPlayers < GameConfiguration.MinPlayers)
            {
                return "Not enough players";
            }

            if (NumberOfPlayers > GameConfiguration.MaxPlayers)
            {
                return $"Max players {GameConfiguration.MaxPlayers} exceeded";
            }

            return new SetupGameState(Rnd, gameBoard);
        }

        public Either<string, IGameState> Finish()
        {
            return "Change not allowed";
        }
    }
}