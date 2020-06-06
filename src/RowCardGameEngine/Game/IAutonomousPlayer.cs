using LanguageExt;
using RowCardGameEngine.Game.Models;

namespace RowCardGameEngine.Game
{
    public interface IAutonomousPlayer
    {
        Option<Card> GetNextCard(IGameBoardInfo gameBoard);

        Option<Card> GetStartingCard(IGameBoardInfo gameBoard);

    }
}