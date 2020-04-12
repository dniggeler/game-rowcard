using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;
using Xunit;
using Xunit.Abstractions;

namespace RowCardGameEngine.Tests
{
    [Trait("Game Engine", "Deck")]
    public class DeckTests
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly Random rnd;

        public DeckTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.rnd = new Random(1);
        }

        [Fact(DisplayName = "Create Deck")]
        public void ShouldCreateDeck()
        {
            // given

            // when
            var deck = new Deck(rnd);

            // then
            Assert.False(deck.IsEmpty());
        }

        [Fact(DisplayName = "Remove Card from Deck")]
        public void ShouldRemoveCardFromDeck()
        {
            // given
            var deck = new Deck(rnd);

            // when
            Option<(Suits, Ranks)> card = deck.DequeueCard();

            // then
            Assert.True(card.IsSome);
            card.IfSome(c => outputHelper.WriteLine(Deck.ToString(c)));
        }
    }
}
