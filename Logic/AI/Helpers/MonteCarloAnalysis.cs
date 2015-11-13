namespace TexasHoldem.AI.Helpers
{
    using HoldemHand;

    public static class MonteCarloAnalysis
    {
        private const int GameTrials = 500; // 1000 or 250

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
