using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class PlayGameState : GameStateBase, IGameState
    {
        public PlayGameState(Random rnd) : base(rnd)
        {
        }

        public new Either<string, long> AddPlayer(string playerName)
        {
            return AddPlayerNotPossible(playerName);
        }

        public Either<string, long> Start()
        {
            throw new NotImplementedException();
        }

        public Either<string, long> Setup()
        {
            throw new NotImplementedException();
        }

        public Either<string, FinalGameResult> Finish()
        {
            return "Game is not finished";
        }
    }
}