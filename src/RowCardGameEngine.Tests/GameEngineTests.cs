using LanguageExt;
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
        private readonly GameEngineFixture fixture;

        public GameEngineTests(ITestOutputHelper outputHelper, GameEngineFixture fixture)
        {
            this.outputHelper = outputHelper;
            this.fixture = fixture;
        }

        [Fact(DisplayName = "Fail Engine Setup")]
        public void ShouldFailSetupGameEngine()
        {
            // given

            // when
            var engine = fixture.GetService<IGameEngine>();
            bool result = engine.Setup().IsLeft;

            // then
            Assert.True(result);
        }

        [Fact(DisplayName = "Engine Ready to Play")]
        public void ShouldSuccessfullySetupGameEngineReadyToPlay()
        {
            // given
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);
            IGameEngine gameEngine = fixture.GetService<IGameEngine>();

            // when
            long startingPlayerId = gameEngine.AddPlayer(TestHelpers.PlayerNameA).IfLeft(0);
            long notStartingPlayerId = gameEngine.AddPlayer(TestHelpers.PlayerNameB).IfLeft(0);

            var result = from id in gameEngine.Setup()
                from p in gameEngine.SetStartingPlayer(startingPlayerId)
                from c in gameEngine.SetStartingCard(startingPlayerId, startingCard)
                select id;

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
            IGameEngine gameEngine = fixture.GetService<IGameEngine>();

            // when
            long startingPlayerId = gameEngine.AddPlayer(TestHelpers.PlayerNameA).IfLeft(0);
            long notStartingPlayerId = gameEngine.AddPlayer(TestHelpers.PlayerNameB).IfLeft(0);

            var result = from id in gameEngine.Setup()
                from p in gameEngine.SetStartingPlayer(startingPlayerId)
                from c in gameEngine.SetStartingCard(notStartingPlayerId, startingCard)
                select id;

            result.IfLeft(error => outputHelper.WriteLine(error));
            result.IfRight(_ => gameEngine.GetActionHistory().Iter(a => outputHelper.WriteLine(a)));

            // then
            Assert.True(result.IsLeft);
        }

        [Fact(DisplayName = "Return All Possible Cards")]
        public void ShouldReturnAllPossibleCards()
        {
            // given
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);

            IGameEngine gameEngine = fixture.GetService<IGameEngine>();

            // when
            (long startingPlayer, long notStartingPlayer) = gameEngine.SetupWithTwoPlayer(startingCard);

            bool result
                = gameEngine.GetGameBoard()
                .Map(b => b.GetAllPossibleCards())
                .Match(
                    Right: h => h.SetEquals(GetExpectedPossibleCards()),
                    Left: _ => false);

            // then
            Assert.True(result);

            System.Collections.Generic.HashSet<Card> GetExpectedPossibleCards()
            {
                return new System.Collections.Generic.HashSet<Card>
                {
                    new Card(Suits.Clubs, Ranks.King),
                    new Card(Suits.Diamonds, Ranks.Ace),
                    new Card(Suits.Hearts, Ranks.Ace),
                    new Card(Suits.Spades, Ranks.Ace),
                };
            }
        }
    }
}
