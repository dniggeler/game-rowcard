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