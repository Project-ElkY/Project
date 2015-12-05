namespace TexasHoldem.AI.ElkyPlayer.Strategy
{
    using System.Collections.Generic;
    using TexasHoldem.Logic.Players;
    using TexasHoldem.Logic.Cards;

    public interface IElkyPlayerStrategy
    {
        int Fold { get; set; }

        int Call { get; set; }

        int Raise { get; set; }

        PlayerAction MakeTurn(GetTurnContext context, Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards);
    }
}
