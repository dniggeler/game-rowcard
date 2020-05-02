using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<long, Player> players =
            new ConcurrentDictionary<long, Player>();

        private readonly List<string> actionHistory = new List<string>();

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
            return gameState.AddPlayer(playerName)
                .Map(id =>
                {
                    actionHistory.Add($"player {playerName}, {id} added");
                    return id;
                });
        }

        public Either<string, int> Setup()
        {
            var gameBoard = createNewGameBoardFunc();

            Either<string, IGameState> r =
                from board in gameBoard.Setup(GetPlayers().ToList().AsReadOnly())
                from newState in gameState.Setup(board)
                select newState;

            r.Iter(newState => gameState = newState);

            actionHistory.Add($"Game {GameId} setup");

            return r.Map(_ => GameId);
        }

        public Either<string, Unit> SetStartingPlayer(long playerId)
        {
            startingPlayerId = playerId;

            return gameState
                .Start()
                .Iter(newState =>
                {
                    actionHistory.Add($"Player {playerId} set as starter");
                    gameState = newState;
                });
        }

        public Either<string, Unit> SetStartingCard(long playerId, Card card)
        {
            if (playerId != startingPlayerId)
            {
                return "Player not allowed to play first card";
            }

            return gameState
                .PlayCard(playerId, card)
                .Iter(newState =>
                {
                    actionHistory.Add($"Player {playerId} set {card} as starting card");

                    gameState = newState;
                });
        }

        public Either<string, Unit> PlayCard(long playerId, Card card)
        {
            return gameState
                .PlayCard(playerId, card)
                .Iter(newState =>
                {
                    actionHistory.Add($"Player {playerId} played card {card}");
                    gameState = newState;
                });
        }

        public IReadOnlyCollection<string> GetActionHistory()
        {
            return actionHistory;
        }
    }
}