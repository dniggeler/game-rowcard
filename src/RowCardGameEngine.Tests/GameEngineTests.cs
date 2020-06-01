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
            var engine = _fixture.GetService<IGameEngine>();
            bool result = engine.Setup().IsLeft;

            // then
            Assert.True(result);
        }

        [Fact(DisplayName = "Engine Ready to Play")]
        public void ShouldSuccessfullySetupGameEngineReadyToPlay()
        {
            // given
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);
            IGameEngine gameEngine = _fixture.GetService<IGameEngine>();

            // when
            (long StartingPlayer, long NotStartingPlayer) t =
                gameEngine.SetupTwoPlayerEngine(startingCard);

            result.IfLeft(error => outputHelper.WriteLine(error));
            result.IfRight(_ => gameEngine.GetActionHistory().Iter(a => outputHelper.WriteLine(a)));

            // then
            Assert.True(result.IsRight);
        }

        [Fact(DisplayName = "Engine Not Ready to Play")]
        public void ShouldSuccessfullySetupGameEngineNotReadyToPlay()
        {
            // given
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);
            IGameEngine gameEngine = _fixture.GetService<IGameEngine>();

            // when
            (long StartingPlayer, long NotStartingPlayer) t =
                gameEngine.SetupTwoPlayerEngine(startingCard);

            var result = from id in gameEngine.Setup()
                from p in gameEngine.SetStartingPlayer(t.StartingPlayer)
                from c in gameEngine.SetStartingCard(t.NotStartingPlayer, startingCard)
                select id;

            result.IfLeft(error => outputHelper.WriteLine(error));
            result.IfRight(_ => gameEngine.GetActionHistory().Iter(a => outputHelper.WriteLine(a)));

            // then
            Assert.True(result.IsLeft);
        }


    }


}
