using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace RowCardGameEngine.Game.Models
{
    public class Deck
    {
        private readonly Random rnd;
        private const int NumberOfCards = NumberOfRanks * NumberOfSuits;
        private const int NumberOfRanks = 9;
        private const int NumberOfSuits = 4;

        readonly Queue<Card> deckQueue = new Queue<Card>();

        public Deck(Random rnd)
        {
            this.rnd = rnd;
            foreach (int randomValue in CreateUniqueRandomNumbers())
            {
                deckQueue.Enqueue(Derive(randomValue));
            }
        }

        public bool IsEmpty()
        {
            return deckQueue.Count == 0;
        }

        public Option<Card> DequeueCard()
        {
            if (deckQueue.Count == 0)
            {
                return Option<Card>.None;
            }

            return deckQueue.Dequeue();
        }

        private IEnumerable<int> CreateUniqueRandomNumbers()
        {
            var listOfSequentialNumbers =
                Enumerable.Range(1, NumberOfCards)
                    .ToList();
            for (int ii = 0; ii < NumberOfCards; ii++)
            {
                var value = rnd.Next(1, NumberOfCards - ii);
                listOfSequentialNumbers.Remove(value);

                yield return value;
            }
        }

        private Card Derive(int i)
        {
            int rank = i % NumberOfRanks;
            int suit = i / NumberOfRanks;

            return new Card((Suits)suit, (Ranks)rank);
        }
    }
}