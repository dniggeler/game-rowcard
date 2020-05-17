﻿using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class SetupGameState : GameStateBase, IGameState
    {
        public SetupGameState(Random rnd, GameBoard gameBoard)
            : base(rnd, gameBoard)
        {
        }

        public Either<string, IGameState> Start()
        {
            return new StartGameState(Rnd, GameBoard);
        }
    }
}