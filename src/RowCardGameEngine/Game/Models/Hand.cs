using System;
using LanguageExt;

namespace RowCardGameEngine.Game.Models
{
    public class Hand
    {
        private readonly int numberOfCards;
        private HashSet<Card> cards = HashSet<Card>.Empty;

        public Hand(int numberOfCards)
        {
            this.numberOfCards = numberOfCards;
        }

        public HashSet<Card> GetCards() => cards;

        public int Count => cards.Count;

        public bool IsEmpty => cards.Count == 0;

        public bool IsFull => cards.Count == numberOfCards;

        public void Add(Card card)
        {
            if (cards.Contains(card))
            {
                throw new ArgumentException($"Card {card.AsPokerKey()} already exists");
            }

            cards = cards.Add(card);
        }

        public bool Remove(Card card)
        {
            if (cards.Contains(card))
            {
                cards = cards.Remove(card);

                return true;
            }

            return false;
        }
    }
}