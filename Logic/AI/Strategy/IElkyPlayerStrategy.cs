namespace AI.Strategy
{
    using System.Collections.Generic;
    using TexasHoldem.Logic.Players;
    using TexasHoldem.Logic.Cards;

    /// <summary>
    /// Determinates the basic functionality of the player strategy.
    /// </summary>
    public interface IElkyPlayerStrategy
    {
        /// <summary>
        /// Sets the level below which the player will Fold.
        /// </summary>
        int Fold { get; set; }

        /// <summary>
        /// Sets the level below which the player will Call.
        /// </summary>
        int Call { get; set; }

        /// <summary>
        /// Sets the level below which the player will Raise.
        /// </summary>
        int Raise { get; set; }

        /// <summary>
        /// Decides what the player action will be.
        /// </summary>
        /// <param name="context">The given game context.</param>
        /// <param name="firstCard">The player first card.</param>
        /// <param name="secondCard">The player second card.</param>
        /// <param name="communityCards">All open cards on the table.</param>
        /// <returns>An PlayerAction instance.</returns>
        PlayerAction MakeTurn(GetTurnContext context, Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards);

        /// <summary>
        /// Method used for changing the game strategy.
        /// </summary>
        void ReEvaluateGameStrategy();
    }
}
