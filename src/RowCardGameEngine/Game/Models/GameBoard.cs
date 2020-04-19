using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace RowCardGameEngine.Game.Models
{
    public class GameBoard
    {
        private readonly ILogger<GameBoard> logger;

        private readonly Dictionary<Suits,Stack<Card>> highStacks = new Dictionary<Suits, Stack<Card>>();
        private readonly Dictionary<Suits,Stack<Card>> lowStacks = new Dictionary<Suits, Stack<Card>>();
        private readonly Dictionary<Suits, Card> startCards = new Dictionary<Suits, Card>();
        private Card startingCard;

        public GameBoard(ILogger<GameBoard> logger)
        {
            this.logger = logger;

            foreach (var s in GetSuitsValues())
            {
                lowStacks.Add(s, new Stack<Card>());
                highStacks.Add(s, new Stack<Card>());
            }
        }

        public Either<string, Unit> SetStartingCard(Card card)
        {
            if (card == null)
            {
                return "card is null";
            }

            if (!IsEmpty())
            {
                return "Game board ist not empty";
            }

            startCards[card.Suit] = card;
            startingCard = card;

            logger.LogDebug($"Starting with card {card.AsPokerKey()}");

            return Unit.Default;
        }

        public void Clear()
        {
            startingCard = null;
            startCards.Clear();

            GetSuitsValues()
                .Iter(s =>
                {
                    lowStacks[s].Clear();
                    highStacks[s].Clear();
                });
        }

        public bool IsEmpty()
        {
            if (startingCard == null)
            {
                return true;
            }

            return false;
        }

        public Either<string, Unit> PushCard(Card card)
        {
            if (card == null)
            {
                return "card is null";
            }

            if (IsEmpty())
            {
                return "set starting card first";
            }

            if(IsStartingCard())
            {
                startCards[card.Suit] = card;

                return Unit.Default;
            }

            return AddCardInternal(card);

            bool IsStartingCard()
            {
                if (startCards.ContainsKey(card.Suit))
                {
                    return false;
                }

                if (startingCard.Rank == card.Rank)
                {
                    return true;
                }

                return false;
            }
        }

        private Either<string, Unit> AddCardInternal(Card card)
        {
            var stack = GetStack(); // high or low

            if (stack.Count == 0)
            {
                Card startCard = startCards[card.Suit];

                return PushCard(startCard);
            }

            if (stack.TryPeek(out Card popCard))
            {
                return PushCard(popCard);
            }

            return $"No stack card available for card {card.AsPokerKey()}";

            Either<string, Unit> PushCard(Card compareCard)
            {
                var valid = (compareCard - card)
                    .Map(Math.Abs)
                    .Map(diff => diff == decimal.One)
                    .IfNone(false);

                if (!valid)
                {
                    return $"Difference of card {card.AsPokerKey()} compared to start card {compareCard.AsPokerKey()} not valid";
                }

                stack.Push(card);

                return Unit.Default;
            }

            Stack<Card> GetStack()
            {
                Card suitStartingCard = startCards[card.Suit];

                if (card < suitStartingCard)
                {
                    return lowStacks[card.Suit];
                }

                return highStacks[card.Suit];
            }
        }

        private static IEnumerable<Suits> GetSuitsValues()
        {
            return Enum.GetValues(typeof(Suits)).Cast<Suits>();
        }
    }
}