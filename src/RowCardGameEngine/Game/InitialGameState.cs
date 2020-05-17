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

        public bool CanAddPlayer => true;

        public Either<string, IGameState> Setup(GameBoard gameBoard, int numberOfPlayers)
        {
            if (numberOfPlayers < GameConfiguration.MinPlayers)
            {
                return "Not enough players";
            }

            if (numberOfPlayers > GameConfiguration.MaxPlayers)
            {
                return $"Max players {GameConfiguration.MaxPlayers} exceeded";
            }

            return new SetupGameState(Rnd, gameBoard);
        }
    }
}