namespace TexasHoldem.AI
{
    using System;
    using Helpers;
    using TexasHoldem.AI;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Players;

    public class ElkYPlayer : BasePlayer
    {
        public override string Name { get; } = "ElkYPlayer";

        public override void StartGame(StartGameContext context)
        {
            base.StartGame(context);
        }

        public override void EndRound(EndRoundContext context)
        {
            base.EndRound(context);

        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var playHand = InitialHandEvaluation.PreFlop(this.FirstCard, this.SecondCard);

                if (playHand < 45)
                {
                    return PlayerAction.Fold();
                }
                else if (playHand < 70)
                {
                    return PlayerAction.CheckOrCall();
                }
                else if (playHand < 80)
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
                else
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }
            }

            if (context.RoundType == GameRoundType.Flop || 
                context.RoundType == GameRoundType.River || 
                context.RoundType == GameRoundType.Turn)
            {
                var playerFirstHand = ParseHandToString.GenerateStringFromCard(this.FirstCard);
                var playerSecondHand = ParseHandToString.GenerateStringFromCard(this.SecondCard);

                // How to get the community hands

                return PlayerAction.CheckOrCall();
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
