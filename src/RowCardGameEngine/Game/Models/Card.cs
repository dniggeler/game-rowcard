using System.Text;
using LanguageExt;


namespace RowCardGameEngine.Game.Models
{
    public class Card : Record<Card>
    {
        private readonly Suits suit;
        private readonly Ranks rank;

        public Card(Suits suit, Ranks rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        public Option<string> AsPokerKey()
        {
            StringBuilder sb = new StringBuilder();

            var suitStr = suit switch
            {
                Suits.Clubs => "C",
                Suits.Spades => "S",
                Suits.Hearts => "H",
                Suits.Diamonds => "D",
                _ => Option<string>.None
            };

            var rankStr = rank switch
            {
                Ranks.Ace => "A",
                Ranks.King => "K",
                Ranks.Queen => "Q",
                Ranks.Jack => "J",
                Ranks.Ten => "T",
                Ranks.Nine => "9",
                Ranks.Eight => "8",
                Ranks.Seven => "7",
                Ranks.Six => "6",
                _ => Option<string>.None
            };

            return 
                from s in suitStr
                from r in rankStr
                select sb.Append(s)
                    .Append(r)
                    .ToString();

        }

    }
}