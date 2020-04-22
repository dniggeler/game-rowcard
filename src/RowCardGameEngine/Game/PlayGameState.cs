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
            return "Game already started, adding player is no more possible";
        }

        public Either<string, (long GameId, long GameStateId)> Create()
        {
            return "Game has already started";
        }

        public Either<string, long> Start()
        {
            throw new NotImplementedException();
        }

        public Either<string, FinalGameResult> Finish()
        {
            return "Game is not finished";
        }
    }
}