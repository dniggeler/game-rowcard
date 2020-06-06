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
                from c in gameEngine.SetStartCard(startingPlayerId, startingCard)
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
                from c in gameEngine.SetStartCard(notStartingPlayerId, startingCard)
                select id;

            result.IfLeft(error => outputHelper.WriteLine(error));
            result.IfRight(_ => gameEngine.GetActionHistory().Iter(a => outputHelper.WriteLine(a)));

            // then
            Assert.True(result.IsLeft);
        }

        [Fact(DisplayName = "Return All Playable Cards")]
        public void ShouldReturnAllPlayableCards()
        {
            // given
            var startingCard = new Card(Suits.Clubs, Ranks.Ace);

            IGameEngine gameEngine = fixture.GetService<IGameEngine>();

            // when
            (long startingPlayer, long notStartingPlayer) = gameEngine.SetupWithTwoPlayer(startingCard);

            var playableCardsPlayer1
                = gameEngine.GetGameBoard()
                .Map(b => b.GetPlayableCards(startingPlayer))
                .IfLeft(HashSet<Card>.Empty);

            var playableCardsPlayer2
                = gameEngine.GetGameBoard()
                    .Map(b => b.GetPlayableCards(notStartingPlayer))
                    .IfLeft(HashSet<Card>.Empty);

            HashSet<Card> playableCards = new HashSet<Card>()
                .AddRange(playableCardsPlayer1)
                .AddRange(playableCardsPlayer2);

            HashSet<Card> expectedPlayableCards = GetExpectedFeasibleCards();

            var result = playableCards == expectedPlayableCards;

            // then
            Assert.True(result);

            LanguageExt.HashSet<Card> GetExpectedFeasibleCards()
            {
                var cards = new HashSet<Card>();

                return cards
                    .Add(new Card(Suits.Clubs, Ranks.King))
                    .Add(new Card(Suits.Diamonds, Ranks.Ace))
                    .Add(new Card(Suits.Hearts, Ranks.Ace))
                    .Add(new Card(Suits.Spades, Ranks.Ace));
            }
        }
    }
}
