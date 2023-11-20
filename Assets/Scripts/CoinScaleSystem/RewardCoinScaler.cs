namespace CoinScaleSystem
{
    public static class RewardCoinScaler
    {
        public static double scaleFactor = 1;

        private static int dieCount = 0;

        public static void UpdateScale()
        {
            dieCount += 1;

            scaleFactor = scaleFactor * (1 + 0.1 * 1);
        }
    }
}