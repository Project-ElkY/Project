namespace TexasHoldem.AI
{
    using TexasHoldem.Logic.Players;
    using global::AI.Strategy;
    using System.Collections.Generic;
    using Helpers;
    using System;

    public class ElkYPlayer : BasePlayer
    {

        private IList<double> opponentStrenghtList;
        private IElkyPlayerStrategy strategy;

        public ElkYPlayer(IElkyPlayerStrategy strategy)
            : base()
        {
            this.strategy = strategy;
            this.opponentStrenghtList = new List<double>();
        }

        public ElkYPlayer()
            : this(new ElkyPlayerStrategy())
        {
        }

        public override string Name { get; } = "ElkYPlayer" + Guid.NewGuid();

        public override void StartGame(StartGameContext context)
        {
            base.StartGame(context);
        }

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


        public override void EndRound(EndRoundContext context)
        {
            base.EndRound(context);
        }

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

        public override void StartHand(StartHandContext context)
        {
            base.StartHand(context);
        }

        public override void StartRound(StartRoundContext context)
        {
            base.StartRound(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return this.strategy.MakeTurn(context, this.FirstCard, this.SecondCard, this.CommunityCards);
        }
    }
}
