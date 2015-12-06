namespace TexasHoldem.AI
{
    public class GamesStatistics
    {
        private static GamesStatistics instance;

        private GamesStatistics()
        {
            this.PlayerWins = 0;
            this.PlayerLosses = 0;
            this.TotalGames = 0;
        }

        public double PlayerWins { get; set; }

        public double PlayerLosses { get; set; }

        public double TotalGames { get; set; }

        public static GamesStatistics Instance()
        {
            if (instance == null)
            {
                instance = new GamesStatistics();
            }

            return instance;
        }
    }
}