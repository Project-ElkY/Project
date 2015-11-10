namespace AI
{
    using System;
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
            throw new NotImplementedException();
        }
    }
}
