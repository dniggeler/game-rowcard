using System;
using AzulGameEngine.Game.Models;
using LanguageExt;

namespace AzulGameEngine.Game
{
    internal class StartGameState : GameStateBase, IGameState
    {
        public StartGameState(Random rnd) : base(rnd)
        {
        }

        public new Either<string, long> AddPlayer(string playerName)
        {
            return "Game already started, adding player is no more possible";
        }

        public Either<string, (long GameId, long GameStateId)> Create()
        {
            return "Game has already started";
        }

        public Either<string, long> Start()
        {
            if (NumberOfPlayers < GameConfiguration.MinPlayers)
            {
                return "Not enough players";
            }

            if (NumberOfPlayers > GameConfiguration.MaxPlayers)
            {
                return $"Max players {GameConfiguration.MaxPlayers} exceeded";
            }

            return 1;
        }

        public Either<string, FinalGameResult> Finish()
        {
            return "Game is not finished";
        }
    }
}