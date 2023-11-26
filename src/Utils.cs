using System;

namespace ProceduralRPG.src
{
    internal static class Utils
    {

        private static Random Random = new(Seed: DateTime.Now.Second);

        internal static double RandFloat(float max = 1, float min = 0)
        {
            return Random.NextSingle() * (max - min) + min;
        }


        internal static int RandInt(int max) => RandInt(0, max);

        internal static int RandInt(int min, int max)
        {
            return Random.Next(min, max);
        }
    }
}
