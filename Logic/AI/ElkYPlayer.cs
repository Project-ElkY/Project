namespace TexasHoldem.AI
{
    using TexasHoldem.Logic.Players;
    using global::AI.Strategy;
    using System.Collections.Generic;
    using Helpers;
    using System.Linq;

    public class ElkYPlayer : BasePlayer
    {
        private IList<PlayerActionAndName> opponentActions;
        private IElkyPlayerStrategy strategy;

        public ElkYPlayer(IElkyPlayerStrategy strategy)
            : base()
        {
            this.strategy = strategy;
            opponentActions = new List<PlayerActionAndName>();
        }

        public ElkYPlayer()
            : this(new ElkyPlayerStrategy())
        { }

        public override string Name { get; } = "ElkYPlayer";

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
                    }
                }
            }

            base.EndHand(context);
        }


        public override void EndRound(EndRoundContext context)
        {
            var opponentMoves = context.RoundActions.Where(x => x.PlayerName != this.Name);

            foreach (var oponentAction in opponentMoves)
            {
                this.opponentActions.Add(oponentAction);
            }

            base.EndRound(context);
        }

        public override void EndGame(EndGameContext context)
        {
            if (context.WinnerName == this.Name)
            {
                GamesWon.PlayerWins++;
            }
            else
            {
                GamesWon.PlayerLosses++;
            }
            GamesWon.TotalGames++;
            base.EndGame(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return strategy.MakeTurn(context, this.FirstCard, this.SecondCard, this.CommunityCards);
        }
    }
}
