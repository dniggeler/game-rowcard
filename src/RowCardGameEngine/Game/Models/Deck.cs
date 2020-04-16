using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace RowCardGameEngine.Game.Models
{
    public class Deck
    {
        private readonly Random rnd;
        private readonly ILogger<Deck> logger;
        private const int NumberOfCards = NumberOfRanks * NumberOfSuits;
        private const int NumberOfRanks = 9;
        private const int NumberOfSuits = 4;

        private readonly Queue<Card> deckQueue = new Queue<Card>();

        public Deck(Random rnd, ILogger<Deck> logger)
        {
            this.rnd = rnd;
            this.logger = logger;

            foreach (int randomValue in CreateUniqueRandomNumbers())
            {
                deckQueue.Enqueue(Derive(randomValue));
            }
        }

        public int Count => deckQueue.Count;

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
                Enumerable.Range(0, NumberOfCards)
                    .ToList();

            for (int ii = 0; ii < NumberOfCards; ii++)
            {
                var index = rnd.Next(0, NumberOfCards - ii);
                var value = listOfSequentialNumbers[index];

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