using System;
using System.Linq;
using LanguageExt;


namespace RowCardGameEngine.Game.Models
{
    public class Hand
    {
        private readonly int numberOfCards;

        public Hand(int numberOfCards)
        {
            this.numberOfCards = numberOfCards;
        }
    }
}