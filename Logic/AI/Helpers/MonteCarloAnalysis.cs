namespace TexasHoldem.AI.Helpers
{
    using HoldemHand;

    /// <summary>
    /// Simulates the outcome of a given number of poker games
    /// </summary>
    public static class MonteCarloAnalysis
    {
        /// <summary>
        /// How many games will be simulated
        /// </summary>
        private const int GameTrials = 500; // 1000 or 250

        /// <summary>
        /// The method simulate the outcome of a poker game
        /// </summary>
        /// <param name="myCards">Those are the player cards</param>
        /// <param name="openCards">Those are the open cards on the table</param>
        /// <returns>The win ratio</returns>
        public static double CalculateWinChance(string myCards, string openCards)
        {
            ulong pocketmask = Hand.ParseHand(myCards);    
            ulong board = Hand.ParseHand(openCards);

            short wins = 0;
            short ties = 0;
            short count = 0;

            foreach (ulong boardmask in Hand.RandomHands(board, pocketmask, 5, GameTrials))
            {
                // Get a random opponent hand                    
                ulong oppmask = Hand.RandomHand(boardmask | pocketmask, 2);
                // Evaluate the player and opponent hands                    
                uint pocketHandVal = Hand.Evaluate(pocketmask | boardmask, 7);
                uint oppHandVal = Hand.Evaluate(oppmask | boardmask, 7);
                // Calculate Statistics                    
                if (pocketHandVal > oppHandVal)
                {
                    wins++;
                }
                else if (pocketHandVal == oppHandVal)
                {
                    ties++;
                }
                count++;
            }

            return (((double)wins) + ((double)ties) / 2.0) / ((double)count) * 100.0;
        } 
    }
}
