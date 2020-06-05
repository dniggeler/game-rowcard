namespace RowCardGameEngine.Game.Models
{
    public interface IGameBoardInfo
    {
        public LanguageExt.HashSet<Card> GetFeasibleCards();

        public LanguageExt.HashSet<Card> GetPlayableCards(long playerId);
    }
}