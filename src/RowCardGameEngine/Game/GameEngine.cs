using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public class GameEngine : IGameEngine
    {
        private readonly int gameId;
        private readonly Random rnd;
        private readonly Func<GameBoard> createNewGameBoardFunc;
        private CircularPlayerList circularPlayerList;
        private long startingPlayerId;

        private IGameState gameState;

        private readonly ConcurrentDictionary<long, Player> players =
            new ConcurrentDictionary<long, Player>();

        private readonly List<string> actionHistory = new List<string>();

        public GameEngine(Random rnd, Func<GameBoard> createBoardFunc)
        {
            this.rnd = rnd;
            createNewGameBoardFunc = createBoardFunc;

            gameState = new InitialGameState(rnd);

            gameId = rnd.Next();
        }

        public ICollection<Player> GetPlayers()
        {
            return players.Values;
        }
        
        public Either<string, long> AddPlayer(string playerName)
        {
            if (!gameState.CanAddPlayer)
            {
                return "Game already started, adding player is no more possible";
            }

            if (Contains(playerName))
            {
                return "Player already exist";
            }

            if (players.Count == GameConfiguration.MaxPlayers)
            {
                return "Game is full with players";
            }

            long newPlayerId = rnd.Next();

            var newPlayer = players.AddOrUpdate(
                newPlayerId,
                id => new Player(id, false, playerName, DateTime.Now),
                (_, p) => p);

            actionHistory.Add($"player {playerName}, {newPlayer.Id} added");

            return newPlayer.Id;

            bool Contains(string name)
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

        public Either<string, int> Setup()
        {
            var gameBoard = createNewGameBoardFunc();

            Either<string, IGameState> r =
                from board in gameBoard.Setup(GetPlayers().ToList().AsReadOnly())
                from newState in gameState.Setup(board, GetPlayers().Count)
                select newState;

            r.Iter(newState =>
            {
                gameState = newState;
            });

            actionHistory.Add($"Game {gameId} setup");

            return r.Map(_ => gameId);
        }

        public Either<string, Unit> SetStartingPlayer(long playerId)
        {
            return gameState.Start()
                    .Bind<Unit>(s =>
                    {
                        startingPlayerId = playerId;

                        SetupPlayerCircle();
                        gameState = s;
                        actionHistory.Add($"Player {playerId} set as starter");

                        return Unit.Default;
                    });
        }

        public Either<string, Unit> SetStartCard(long playerId, Card card)
        {
            if (playerId != startingPlayerId)
            {
                return "Player not allowed to play first card";
            }

            return gameState
                .PlayCard(playerId, card)
                .Bind<Unit>(newState =>
                {

                    actionHistory.Add($"Player {playerId} set {card} as starting card");

                    gameState = newState;

                    circularPlayerList.GetNext();

                    return Unit.Default;
                });
        }

        public Either<string, Unit> PlayCard(long playerId, Card card)
        {
            if (playerId != circularPlayerList.CurrentPlayer)
            {
                return $"Current player is {circularPlayerList.CurrentPlayer} not player {playerId}";
            }

            return gameState
                .PlayCard(playerId, card)
                .Map(newState =>
                {
                    actionHistory.Add($"Player {playerId} played card {card.Suit}/{card.Rank}");
                    gameState = newState;

                    circularPlayerList.GetNext();

                    return Unit.Default;
                });
        }

        public Either<string, GameBoard> GetGameBoard()
        {
            return gameState.GetGameBoard();
        }

        public Either<string, int> Reset()
        {
            var gameBoard = createNewGameBoardFunc();

            Either<string, IGameState> r =
                from board in gameBoard.Setup(GetPlayers().ToList().AsReadOnly())
                from newState in gameState.Setup(board, GetPlayers().Count)
                select newState;

            r.Iter(newState =>
            {
                gameState = newState;
            });

            actionHistory.Add($"Game {gameId} reset");

            return r.Map(_ => gameId);
        }

        public IReadOnlyCollection<string> GetActionHistory()
        {
            return actionHistory;
        }

        private void SetupPlayerCircle()
        {
            var playerList =
                players
                    .Select(p => p.Key)
                    .ToList()
                    .AsReadOnly();

            circularPlayerList = new CircularPlayerList(playerList, startingPlayerId);
        }
    }
}