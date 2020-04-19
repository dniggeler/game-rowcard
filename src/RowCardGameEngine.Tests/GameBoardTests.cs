using RowCardGameEngine.Game.Models;
using Xunit;
using Xunit.Abstractions;

namespace RowCardGameEngine.Tests
{
    [Trait("Game Engine", "Game Board")]
    public class GameBoardTests : IClassFixture<GameEngineFixture>
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly GameEngineFixture _fixture;

        public GameBoardTests(ITestOutputHelper outputHelper, GameEngineFixture fixture)
        {
            this.outputHelper = outputHelper;
            _fixture = fixture;
        }

        [Fact(DisplayName = "Create Empty Board")]
        public void ShouldCreateEmptyBoard()
        {
            // given

            // when
            var board = _fixture.GetService<GameBoard>();
            board.Clear();

            var result = board.IsEmpty();

            // then
            Assert.True(result);
        }

        [Fact(DisplayName = "Clear Board")]
        public void ShouldClearBoard()
        {
            // given
            Card newCard = new Card(Suits.Spades, Ranks.Eight);

            // when
            var board = _fixture.GetService<GameBoard>();

            var conditionNewCard = board.SetStartingCard(newCard).IsRight;
            board.Clear();
            var result = conditionNewCard && board.IsEmpty();

            // then
            Assert.True(result);
        }

        [Fact(DisplayName = "Add Starting Card")]
        public void ShouldAddStartingCard()
        {
            // given
            Card startingCard = new Card(Suits.Clubs, Ranks.Seven);

            // when
            var board = _fixture.GetService<GameBoard>();
            var success = board.SetStartingCard(startingCard).IsRight;

            var result = success && !board.IsEmpty();

            // then
            Assert.True(result);
        }

        [Fact(DisplayName = "Add Cards")]
        public void ShouldAddCards()
        {
            // given
            Card startingCard = new Card(Suits.Clubs, Ranks.Seven);
            Card startCard = new Card(Suits.Diamonds, Ranks.Seven);
            Card lowCard = new Card(Suits.Clubs, Ranks.Six);
            Card highCard = new Card(Suits.Clubs, Ranks.Eight);

            // when
            var board = _fixture.GetService<GameBoard>();
            board.Clear();

            var success = board.SetStartingCard(startingCard).IsRight;

            success = success && board.AddCard(lowCard).IsRight;
            success = success && board.AddCard(highCard).IsRight;
            success = success && board.AddCard(startingCard).IsRight;

            var result = success && !board.IsEmpty();

            // then
            Assert.True(result);
        }
    }
}
