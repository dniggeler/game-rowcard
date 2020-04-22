using System;
using System.Collections.Generic;
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
        private readonly Random _rnd;

        public GameBoardTests(ITestOutputHelper outputHelper, GameEngineFixture fixture)
        {
            this.outputHelper = outputHelper;
            _fixture = fixture;
            _rnd = new Random(1);
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
            Card startingCard = new Card(Suits.Hearts, Ranks.Ace);
            Card card6 = new Card(Suits.Hearts, Ranks.Six);
            Card card7 = new Card(Suits.Hearts, Ranks.Seven);
            Card card8 = new Card(Suits.Hearts, Ranks.Eight);
            Card card9 = new Card(Suits.Hearts, Ranks.Nine);
            Card card10 = new Card(Suits.Hearts, Ranks.Ten);
            Card card11 = new Card(Suits.Hearts, Ranks.Jack);
            Card card12 = new Card(Suits.Hearts, Ranks.Queen);
            Card card13 = new Card(Suits.Hearts, Ranks.King);

            // when
            var board = _fixture.GetService<GameBoard>();
            board.Clear();

            board.SetStartingCard(startingCard);
            board.PushCard(card13);
            board.PushCard(card12);
            board.PushCard(card11);
            board.PushCard(card10);
            board.PushCard(card9);
            board.PushCard(card8);
            board.PushCard(card7);
            board.PushCard(card6);

            var result = board.IsFull(Suits.Hearts);

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

        [Fact(DisplayName = "Setup Board")]
        public void ShouldSetNumberOfPlayers()
        {
            // given
            var players = new List<Player>
            {
                Create(false),
                Create(true)
            };

            // when
            var board = _fixture.GetService<GameBoard>();
            board.Clear();

            var result = board.Setup(players).IsRight;

            // then
            Assert.True(result);
        }

        private Player Create(bool isMachinePlayer)
        {
            long id = _rnd.Next(1, 1000);
            return new Player(id, isMachinePlayer,$"{id}_test", DateTime.Now);
        }
    }
}
