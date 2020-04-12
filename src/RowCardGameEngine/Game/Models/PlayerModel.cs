using System;


namespace RowCardGameEngine.Game.Models
{
    public class PlayerModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}