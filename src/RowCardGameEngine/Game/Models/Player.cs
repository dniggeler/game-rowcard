using System;
using System.Runtime.Serialization;
using LanguageExt;

namespace RowCardGameEngine.Game.Models
{
    public class Player
    {
        public long Id { get; set; }
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