using System;
using System.Collections.Generic;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
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

        public ICollection<Player> GetPlayers()
        {
            return gameState.GetPlayers();
        }

        public Either<string, long> AddPlayer(string playerName)
        {
            return gameState.AddPlayer(playerName);
        }

        public Either<string, (long GameId, long GameStateId)> Setup()
        {
            gameState = new SetupGameState(rnd);

            long stateId = 0;
            long gameId = rnd.Next();

            return (gameId, stateId);
        }

        public Either<string,(long GameId, long GameStateId)> Start()
        {
            gameState = new PlayGameState(rnd);

            long stateId = 0;
            long gameId = rnd.Next();

            return (gameId, stateId);
        }
    }
}