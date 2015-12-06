namespace TexasHoldem.AI
{
    using TexasHoldem.Logic.Players;
    using global::AI.Strategy;
    using System.Collections.Generic;
    using Helpers;
    using System;

    public class ElkYPlayer : BasePlayer
    {
        private const int MaxFoldLevel = 49;
        private const int MaxCallLevel = 68;
        private const int MaxRiseLevel = 82;

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
                GamesStatistics.PlayerWins++;
            }
            else
            {
                GamesStatistics.PlayerLosses++;
            }

            GamesStatistics.TotalGames++;

            if (GamesStatistics.TotalGames % 200 == 0)
            {
                this.ReEvaluateGameStrategy();
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

        private void ReEvaluateGameStrategy()
        {
            if (GamesStatistics.PlayerLosses / GamesStatistics.TotalGames > 0.8)
            {
                if ((this.strategy.Fold += 3) < MaxFoldLevel)
                {
                    this.strategy.Fold += 3;
                }

                if ((this.strategy.Call += 2) < MaxCallLevel)
                {
                    this.strategy.Call += 2;
                }

                if ((this.strategy.Raise += 1) < MaxRiseLevel)
                {
                    this.strategy.Raise += 1;
                }

                return;
            }

            if (GamesStatistics.PlayerLosses / GamesStatistics.TotalGames > 0.7)
            {
                if ((this.strategy.Fold += 2) < MaxFoldLevel)
                {
                    this.strategy.Fold += 2;
                }

                if ((this.strategy.Call += 1) < MaxCallLevel)
                {
                    this.strategy.Call += 1;
                }

                return;
            }

            if (GamesStatistics.PlayerLosses / GamesStatistics.TotalGames > 0.6)
            {
                if ((this.strategy.Fold += 1) < MaxFoldLevel)
                {
                    this.strategy.Fold += 1;
                }

                return;
            }
        }
    }
}
