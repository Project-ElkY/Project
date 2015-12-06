namespace AI.Strategy
{
    using TexasHoldem.AI.Helpers;
    using TexasHoldem.Logic.Players;
    using TexasHoldem.Logic.Cards;
    using System.Collections.Generic;
    using TexasHoldem.Logic;
    using TexasHoldem.AI;

    public class ElkyPlayerStrategy : IElkyPlayerStrategy
    {
        private const int PreFlopFoldLevel = 41;
        private const int MaxFoldLevel = 45;
        private const int MaxCallLevel = 75;
        private const int MaxRiseLevel = 82;

        public ElkyPlayerStrategy(int fold, int call, int raise, int allIn)
        {
            this.Fold = fold;
            this.Call = call;
            this.Raise = raise;
            this.AllIn = allIn;
        }

        public ElkyPlayerStrategy()
            : this(33, 70, 80, 95)
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

        public void ReEvaluateGameStrategy()
        {
            if (GamesStatistics.PlayerLosses / GamesStatistics.TotalGames > 0.75)
            {
                this.Fold = 0;
                this.Call = 30;
                this.Raise = 45;
                this.AllIn = 60;
            }

            if (GamesStatistics.PlayerLosses / GamesStatistics.TotalGames > 0.65)
            {
                this.Fold = 25;
                this.Call = 55;
                this.Raise = 70;
                this.AllIn = 85;
            }
        }

        private PlayerAction FlopAction(GetTurnContext context, Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            var moneyToCall = context.MoneyToCall;
            var potMoney = context.CurrentPot;
            var potAndCall = moneyToCall + potMoney;
            var chanceToFold = moneyToCall / potAndCall;
            var playerFirstHand = ParseHandToString.GenerateStringFromCard(firstCard);
            var playerSecondHand = ParseHandToString.GenerateStringFromCard(secondCard);

            string playerHand = playerFirstHand + " " + playerSecondHand;
            string openCards = string.Empty;

            foreach (var item in communityCards)
            {
                openCards += ParseHandToString.GenerateStringFromCard(item) + " ";
            }

            var chance = MonteCarloAnalysis.CalculateWinChance(playerHand, openCards.Trim());
            if (chance < chanceToFold || chance < this.Fold)
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

            if (playHand <= PreFlopFoldLevel)
            {
                return PlayerAction.Fold();
            }
            else if (playHand <= this.Call)
            {
                return PlayerAction.CheckOrCall();
            }
            else if (playHand <= this.Raise)
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
            else if (playHand <= this.AllIn)
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
    }
}
