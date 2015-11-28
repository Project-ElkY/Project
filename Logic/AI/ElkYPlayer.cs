namespace TexasHoldem.AI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using TexasHoldem.AI;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Players;

    public class ElkYPlayer : BasePlayer
    {
        public override string Name { get; } = "ElkYPlayer";
        
        public override void StartGame(StartGameContext context)
        {
            InitMyGameStartegy();
            base.StartGame(context);
        }
        
        public override void EndRound(EndRoundContext context)
        { 
            base.EndRound(context);
        }

        public override void EndGame(EndGameContext context)
        {
            base.EndGame(context);
        }
        
        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                PlayerAction act = PreFlopAction(context);
                return act;
            }

            if (context.RoundType == GameRoundType.Flop || 
                context.RoundType == GameRoundType.River || 
                context.RoundType == GameRoundType.Turn)
            {
                PlayerAction act = FlopAction(context);
                return act;                
            }

            return PlayerAction.CheckOrCall();
        }

        private void InitMyGameStartegy()
        {
            MyGameStartegy.Fold = 45;
            MyGameStartegy.Call = 70;
            MyGameStartegy.Rise = 80;
        }

        private PlayerAction FlopAction(GetTurnContext context)
        {
            var playerFirstHand = ParseHandToString.GenerateStringFromCard(this.FirstCard);
            var playerSecondHand = ParseHandToString.GenerateStringFromCard(this.SecondCard);

            string playerHand = playerFirstHand + " " + playerSecondHand;
            string openCards = string.Empty;

            foreach (var item in this.CommunityCards)
            {
                openCards += ParseHandToString.GenerateStringFromCard(item) + " ";
            }

            var chance = MonteCarloAnalysis.CalculateWinChance(playerHand, openCards.Trim());

            if (chance < MyGameStartegy.Fold)
            {
                return PlayerAction.Fold();
            }
            else if (chance < MyGameStartegy.Call)
            {
                return PlayerAction.CheckOrCall();
            }
            else if (chance < MyGameStartegy.Rise)
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

        private PlayerAction PreFlopAction(GetTurnContext context)
        {

            var playHand = InitialHandEvaluation.PreFlop(this.FirstCard, this.SecondCard);

            if (playHand < MyGameStartegy.Fold)
            {
                return PlayerAction.Fold();
            }
            else if (playHand < MyGameStartegy.Call)
            {
                return PlayerAction.CheckOrCall();
            }
            else if (playHand < MyGameStartegy.Rise)
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
