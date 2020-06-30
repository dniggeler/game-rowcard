using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace RowCardGameEngine.Game
{
    public class CircularPlayerList
    {
        private readonly LinkedList<long> players;

        public long CurrentPlayer { get; set; }

        public CircularPlayerList(IReadOnlyCollection<long> players, long startingPlayer)
        {
            
            if (players == null || !players.Any())
            {
                throw new ArgumentException("players list is null or empty");
            }

            CurrentPlayer = startingPlayer;

            this.players = new LinkedList<long>(players);
        }

        public Option<long> Next()
        {
            var currentLinkedNode =
                players.Find(CurrentPlayer);

            if (currentLinkedNode == null)
            {
                return Option<long>.None;
            }

            var nextLinkedNode = currentLinkedNode.Next;

            if (nextLinkedNode == null)
            {
                nextLinkedNode = players.First;
            }

            CurrentPlayer = nextLinkedNode.Value;

            return nextLinkedNode.Value;
        }

        public Option<long> GetCurrent()
        {
            var currentLinkedNode =
                players.Find(CurrentPlayer);

            if (currentLinkedNode == null)
            {
                return Option<long>.None;
            }

            return CurrentPlayer;
        }
    }
}