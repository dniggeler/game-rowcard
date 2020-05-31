using System.Collections.Generic;
using LanguageExt;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Game
{
    public interface IAutonomousPlayer
    {
        Option<Card> GetNextCard(Hand hand, GameBoard gameBoard);
    }

    public class AutonomousPlayer : IAutonomousPlayer
    {
        public Option<Card> GetNextCard(Hand hand, GameBoard gameBoard)
        {
            return Option<Card>.None;
        }
    }
}