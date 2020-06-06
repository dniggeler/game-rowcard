using LanguageExt;

namespace RowCardGameEngine.Game.Models
{
    public interface IGameBoardInfo
    {
        public HashSet<Card> GetFeasibleCards();

        public HashSet<Card> GetPlayableCards(long playerId);

        public Option<Hand> GetHand(long playerId);
    }
}