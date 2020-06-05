using LanguageExt;
using Moq;
using RowCardGameEngine.Game;
using RowCardGameEngine.Game.Models;
using Xunit;

namespace RowCardGameEngine.Tests
{
    [Trait("Client", "Autonomous Player")]
    public class AutonomousPlayerTests
    {
        private const long PlayerNoCard = 1;
        private const long PlayerWithCard = 2;
        private readonly Card playableCard = new Card(Suits.Clubs, Ranks.King);

        private readonly Mock<IGameBoardInfo> gameBoard;
        
        public AutonomousPlayerTests()
        {
            gameBoard = new Mock<IGameBoardInfo>();
            gameBoard.Setup(m => m.GetFeasibleCards())
                .Returns(HashSet<Card>.Empty);
            gameBoard.Setup(m => m.GetPlayableCards(PlayerNoCard))
                .Returns(HashSet<Card>.Empty);

            HashSet<Card> playableCards = HashSet<Card>.Empty;
            playableCards = playableCards.Add(playableCard);

            gameBoard.Setup(m => m.GetPlayableCards(PlayerWithCard))
                .Returns(playableCards);
        }

        [Fact(DisplayName = "No Card To Play")]
        public void ShouldReturnNoCard()
        {
            // given
            IAutonomousPlayer player = new AutonomousPlayer(PlayerNoCard);

            // when
            var result = player.GetNextCard(gameBoard.Object);

            // then
            Assert.True(result.IsNone);
        }

        [Fact(DisplayName = "With Card To Play")]
        public void ShouldReturnCard()
        {
            // given
            IAutonomousPlayer player = new AutonomousPlayer(PlayerWithCard);

            // when
            var result = player.GetNextCard(gameBoard.Object);

            // then
            Assert.True(result.IsSome);
            result.IfSome(c => Assert.True(c == playableCard));
        }
    }
}
