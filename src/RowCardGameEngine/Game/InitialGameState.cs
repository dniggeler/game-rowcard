using System;
using AzulGameEngine.Game.Models;
using LanguageExt;

namespace AzulGameEngine.Game
{
    internal class InitialGameState : GameStateBase, IGameState
    {
        public InitialGameState(Random rnd)
            :base(rnd)
        {
        }

        public Either<string, (long GameId, long GameStateId)> Create()
        {
            return "Cannot start yet";
        }

        public Either<string, long> Start()
        {
            return "Change not allowed";
        }

        public Either<string, FinalGameResult> Finish()
        {
            return "Change not allowed";
        }
    }
}