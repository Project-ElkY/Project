namespace TexasHoldem.AI
{
    using TexasHoldem.Logic.Players;
    using global::AI.Strategy;
    using System.Collections.Generic;
    using Helpers;
    using System;

    /// <summary>
    /// An instance of an AI for a C# Texas Hold'em game.
    /// </summary>
    public class ElkYPlayer : BasePlayer
    {
        /// <summary>
        /// May be used for gathering information about the opposition player.
        /// </summary>
        private IList<double> opponentStrenghtList;

        /// <summary>
        /// Holds the current player startegy.
        /// </summary>
        private IElkyPlayerStrategy strategy;

        /// <summary>
        /// Constructor with the class dependencies.
        /// </summary>
        /// <param name="strategy">The player startegy for the game.</param>
        public ElkYPlayer(IElkyPlayerStrategy strategy)
            : base()
        {
            this.strategy = strategy;
            this.opponentStrenghtList = new List<double>();
        }

        /// <summary>
        /// An empty constructor to be used from the game engine.
        /// </summary>
        public ElkYPlayer()
            : this(new ElkyPlayerStrategy())
        {
        }

        /// <summary>
        /// The player Name.
        /// </summary>
        public override string Name { get; } = "ElkYPlayer" + Guid.NewGuid();

        /// <summary>
        /// Method used at the start of the game.
        /// </summary>
        /// <param name="context">The current context of the game.</param>
        public override void StartGame(StartGameContext context)
        {
            base.StartGame(context);
        }

        /// <summary>
        /// Method that gathers information about the opposition player hand strength.
        /// </summary>
        /// <param name="context">The current context of the game.</param>
        public override void EndHand(EndHandContext context)
        {
            /*
            var openCards = context.ShowdownCards;
            if (openCards.Count != 0)
            {
                foreach (var item in openCards)
                {

                    if (item.Key != this.Name)
                    {
                        List<TexasHoldem.Logic.Cards.Card> cards = new List<TexasHoldem.Logic.Cards.Card>();
                        foreach (var card in item.Value)
                        {
                            var cardToAdd = new TexasHoldem.Logic.Cards.Card(card.Suit, card.Type);
                            cards.Add(card);
                        }

                        var opponentSrenght = InitialHandEvaluation.PreFlop(cards[0], cards[1]);
                        this.opponentStrenghtList.Add(opponentSrenght);
                    }
                }
            }
            */
            base.EndHand(context);
        }

        /// <summary>
        /// Method used at the end of each round.
        /// </summary>
        /// <param name="context">The current context of the game.</param>
        public override void EndRound(EndRoundContext context)
        {
            base.EndRound(context);
        }

        /// <summary>
        /// The method gathers statistical information about the given AI vs AI battle.
        /// </summary>
        /// <param name="context">The current context of the game.</param>
        public override void EndGame(EndGameContext context)
        {
            if (context.WinnerName == this.Name)
            {
                GamesStatistics.Instance().PlayerWins++;
            }
            else
            {
                GamesStatistics.Instance().PlayerLosses++;
            }

            GamesStatistics.Instance().TotalGames++;

            if (GamesStatistics.Instance().TotalGames % 70 == 0)
            {
                this.strategy.ReEvaluateGameStrategy();
            }

            base.EndGame(context);
        }

        /// <summary>
        /// Method used at the start of the game.
        /// </summary>
        /// <param name="context">The current context of the game.</param>
        public override void StartHand(StartHandContext context)
        {
            base.StartHand(context);
        }

        /// <summary>
        /// Method used at the start of each round.
        /// </summary>
        /// <param name="context">The current context of the game.</param>
        public override void StartRound(StartRoundContext context)
        {
            base.StartRound(context);
        }

        /// <summary>
        /// The player strategy will determine what type of action to be taken.
        /// </summary>
        /// <param name="context">The current context of the game.</param>
        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return this.strategy.MakeTurn(context, this.FirstCard, this.SecondCard, this.CommunityCards);
        }
    }
}
