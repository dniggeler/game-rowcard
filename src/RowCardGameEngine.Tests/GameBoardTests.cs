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

        [Fact(DisplayName = "Is board full")]
        public void ShouldReturnBoardIsNotFull()
        {
            // given

            // when
            var board = _fixture.GetService<GameBoard>();
            board.Clear();

            var result = board.IsFull(Suits.Clubs);

            // then
            Assert.False(result);
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

            success = success && board.PushCard(lowCard).IsRight;
            success = success && board.PushCard(startCard).IsRight;
            success = success && board.PushCard(highCard).IsRight;

            var result = success && !board.IsEmpty();

            // then
            Assert.True(result);
        }

        [Fact(DisplayName = "Fail to add card")]
        public void ShouldFailToAddCard()
        {
            // given
            Card startingCard = new Card(Suits.Clubs, Ranks.Seven);
            Card highCard = new Card(Suits.Clubs, Ranks.Queen);

            // when
            var board = _fixture.GetService<GameBoard>();
            board.Clear();

            var success = board.SetStartingCard(startingCard).IsRight;

            success = success && board.PushCard(highCard).IsLeft;

            var result = success && !board.IsEmpty();

            // then
            Assert.True(result);
        }
    }
}
