using System.Collections.Generic;
using RowCardGameEngine.Game;
using Xunit;

namespace RowCardGameEngine.Tests
{
    [Trait("Game", "Circular Player List")]
    public class CircularPlayerListTests
    {
        [Fact(DisplayName = "One Round Trip")]
        public void ShouldReturnStartingPlayer()
        {
            // given
            long startingPlayerId = 1;
            var players = new List<long>
            {
                startingPlayerId,
                2,
                3
            };

            // when
            var circularPlayerList = new CircularPlayerList(players, startingPlayerId);
            circularPlayerList.GetNext();
            circularPlayerList.GetNext();
            var result = circularPlayerList.GetNext()
                .IfNone(-1);

            // then
            Assert.True(result == startingPlayerId);
        }
    }
}
