using RowCardGameEngine.Game.Models;
using Xunit;
using Xunit.Abstractions;

namespace RowCardGameEngine.Tests
{
    [Trait("Game Engine", "Hand")]
    public class HandTests
    {
        private readonly ITestOutputHelper outputHelper;

        public HandTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact(DisplayName = "Create Full Hand")]
        public void ShouldCreateFullHand()
        {
            // given
            int numberOfCardsInHand = 3;

            // when
            var card1 = new Card(Suits.Hearts, Ranks.Jack);
            var card2 = new Card(Suits.Hearts, Ranks.Ten);
            var card3 = new Card(Suits.Clubs, Ranks.Ten);

            var hand = new Hand(numberOfCardsInHand);
            hand.Add(card1);
            hand.Add(card2);
            hand.Add(card3);

            var result = hand.IsFull;

            // then
            Assert.True(result);
        }
    }
}
