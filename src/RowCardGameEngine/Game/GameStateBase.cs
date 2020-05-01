﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class GameStateBase
    {
        protected readonly Random Rnd;
        protected readonly GameBoard GameBoard;

        private readonly ConcurrentDictionary<long, Player> players =
            new ConcurrentDictionary<long, Player>();

        public GameStateBase(Random rnd)
        {
            this.Rnd = rnd;
        }

        public GameStateBase(Random rnd, GameBoard gameBoard)
        {
            this.Rnd = rnd;
            this.GameBoard = gameBoard;
        }

        public int NumberOfPlayers => players.Count;

        public ICollection<Player> GetPlayers()
        {
            return players.Values;
        }

        public Either<string, GameBoard> GetGameBoard()
        {
            if (GameBoard == null)
            {
                return "Game is not initialized";
            }

            return GameBoard;
        }

        public Either<string, long> AddPlayer(string playerName)
        {
            if (Contains(playerName))
            {
                return "Player already exist";
            }

            if (players.Count == GameConfiguration.MaxPlayers)
            {
                return "Game is full with players";
            }

            long newPlayerId = Rnd.Next();

            var newPlayer = players.AddOrUpdate(
                newPlayerId,
                id => new Player(id, false, playerName, DateTime.Now),
                (_, p) => p);

            return newPlayer.Id;
        }

        protected Either<string, long> AddPlayerNotPossible(string playerName)
        {
            return "Game already started, adding player is no more possible";
        }

        protected bool Contains(string name)
        {
            foreach (KeyValuePair<long, Player> keyValuePair in players)
            {
                if (keyValuePair.Value.Name == name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}