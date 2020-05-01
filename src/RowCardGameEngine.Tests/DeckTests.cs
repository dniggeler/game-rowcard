using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using RowCardGameEngine.Game.Models;
using Xunit;
using Xunit.Abstractions;

namespace RowCardGameEngine.Tests
{
    [Trait("Game", "Deck")]
    public class DeckTests : IClassFixture<GameEngineFixture>
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly GameEngineFixture _fixture;

        public DeckTests(ITestOutputHelper outputHelper, GameEngineFixture fixture)
        {
            this.outputHelper = outputHelper;
            _fixture = fixture;
        }

        [Fact(DisplayName = "Create Deck")]
        public void ShouldCreateDeck()
        {
            // given

            // when
            var deck = _fixture.GetService<Deck>();

            // then
            Assert.False(deck.IsEmpty());
        }

        [Fact(DisplayName = "Deck contains no duplicates")]
        public void ShouldContainNoDuplicates()
        {
            // given

            // when
            var deck = _fixture.GetService<Deck>();

            List<Card> cards = new List<Card>();
            while (!deck.IsEmpty())
            {
                deck.DequeueCard()
                    .IfSome(c => cards.Add(c));
            }

            var result =
                cards.GroupBy(c => c.AsPokerKey()
                    .IfNone("na"))
                    .Select(g => g.Count())
                    .Where(count => count>1)
                    .ToList();

            // then
            Assert.True(result.Count==0);
        }

        [Fact(DisplayName = "Remove Card from Deck")]
        public void ShouldRemoveCardFromDeck()
        {
            // given

            // when
            var deck = _fixture.GetService<Deck>();

            Option<Card> card = deck.DequeueCard();

            // then
            Assert.True(card.IsSome);
        }

        [Fact(DisplayName = "Distribute Deck to Hands")]
        public void ShouldDistributeDeckToHands()
        {
            // given

            // when
            var deck = _fixture.GetService<Deck>();

            List<Hand> hands = CreateHands(deck.Count).ToList();

            while (!deck.IsEmpty())
            {
                foreach (var hand in hands)
                {
                    deck.DequeueCard()
                        .IfSome(c => hand.Add(c));
                }
            }

            var result = hands.Aggregate(true, (a,h) => a && h.IsFull);

            // then
            Assert.True(deck.IsEmpty());
            Assert.True(result);
        }

        private IEnumerable<Hand> CreateHands(int deckSize)
        {
            var numberOfHands = 4;
            int handSize = deckSize / numberOfHands;

            var listOfHands = new List<Hand>();

            Enumerable
                .Range(0, numberOfHands)
                .Iter(_ => listOfHands.Add(new Hand(handSize)));

            return listOfHands;
        }
    }
}
