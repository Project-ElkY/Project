namespace AI.Strategy
{
    using System;
    using TexasHoldem.AI.Helpers;
    using TexasHoldem.Logic.Players;
    using TexasHoldem.Logic.Cards;
    using System.Collections.Generic;
    using TexasHoldem.Logic;
    using TexasHoldem.AI;

    /// <summary>
    /// The class will influence the player behaivor
    /// </summary>
    public class ElkyPlayerStrategy : IElkyPlayerStrategy
    {
        /// <summary>
        /// It is used only in the PreFlop part of the game, and sets a higher level than usual.
        /// </summary>
        private const int PreFlopFoldLevel = 41;

        /// <summary>
        /// A fully customizable constructor for the game strategy.
        /// </summary>
        /// <param name="fold">The maximum hand strength below which the player will fold.</param>
        /// <param name="call">The maximum hand strength below which the player will Call.</param>
        /// <param name="raise">The maximum hand strength below which the player will Rise.</param>
        /// <param name="allIn">The maximum hand strength below which the player will play All-In.</param>
        public ElkyPlayerStrategy(int fold, int call, int raise, int allIn)
        {
            this.Fold = fold;
            this.Call = call;
            this.Raise = raise;
            this.AllIn = allIn;
        }

        /// <summary>
        /// Basic empty constructor
        /// </summary>
        public ElkyPlayerStrategy()
            : this(33, 70, 80, 95)
        { }

        /// <summary>
        /// Sets the level below which the player will Fold.
        /// </summary>
        public int Fold { get; set; }

        /// <summary>
        /// Sets the level below which the player will Call.
        /// </summary>
        public int Call { get; set; }

        /// <summary>
        /// Sets the level below which the player will Raise.
        /// </summary>
        public int Raise { get; set; }

        /// <summary>
        /// Sets the level below which the player will play All-In.
        /// </summary>
        public int AllIn { get; set; }

        /// <summary>
        /// Decides what the player action will be.
        /// </summary>
        /// <param name="context">The given game context.</param>
        /// <param name="firstCard">The player first card.</param>
        /// <param name="secondCard">The player second card.</param>
        /// <param name="communityCards">All open cards on the table.</param>
        /// <returns>An PlayerAction instance.</returns>
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

        /// <summary>
        /// Sets a new behavior for the player depending the current loss ratio.
        /// </summary>
        public void ReEvaluateGameStrategy()
        {
            if (GamesStatistics.Instance().PlayerLosses / GamesStatistics.Instance().TotalGames > 0.75)
            {
                this.Fold = 0;
                this.Call = 30;
                this.Raise = 45;
                this.AllIn = 60;
            }

            if (GamesStatistics.Instance().PlayerLosses / GamesStatistics.Instance().TotalGames > 0.65)
            {
                this.Fold = 25;
                this.Call = 55;
                this.Raise = 70;
                this.AllIn = 85;
            }
        }

        /// <summary>
        /// Decides what action to take when there are any open cards on the table. 
        /// </summary>
        /// <param name="context">The given game context.</param>
        /// <param name="firstCard">The player first card.</param>
        /// <param name="secondCard">The player second card.</param>
        /// <param name="communityCards">All open cards on the table.</param>
        /// <returns>An PlayerAction instance.</returns>
        private PlayerAction FlopAction(GetTurnContext context, Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            var moneyToCall = context.MoneyToCall;
            var potMoney = context.CurrentPot;
            var potAndCall = moneyToCall + potMoney;
            var chanceToFold = moneyToCall * 100 / (double)potAndCall;
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
                var isAllIn = context.IsAllIn;
                int moneyToFold = Math.Min(context.SmallBlind * 2, context.MoneyLeft);
                if (isAllIn)
                {
                    if (moneyToCall >= (context.MoneyLeft / 3))
                    {
                        return PlayerAction.Fold();
                    }
                    return PlayerAction.CheckOrCall();
                }
                if (moneyToCall > moneyToFold)
                {
                    return PlayerAction.Fold();
                }

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

        /// <summary>
        /// Decides what action to take during the initial game stage. 
        /// </summary>
        /// <param name="context">The given game context.</param>
        /// <param name="firstCard">The player first card.</param>
        /// <param name="secondCard">The player second card.</param>
        /// <returns>An PlayerAction instance.</returns>
        private PlayerAction PreFlopAction(GetTurnContext context, Card firstCard, Card secondCard)
        {

            var playHand = InitialHandEvaluation.PreFlop(firstCard, secondCard);

            if (playHand <= PreFlopFoldLevel)
            {
                // Perform a Bluff
                if (GamesStatistics.Instance().TotalGames % 3 == 0)
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
                }

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
