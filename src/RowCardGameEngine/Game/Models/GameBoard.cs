using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RowCardGameEngine.Game.Models
{
    public class GameBoard
    {
        private readonly ILogger<GameBoard> logger;

        private readonly Stack<Card> highStack = new Stack<Card>();
        private readonly Stack<Card> lowStack = new Stack<Card>();
        private Card startCard;

        public GameBoard(ILogger<GameBoard> logger)
        {
            this.logger = logger;
        }

        public void AddStartCard(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            if (!IsEmpty())
            {
                logger.LogWarning("Game board ist not empty");
            }

            startCard = card;
        }

        public bool IsEmpty()
        {
            return highStack.Count == 0 && lowStack.Count == 0 && startCard == null;
        }
    }
}