namespace TexasHoldem.AI
{
    using TexasHoldem.Logic.Players;
    using global::AI.Strategy;
    using System.Collections.Generic;
    using Helpers;
    using System.Linq;
    using System;

    public class ElkYPlayer : BasePlayer
    {
        private const int MaxFoldLevel = 65;
        private const int MaxCallLevel = 77;
        private const int MaxRiseLevel = 87;

        private IList<double> opponentStrenghtList = new List<double>();
        private IElkyPlayerStrategy strategy;

        public ElkYPlayer(IElkyPlayerStrategy strategy)
            : base()
        {
            this.strategy = strategy;
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
            /*
            var opponentMoves = context.RoundActions.Where(x => x.PlayerName != this.Name);

            foreach (var oponentAction in opponentMoves)
            {
                this.opponentActions.Add((int)oponentAction.Action.Type);
            }
            */
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
                ReEvaluateGameStrategy();
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
                if ((this.strategy.Fold += 12) < MaxFoldLevel)
                {
                    this.strategy.Fold += 12;
                }

                if ((this.strategy.Call += 5) < MaxCallLevel)
                {
                    this.strategy.Call += 5;
                }

                if ((this.strategy.Raise += 2) < MaxRiseLevel)
                {
                    this.strategy.Raise += 2;
                }

                return;
            }

            if (GamesStatistics.PlayerLosses / GamesStatistics.TotalGames > 0.7)
            {
                if ((this.strategy.Fold += 10) < MaxFoldLevel)
                {
                    this.strategy.Fold += 10;
                }

                if ((this.strategy.Call += 4) < MaxCallLevel)
                {
                    this.strategy.Call += 4;
                }

                if ((this.strategy.Raise += 1) < MaxRiseLevel)
                {
                    this.strategy.Raise += 1;
                }

                return;
            }

            if (GamesStatistics.PlayerLosses / GamesStatistics.TotalGames > 0.6)
            {
                if ((this.strategy.Fold += 7) < MaxFoldLevel)
                {
                    this.strategy.Fold += 7;
                }

                if ((this.strategy.Call += 3) < MaxCallLevel)
                {
                    this.strategy.Call += 3;
                }

                return;
            }
        }
    }
}
