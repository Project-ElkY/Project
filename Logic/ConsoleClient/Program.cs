namespace TexasHoldem.UI.Console
{
    using System;
    using AI;
    using TexasHoldem.Logic.GameMechanics;

    public static class Program
    {
        private const string ProgramName = "TexasHoldem.UI.Console (c) 2015";
        private const int GameHeight = 50;
        private const int GameWidth = 50;

        public static void Main()
        {
            //Console.BackgroundColor = ConsoleColor.Black;
            //Console.ForegroundColor = ConsoleColor.Gray;
            //Console.BufferHeight = Console.WindowHeight = GameHeight;
            //Console.BufferWidth = Console.WindowWidth = GameWidth;

            //ConsoleHelper.WriteOnConsole(GameHeight - 1, GameWidth - ProgramName.Length - 1, ProgramName, ConsoleColor.Green);

            //var consolePlayer1 = new ConsoleUiDecorator(new ElkYPlayer(), 0, GameWidth, 5);
            //var consolePlayer2 = new ConsoleUiDecorator(new SmartPlayer(), 6, GameWidth, 5);

            var player1 = new ElkYPlayer();
            var player2 = new ElkYPlayer();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 100; i++)
                {
                    ITexasHoldemGame game = new TwoPlayersTexasHoldemGame(player1, player2);
                    game.Start();
                }

                var playerWins = GamesStatistics.Instance().PlayerWins;
                var allgames = GamesStatistics.Instance().TotalGames;

                Console.WriteLine("Games won: {0}", playerWins);

                GamesStatistics.Instance().PlayerWins = 0;
                GamesStatistics.Instance().PlayerLosses = 0;
                GamesStatistics.Instance().TotalGames = 0;
            }
        }
    }
}
