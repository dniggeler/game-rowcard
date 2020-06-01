using LanguageExt;
using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Game
{
    public class AutonomousPlayer : IAutonomousPlayer
    {
        public Option<Card> GetNextCard(Hand hand, GameBoard gameBoard)
        {
            return Option<Card>.None;
        }
    }
}