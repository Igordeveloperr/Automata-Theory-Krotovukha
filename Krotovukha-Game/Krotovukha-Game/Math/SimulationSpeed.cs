namespace Krotovukha_Game.Math
{
    internal static class SimulationSpeed
    {
        private const int MillisecondsPerSecond = 1000;
        private const int TimerOffsetMilliseconds = 15;

        public static int ConvertToMilliseconds(int speed)
        {
            return speed > 0
                ? (int)((1.0 / speed) * MillisecondsPerSecond) + TimerOffsetMilliseconds
                : 0;
        }
    }
}
