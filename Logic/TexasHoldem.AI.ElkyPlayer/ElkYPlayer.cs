namespace TexasHoldem.AI.ElkyPlayer
{
    using TexasHoldem.Logic.Players;
    using System.Collections.Generic;
    using Helpers;
    using System.Linq;
    using Strategy;
    using System;

    public class ElkYPlayer : BasePlayer
    {
        private IList<int> opponentActions = new List<int>();
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
                    }
                }
            }
            */
            base.EndHand(context);
        }


        public override void EndRound(EndRoundContext context)
        {
            var opponentMoves = context.RoundActions.Where(x => x.PlayerName != this.Name);

            foreach (var oponentAction in opponentMoves)
            {
                this.opponentActions.Add((int)oponentAction.Action.Type);
            }

            base.EndRound(context);
        }

        public override void EndGame(EndGameContext context)
        {
            /*
            if (context.WinnerName == this.Name)
            {
                GamesWon.PlayerWins++;
            }
            else
            {
                GamesWon.PlayerLosses++;
            }
            GamesWon.TotalGames++;*/
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
