using System.Text;
using LanguageExt;


namespace RowCardGameEngine.Game.Models
{
    public class Card : Record<Card>
    {
        public readonly Suits Suit;
        public readonly Ranks Rank;

        public Card(Suits suit, Ranks rank)
        {
            this.Suit = suit;
            this.Rank = rank;
        }

        public static Option<int> operator -(Card card1, Option<Card> card2)
        {
            if (card1 == null)
            {
                return Option<int>.None;
            }

            return from c1 in Prelude.Some(card1)
                from c2 in card2
                where c1.Suit == c2.Suit
                select c1.Rank - c2.Rank;
        }

        public Option<string> AsPokerKey()
        {
            StringBuilder sb = new StringBuilder();

            var suitStr = Suit switch
            {
                Suits.Clubs => "C",
                Suits.Spades => "S",
                Suits.Hearts => "H",
                Suits.Diamonds => "D",
                _ => Option<string>.None
            };

            var rankStr = Rank switch
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