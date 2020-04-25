﻿using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class StartGameState : GameStateBase, IGameState
    {
        public StartGameState(Random rnd) : base(rnd)
        {
        }

        public new Either<string, long> AddPlayer(string playerName)
        {
            return AddPlayerNotPossible(playerName);
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