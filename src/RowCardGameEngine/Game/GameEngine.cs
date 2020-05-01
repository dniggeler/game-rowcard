using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public class GameEngine
    {
        private readonly int GameId;
        private readonly Func<GameBoard> createNewGameBoardFunc;
        private long startingPlayerId;

        private IGameState gameState;

        public GameEngine(Random rnd, Func<GameBoard> createBoardFunc)
        {
            createNewGameBoardFunc = createBoardFunc;

            gameState = new InitialGameState(rnd);

            GameId = rnd.Next();
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

        public Either<string, int> Setup()
        {
            var gameBoard = createNewGameBoardFunc();

            Either<string, IGameState> r =
                from board in gameBoard.Setup(GetPlayers().ToList().AsReadOnly())
                from newState in gameState.Setup(board)
                select newState;

            r.Iter(newState => gameState = newState);

            return r.Map(_ => GameId);
        }

        public Either<string, Unit> SetStartingPlayer(long playerId)
        {
            startingPlayerId = playerId;

            return gameState
                .Start()
                .Iter(newState => gameState = newState);
        }

        public Either<string, Unit> SetStartingCard(long playerId, Card card)
        {
            if (playerId != startingPlayerId)
            {
                return "Player not allowed to play first card";
            }

            return gameState
                .PlayCard(playerId, card)
                .Iter(newState => gameState = newState);
        }

        public Either<string, Unit> PlayCard(long playerId, Card card)
        {
            return gameState
                .PlayCard(playerId, card)
                .Iter(newState => gameState = newState);
        }
    }
}