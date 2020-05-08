using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace RowCardGameEngine.Game
{
    public class CircularPlayerList
    {
        private long currentPlayer;
        private readonly LinkedList<long> players;

        public CircularPlayerList(IReadOnlyCollection<long> players, long startingPlayer)
        {
            
            if (players == null || !players.Any())
            {
                throw new ArgumentException("players list is null or empty");
            }

            this.currentPlayer = startingPlayer;

            this.players = new LinkedList<long>(players);
        }

        public Option<long> GetNext()
        {
            var currentLinkedNode =
                players.Find(currentPlayer);

            if (currentLinkedNode == null)
            {
                return Option<long>.None;
            }

            var nextLinkedNode = currentLinkedNode.Next;

            if (nextLinkedNode == null)
            {
                nextLinkedNode = players.First;
            }

            currentPlayer = nextLinkedNode.Value;

            return nextLinkedNode.Value;
        }
    }
}