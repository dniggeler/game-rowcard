using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class GameStateBase
    {
        private readonly Random rnd;
        private readonly GameBoard gameBoard;

        private readonly ConcurrentDictionary<string, Player> players =
            new ConcurrentDictionary<string, Player>();

        public GameStateBase(Random rnd)
        {
            this.rnd = rnd;
        }

        public GameStateBase(Random rnd, GameBoard gameBoard)
        {
            this.rnd = rnd;
            this.gameBoard = gameBoard;
        }

        public int NumberOfPlayers => players.Count;

        public ICollection<Player> GetPlayers()
        {
            return players.Values;
        }

        public Either<string, GameBoard> GetGameBoard()
        {
            if (gameBoard == null)
            {
                return "Game is not initialized";
            }

            return gameBoard;
        }

        public Either<string, long> AddPlayer(string playerName)
        {
            if (players.ContainsKey(playerName))
            {
                return "Player already exist";
            }

            if (players.Count == GameConfiguration.MaxPlayers)
            {
                return "Game is full with players";
            }

            var newPlayer = players.AddOrUpdate(
                playerName,
                name => new Player(rnd.Next(),false, name, DateTime.Now),
                (_, p) => p);

            return newPlayer.Id;
        }

        protected Either<string, long> AddPlayerNotPossible(string playerName)
        {
            return "Game already started, adding player is no more possible";
        }
    }
}