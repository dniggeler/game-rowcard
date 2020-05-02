using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    internal class GameStateBase
    {
        protected readonly Random Rnd;
        protected readonly GameBoard GameBoard;

        public GameStateBase(Random rnd)
        {
            this.Rnd = rnd;
        }

        public GameStateBase(Random rnd, GameBoard gameBoard)
        {
            this.Rnd = rnd;
            this.GameBoard = gameBoard;
        }

        public Either<string, GameBoard> GetGameBoard()
        {
            if (GameBoard == null)
            {
                return "Game is not initialized";
            }

            return GameBoard;
        }
    }
}