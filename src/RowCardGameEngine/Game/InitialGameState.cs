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

        public Either<string, (long GameId, long GameStateId)> GetId()
        {
            throw new NotImplementedException();
        }

        public Either<string, long> Start()
        {
            return "Change not allowed";
        }

        public IGameState Setup(GameBoard gameBoard)
        {
            throw new NotImplementedException();
        }

        public Either<string, FinalGameResult> Finish()
        {
            return "Change not allowed";
        }
    }
}