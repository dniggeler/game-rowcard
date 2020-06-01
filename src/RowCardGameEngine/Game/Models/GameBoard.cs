using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace RowCardGameEngine.Game.Models
{
    public class GameBoard
    {
        private readonly Deck deck;
        private Card startingCard;
        
        private readonly ILogger<GameBoard> logger;
        private readonly Dictionary<Suits,Stack<Card>> highStacks = new Dictionary<Suits, Stack<Card>>();
        private readonly Dictionary<Suits,Stack<Card>> lowStacks = new Dictionary<Suits, Stack<Card>>();
        private readonly Dictionary<Suits, Card> startCards = new Dictionary<Suits, Card>();
        private readonly Dictionary<long, Hand> hands = new Dictionary<long, Hand>();

        public GameBoard(ILogger<GameBoard> logger, Deck deck)
        {
            this.logger = logger;
            this.deck = deck;

            foreach (var s in GetSuitsValues())
            {
                lowStacks.Add(s, new Stack<Card>());
                highStacks.Add(s, new Stack<Card>());
            }
        }
        
        public int NumberOfRealPlayers { get; private set; }
        public int NumberOfMachinePlayers { get; private set; }

        public int NumberOfPlayers => NumberOfMachinePlayers + NumberOfRealPlayers;

        public Either<string, GameBoard> Setup(IReadOnlyCollection<Player> players)
        {
            int numberOfMachinePlayers = players.Count(p => p.IsMachinePlayer);
            int numberOfRealPlayers = players.Count - numberOfMachinePlayers;

            var result = SetNumberOfPlayers(numberOfRealPlayers, numberOfMachinePlayers);
            if (result.IsLeft)
            {
                return this;
            }

            // create hands
            int numberOfCards = deck.Count / NumberOfPlayers;
            foreach (Player player in players)
            {
                var hand = new Hand(numberOfCards);

                for (int ii = 0; ii < numberOfCards; ii++)
                {
                    deck.DequeueCard()
                        .IfSome(c => hand.Add(c));
                }

                hands[player.Id] = hand;
            }

            return this;
        }

        public Either<string, bool> RemoveCardFromHand(long playerId, Card card)
        {
            if (hands.TryGetValue(playerId, out Hand hand))
            {
                return hand.Remove(card);
            }

            return $"Player {playerId} unknown";
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

        public System.Collections.Generic.HashSet<Card> GetAllPossibleCards()
        {
            var possibleCards = new System.Collections.Generic.HashSet<Card>();

            foreach (var suit in GetSuitsValues())
            {
                GetLowStackCard(suit)
                    .Bind(Card.Predecessor)
                    .Map(c => possibleCards.Add(c))
                    .IfNone(() =>
                    {
                        if (!startCards.ContainsKey(suit))
                        {
                            possibleCards.Add(new Card(suit, startingCard.Rank));
                        }
                    });

                GetHighStackCard(suit)
                    .Bind(Card.Successor)
                    .Map(c => possibleCards.Add(c))
                    .IfNone(() =>
                    {
                        if (!startCards.ContainsKey(suit))
                        {
                            possibleCards.Add(new Card(suit, startingCard.Rank));
                        }
                    });
            }

            return possibleCards;
        }

        public Option<Card> GetHighStackCard(Suits suit)
        {
            if (highStacks[suit].Count == 0)
            {
                if (startCards.TryGetValue(suit, out Card card))
                {
                    return card;
                }
            }
            else
            {
                return highStacks[suit].Peek();
            }

            return Option<Card>.None;
        }

        public Option<Card> GetLowStackCard(Suits suit)
        {
            if (lowStacks[suit].Count == 0)
            {
                if (startCards.TryGetValue(suit, out Card card))
                {
                    return card;
                }
            }
            else
            {
                return lowStacks[suit].Peek();
            }

            return Option<Card>.None;
        }

        public void Clear()
        {
            NumberOfRealPlayers = 0;
            NumberOfMachinePlayers = 0;

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

        public bool IsFull(Suits suit)
        {
            if (!startCards.ContainsKey(suit))
            {
                return false;
            }

            Ranks minRank = startCards[suit].Rank;
            if (lowStacks[suit].Count > 0)
            {
                var topCard = lowStacks[suit].Peek();
                if (topCard.Rank < minRank)
                {
                    minRank = topCard.Rank;
                }
            }

            if (minRank != Deck.MinRank)
            {
                return false;
            }

            Ranks maxRank = startCards[suit].Rank;
            if (highStacks[suit].Count > 0)
            {
                var topCard = highStacks[suit].Peek();
                if (topCard.Rank > maxRank)
                {
                    maxRank = topCard.Rank;
                }
            }

            if (maxRank != Deck.MaxRank)
            {
                return false;
            }

            return true;
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
        
        private Either<string, Unit> SetNumberOfPlayers(int numberOfRealPlayers, int numberOfMachinePlayers)
        {
            int totalNumberOfPlayers = numberOfMachinePlayers + numberOfRealPlayers;

            if (totalNumberOfPlayers > GameConfiguration.MaxPlayers)
            {
                return $"Game allows {GameConfiguration.MaxPlayers} players only";
            }

            if (totalNumberOfPlayers < GameConfiguration.MinPlayers)
            {
                return $"Game needs at least {GameConfiguration.MinPlayers} players";
            }

            if (numberOfMachinePlayers > GameConfiguration.MaxMachinePlayers)
            {
                return $"Game allows {GameConfiguration.MaxMachinePlayers} machine players only";
            }

            NumberOfRealPlayers = numberOfRealPlayers;
            NumberOfMachinePlayers = numberOfMachinePlayers;

            return Unit.Default;
        }

        private static IEnumerable<Suits> GetSuitsValues()
        {
            return Enum.GetValues(typeof(Suits)).Cast<Suits>();
        }
    }
}