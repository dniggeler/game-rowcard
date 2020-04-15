using System;
using System.Collections.Generic;

namespace RowCardGameEngine.Game.Models
{
    public class Hand
    {
        private readonly int numberOfCards;
        private readonly List<Card> cards = new List<Card>();

        public Hand(int numberOfCards)
        {
            this.numberOfCards = numberOfCards;
        }

        public bool IsEmpty => cards.Count == 0;

        public bool IsFull => cards.Count == numberOfCards;

        public void Add(Card card)
        {
            if (cards.Contains(card))
            {
                throw new ArgumentException($"Card {card.AsPokerKey()} already exists");
            }

            cards.Add(card);
        }

        public bool Remove(Card card)
        {
            return cards.Remove(card);
        }
    }
}