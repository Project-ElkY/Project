namespace TexasHoldem.AI
{
    using TexasHoldem.Logic.Players;
    using global::AI.Strategy;

    public class ElkYPlayer : BasePlayer
    {
        private IElkyPlayerStrategy strategy;

        public ElkYPlayer(IElkyPlayerStrategy strategy)
            : base()
        {
            this.strategy = strategy;
        }

        public ElkYPlayer()
            : this(new ElkyPlayerStrategy())
        { }

        public override string Name { get; } = "ElkYPlayer";

        public override void StartGame(StartGameContext context)
        {
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
            return strategy.MakeTurn(context, this.FirstCard, this.SecondCard, this.CommunityCards);
        }
    }
}
