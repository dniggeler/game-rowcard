using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanguageExt;


namespace RowCardGameEngine.Game.Models
{
    public class Deck
    {
        private readonly Random rnd;
        private const int NumberOfCards = NumberOfRanks * NumberOfSuits;
        private const int NumberOfRanks = 9;
        private const int NumberOfSuits = 4;

        readonly Queue<(Suits, Ranks)> deckQueue = new Queue<(Suits, Ranks)>();

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

        public Option<(Suits, Ranks)> DequeueCard()
        {
            if (deckQueue.Count == 0)
            {
                return Option<(Suits, Ranks)>.None;
            }

            return deckQueue.Dequeue();
        }

        public static string ToString((Suits, Ranks) card)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(card.Item1 switch
            {
                Suits.Clubs => "C",
                Suits.Spades => "S",
                Suits.Hearts => "H",
                Suits.Diamonds => "D",
                _ => ""
            });

            sb.Append(card.Item2 switch
            {
                Ranks.Ace => "A",
                Ranks.King => "K",
                Ranks.Queen => "Q",
                Ranks.Jack => "J",
                Ranks.Ten => "T",
                Ranks.Nine => "9",
                Ranks.Eight => "8",
                Ranks.Seven => "7",
                Ranks.Six => "6",
                _ => ""
            });

            return sb.ToString();
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

        private (Suits, Ranks) Derive(int i)
        {
            int rank = i % NumberOfRanks;
            int suit = i / NumberOfRanks;

            return ((Suits)suit, (Ranks)rank);
        }
    }
}