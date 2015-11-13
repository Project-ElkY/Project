namespace TexasHoldem.AI
{
    using System;
    using Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class SmartPlayer : BasePlayer
    {
        public override string Name { get; } = "SmartPlayer_" + Guid.NewGuid();


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
                /*
                // some parcing
                var wincChane = biblioteka(,,,,,,);

                DicideWhsatToDo(wincChane, context);

                List<int> pastGames = win/loose ration без фолднати игри

                // енумеражия с поведението
                // енумерация със стратегията

                // при какви проценти какво поведение да имаме по default
                // разлини предифинирани повдение за по 3-5 игри, за да видим как реагира опонента


                // в зависимост от от това какво в статистиката
                // как променяме default проценти

                // ако фолдва чакаме само силни ръце 


                    колко е фолдвал
                    колко е call-ал
                    колко е rise
                    kolko е бил All in

                // имаме общо 10 000 ръце 

                // съотношение между сумата за спечелеване и шансовети за нея 
                // като отчита натрупаните до момента суми

                // дали да смятаме ако фолдваме до края няма да спечелим играта

                // class DicideWhsatToDo


                // речник с информция какво поведение до какви резултати ще води 

                // Как да си взем информация за действията от последната игра
            var t = context.RoundActions;

            foreach (var item in t)
            {
                item.Action.Type;
            }

    */
                var playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);
                if (playHand == CardValuationType.Unplayable)
                {
                    if (context.CanCheck)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationType.Risky)
                {
                    var smallBlindsTimes = RandomProvider.Next(1, 8);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationType.Recommended)
                {
                    var smallBlindsTimes = RandomProvider.Next(6, 14);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.CheckOrCall();
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
