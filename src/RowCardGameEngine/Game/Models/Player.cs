using System;
using LanguageExt;

namespace RowCardGameEngine.Game.Models
{
    public class Player : Record<Player>
    {
        public readonly long Id;

        public bool IsMachinePlayer { get; set; }
        public string Name { get; set; }
        public DateTime JoinedAt { get; set; }

        public Player(long id, bool isMachinePlayer, string name, DateTime joinedAt)
        {
            Id = id;
            JoinedAt = joinedAt;
            Name = name;
            IsMachinePlayer = isMachinePlayer;
        }
    }
}