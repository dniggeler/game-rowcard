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

        private readonly ConcurrentDictionary<string, Player> players =
            new ConcurrentDictionary<string, Player>();

        public GameStateBase(Random rnd)
        {
            this.rnd = rnd;
        }

        public int NumberOfPlayers => players.Count;

        public ICollection<Player> GetPlayers()
        {
            return players.Values;
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
    }
}