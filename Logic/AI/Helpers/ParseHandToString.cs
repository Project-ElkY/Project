namespace TexasHoldem.AI.Helpers
{
    using TexasHoldem.Logic.Cards;

    /// <summary>
    /// Converts the card from the game engine to strings used for game analysis
    /// </summary>
    public static class ParseHandToString
    {
        /// <summary>
        /// Converts the card from the game engine to a string
        /// </summary>
        /// <param name="card">A card in a format from the game engine</param>
        /// <returns>The card as a string</returns>
        public static string GenerateStringFromCard(Card card)
        {
            string result = string.Empty;

            switch (card.Type)
            {
                case CardType.Two:
                    result += "2";
                    break;
                case CardType.Three:
                    result += "3";
                    break;
                case CardType.Four:
                    result += "4";
                    break;
                case CardType.Five:
                    result += "5";
                    break;
                case CardType.Six:
                    result += "6";
                    break;
                case CardType.Seven:
                    result += "7";
                    break;
                case CardType.Eight:
                    result += "8";
                    break;
                case CardType.Nine:
                    result += "9";
                    break;
                case CardType.Ten:
                    result += "10";
                    break;
                case CardType.Jack:
                    result += "J";
                    break;
                case CardType.Queen:
                    result += "Q";
                    break;
                case CardType.King:
                    result += "K";
                    break;
                case CardType.Ace:
                    result += "A";
                    break;
                default:
                    break;
            }

            switch (card.Suit)
            {
                case CardSuit.Club:
                    result += "c";
                    break;
                case CardSuit.Diamond:
                    result += "d";
                    break;
                case CardSuit.Heart:
                    result += "h";
                    break;
                case CardSuit.Spade:
                    result += "s";
                    break;
                default:
                    break;
            }           

            return result;
        }
    }
}
