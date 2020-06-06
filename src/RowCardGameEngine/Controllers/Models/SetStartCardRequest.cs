using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Controllers.Models
{
    public class SetStartCardRequest
    {
        public long PlayerId { get; set; }
        public Suits Suit { get; set; }
        public Ranks Rank { get; set; }
    }
}