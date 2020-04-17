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

            var result = board.IsEmpty();

            // then
            Assert.True(result);
        }
    }
}
