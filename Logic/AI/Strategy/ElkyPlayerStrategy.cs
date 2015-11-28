namespace AI.Strategy
{
    using TexasHoldem.AI.Helpers;
    using TexasHoldem.Logic.Players;
    using TexasHoldem.Logic.Cards;
    using System.Collections.Generic;
    using TexasHoldem.Logic;

    public class ElkyPlayerStrategy : IElkyPlayerStrategy
    {
        public ElkyPlayerStrategy(int fold, int call, int raise, int allIn)
        {
            this.Fold = fold;
            this.Call = call;
            this.Raise = raise;
            this.AllIn = allIn;
        }

        public ElkyPlayerStrategy()
            : this(45, 70, 80, 90)
        { }

        public int Fold { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public int AllIn { get; set; }

        public PlayerAction MakeTurn(GetTurnContext context, Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                PlayerAction act = PreFlopAction(context, firstCard, secondCard);
                return act;
            }

            if (context.RoundType == GameRoundType.Flop ||
                context.RoundType == GameRoundType.River ||
                context.RoundType == GameRoundType.Turn)
            {
                PlayerAction act = FlopAction(context, firstCard, secondCard, communityCards);
                return act;
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction FlopAction(GetTurnContext context, Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            var playerFirstHand = ParseHandToString.GenerateStringFromCard(firstCard);
            var playerSecondHand = ParseHandToString.GenerateStringFromCard(secondCard);

            string playerHand = playerFirstHand + " " + playerSecondHand;
            string openCards = string.Empty;

            foreach (var item in communityCards)
            {
                openCards += ParseHandToString.GenerateStringFromCard(item) + " ";
            }

            var chance = MonteCarloAnalysis.CalculateWinChance(playerHand, openCards.Trim());

            if (chance < this.Fold)
            {
                return PlayerAction.Fold();
            }
            else if (chance < this.Call)
            {
                return PlayerAction.CheckOrCall();
            }
            else if (chance < this.Raise)
            {
                if (context.SmallBlind * 2 < context.MoneyLeft)
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
                else
                {
                    int putMoney = context.MoneyLeft;

                    if (putMoney != 0)
                    {
                        return PlayerAction.Raise(putMoney);
                    }

                    return PlayerAction.CheckOrCall();
                }
            }
            else if (chance <= this.AllIn)
            {
                int putMoney = context.MoneyLeft;
                var pot = context.CurrentPot;
                if (putMoney != 0)
                {
                    if (putMoney > pot && pot > 0)
                    {
                        return PlayerAction.Raise(pot);
                    }
                    else
                    {
                        return PlayerAction.Raise(putMoney);
                    }
                }
                return PlayerAction.CheckOrCall();
            }
            else
            {
                int putMoney = context.MoneyLeft;

                if (putMoney != 0)
                {
                    return PlayerAction.Raise(putMoney);
                }

                return PlayerAction.CheckOrCall();
            }
        }

        private PlayerAction PreFlopAction(GetTurnContext context, Card firstCard, Card secondCard)
        {

            var playHand = InitialHandEvaluation.PreFlop(firstCard, secondCard);

            if (playHand < this.Fold)
            {
                return PlayerAction.Fold();
            }
            else if (playHand < this.Call)
            {
                return PlayerAction.CheckOrCall();
            }
            else if (playHand < this.Raise)
            {
                if (context.SmallBlind * 2 < context.MoneyLeft)
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
                else
                {
                    int putMoney = context.MoneyLeft;

                    if (putMoney != 0)
                    {
                        return PlayerAction.Raise(putMoney);
                    }

                    return PlayerAction.CheckOrCall();
                }
            }
            else
            {
                int putMoney = context.MoneyLeft;

                if (putMoney != 0)
                {
                    return PlayerAction.Raise(putMoney);
                }

                return PlayerAction.CheckOrCall();
            }
        }
    }
}
