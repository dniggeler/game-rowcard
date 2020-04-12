using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AzulGameEngine.Game.Models;
using LanguageExt;
using RowCardGameEngine.Game.Models;


namespace AzulGameEngine.Game
{
    public class GameEngine
    {
        private readonly Random rnd;

        private IGameState gameState;

        public GameEngine(Random rnd)
        {
            this.rnd = rnd;

            gameState = new InitialGameState(rnd);
        }

        public int NumberOfPlayers => gameState.NumberOfPlayers;

        public ICollection<PlayerModel> GetPlayers()
        {
            return gameState.GetPlayers();
        }

        public Either<string, long> AddPlayer(string playerName)
        {
            return gameState.AddPlayer(playerName);
        }

        public Either<string, (long GameId, long GameStateId)> Create()
        {
            return gameState.Create();
        }

        public Either<string,(long GameId, long GameStateId)> Start()
        {
            gameState = new PlayGameState(this.rnd);

            long stateId = 0;
            long gameId = rnd.Next();

            return (gameId, stateId);
        }
    }
}