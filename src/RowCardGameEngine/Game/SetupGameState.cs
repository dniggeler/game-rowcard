using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class SetupGameState : GameStateBase, IGameState
    {
        public SetupGameState(Random rnd, GameBoard gameBoard) : base(rnd, gameBoard)
        {
        }

        public new Either<string, long> AddPlayer(string playerName)
        {
            return AddPlayerNotPossible(playerName);
        }

        public Either<string, IGameState> Start(long playerId)
        {
            if (NumberOfPlayers < GameConfiguration.MinPlayers)
            {
                return "Not enough players";
            }

            if (NumberOfPlayers > GameConfiguration.MaxPlayers)
            {
                return $"Max players {GameConfiguration.MaxPlayers} exceeded";
            }

            return new PlayGameState(Rnd, GameBoard);
        }

        public Either<string, IGameState> PlayCard(long playerId, Card card)
        {
            return "Game is not yet ready to play card";
        }

        public Either<string, IGameState> Setup(GameBoard gameBoard)
        {
            return "Game is already setup";
        }

        public Either<string, IGameState> Finish()
        {
            return "Game is not finished";
        }
    }
}