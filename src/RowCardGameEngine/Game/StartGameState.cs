using System;
using System.Collections.Concurrent;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class StartGameState : GameStateBase, IGameState
    {
        public StartGameState(Random rnd, GameBoard gameBoard, ConcurrentDictionary<long, Player> players)
            : base(rnd, gameBoard, players)
        {
        }

        public new Either<string, long> AddPlayer(string playerName)
        {
            return AddPlayerNotPossible(playerName);
        }

        Either<string, IGameState> IGameState.Start()
        {
            return "Game has already started";
        }

        public Either<string, IGameState> PlayCard(long playerId, Card card)
        {
            return GameBoard
                .RemoveCardFromHand(playerId, card)
                .Where(v => v)
                .Bind(_ => GameBoard.SetStartingCard(card))
                .Map<IGameState>(_ => new PlayGameState(Rnd, GameBoard, Players));
        }

        public Either<string, IGameState> Setup(GameBoard gameBoard)
        {
            return "Game has already setup";
        }

        public Either<string, IGameState> Finish()
        {
            return "Game is not yet finished";
        }
    }
}