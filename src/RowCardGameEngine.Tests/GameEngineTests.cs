using RowCardGameEngine.Game;
using RowCardGameEngine.Game.Models;
using Xunit;
using Xunit.Abstractions;

namespace RowCardGameEngine.Tests
{
    [Trait("Game", "Engine")]
    public class GameEngineTests : IClassFixture<GameEngineFixture>
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly GameEngineFixture _fixture;

        public GameEngineTests(ITestOutputHelper outputHelper, GameEngineFixture fixture)
        {
            this.outputHelper = outputHelper;
            _fixture = fixture;
        }

        [Fact(DisplayName = "Fail Engine Setup")]
        public void ShouldFailSetupGameEngine()
        {
            // given

            // when
            var engine = _fixture.GetService<GameEngine>();
            bool result = engine.Setup().IsLeft;

            // then
            Assert.True(result);
        }

        [Fact(DisplayName = "Engine Ready to Play")]
        public void ShouldSuccessfullySetupGameEngineReadyToPlay()
        {
            // given
            var playerA = "a";
            var playerB = "b";
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);

            // when
            var engine = _fixture.GetService<GameEngine>();
            long startingPlayerId = engine.AddPlayer(playerA).IfLeft(0);
            engine.AddPlayer(playerB);

            var result = engine.Setup()
                .Bind(id => engine.SetStartingPlayer(startingPlayerId))
                .Map(_ => engine.SetStartingCard(startingPlayerId, startingCard));

            result.IfLeft(error => outputHelper.WriteLine(error));
            result.IfRight(_ => engine.GetActionHistory().Iter(a => outputHelper.WriteLine(a)));

            // then
            Assert.True(result.IsRight);
        }
    }
}
