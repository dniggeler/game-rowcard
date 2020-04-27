using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public class GameEngine
    {
        private readonly Random rnd;
        private readonly Func<GameBoard> createBoardFunc;

        private IGameState gameState;

        public GameEngine(Random rnd, Func<GameBoard> createBoardFunc)
        {
            this.rnd = rnd;
            this.createBoardFunc = createBoardFunc;

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
            var gameBoard = createBoardFunc();

            var r = gameBoard
                    .Setup(GetPlayers().ToList().AsReadOnly())
                    .Map(_ =>
                    {
                        gameState = gameState.Setup(gameBoard);


                    });
        }

        public Either<string,(long GameId, long GameStateId)> Start()
        {
            gameState.GetGameBoard()
                .IfRight(board => { gameState = new PlayGameState(rnd, board); });

            long stateId = 0;
            long gameId = rnd.Next();

            return (gameId, stateId);
        }
    }
}