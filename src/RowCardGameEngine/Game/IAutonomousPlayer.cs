using System.Collections.Generic;
using LanguageExt;
using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Game
{
    public interface IAutonomousPlayer
    {
        Option<Card> GetNextCard(Hand hand, GameBoard gameBoard);
    }
}