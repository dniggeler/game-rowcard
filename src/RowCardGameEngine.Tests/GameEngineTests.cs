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
        private readonly string _playerNameA;
        private readonly string _playerNameB;

        public GameEngineTests(ITestOutputHelper outputHelper, GameEngineFixture fixture)
        {
            this.outputHelper = outputHelper;
            _fixture = fixture;

            _playerNameA = "a";
            _playerNameB = "b";

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
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);

            // when
            (GameEngine Engine, long StartingPlayer, long NotStartingPlayer) t
                = SetupTwoPlayerEngine();

            var result = from id in t.Engine.Setup()
                from p in t.Engine.SetStartingPlayer(t.StartingPlayer)
                from c in t.Engine.SetStartingCard(t.StartingPlayer, startingCard)
                select id;

            result.IfLeft(error => outputHelper.WriteLine(error));
            result.IfRight(_ => t.Engine.GetActionHistory().Iter(a => outputHelper.WriteLine(a)));

            // then
            Assert.True(result.IsRight);
        }

        [Fact(DisplayName = "Engine Not Ready to Play")]
        public void ShouldSuccessfullySetupGameEngineNotReadyToPlay()
        {
            // given
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);

            // when
            (GameEngine Engine, long StartingPlayer, long NotStartingPlayer) t
                = SetupTwoPlayerEngine();

            var result = from id in t.Engine.Setup()
                from p in t.Engine.SetStartingPlayer(t.StartingPlayer)
                from c in t.Engine.SetStartingCard(t.NotStartingPlayer, startingCard)
                select id;

            result.IfLeft(error => outputHelper.WriteLine(error));
            result.IfRight(_ => t.Engine.GetActionHistory().Iter(a => outputHelper.WriteLine(a)));

            // then
            Assert.True(result.IsLeft);
        }
        private (GameEngine Engine, long StartingPlayer, long NotStartingPlayer) SetupTwoPlayerEngine()
        {
            var engine = _fixture.GetService<GameEngine>();
            long startingPlayerId = engine.AddPlayer(_playerNameA).IfLeft(0);
            long notStartingPlayerId = engine.AddPlayer(_playerNameB).IfLeft(0);

            return (engine, startingPlayerId, notStartingPlayerId);
        }
    }


}
