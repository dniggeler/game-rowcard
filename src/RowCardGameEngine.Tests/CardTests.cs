using System;
using LanguageExt;
using RowCardGameEngine.Game.Models;
using Xunit;
using Xunit.Abstractions;

namespace RowCardGameEngine.Tests
{
    [Trait("Game Engine", "Card")]
    public class CardTests
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly Random rnd;

        public CardTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.rnd = new Random(1);
        }

        [Fact(DisplayName = "Create Card")]
        public void ShouldCreateCard()
        {
            // given
            string expectedResult = "HJ";
            // when
            var card = new Card(Suits.Hearts, Ranks.Jack);
            var result = card.AsPokerKey().IfNone("");

            // then
            Assert.False(string.IsNullOrEmpty(result));
            Assert.True(expectedResult == result);
        }

        [Fact(DisplayName = "Cards are equal")]
        public void ShouldConsiderCardsAsEqual()
        {
            // given

            // when
            var cardA = new Card(Suits.Hearts, Ranks.Jack);
            var cardB = new Card(Suits.Hearts, Ranks.Jack);
            var result = cardA == cardB;

            // then
            Assert.True(result);
        }
    }
}
