using System;
using System.Linq;
using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public class AutonomousPlayer : IAutonomousPlayer
    {
        private readonly long playerId;
        private readonly Random rnd;

        public AutonomousPlayer(long playerId)
        {
            this.playerId = playerId;
            rnd = new Random();
        }

        public Option<Card> GetNextCard(IGameBoardInfo gameBoard)
        {
            HashSet<Card> playableCards =
                gameBoard.GetPlayableCards(playerId);

            if (playableCards.IsEmpty)
            {
                return Option<Card>.None;
            }

            var cardIndex = rnd.Next(0, playableCards.Count-1);

            return playableCards.ToList()[cardIndex];
        }
    }
}