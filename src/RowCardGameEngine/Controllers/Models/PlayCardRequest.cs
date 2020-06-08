using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Controllers.Models
{
    public class PlayCardRequest
    {
        public long PlayerId { get; set; }
        public Suits Suit { get; set; }
        public Ranks Rank { get; set; }
    }
}