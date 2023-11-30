using System;

namespace ProceduralRPG.src
{
    internal static class Utils
    {

        private static Random Random = new(Seed: DateTime.Now.Second);

        internal static float RandFloat(float min = 0, float max = 1)
        {
            return Random.NextSingle() * (max - min) + min;
        }

        internal static float RandFloat(float max) => RandFloat(0, max);


        internal static int RandInt(int max) => RandInt(0, max);

        internal static int RandInt(int min, int max)
        {
            return Random.Next(min, max + 1);
        }

        internal static void SeedRand(int seed)
        {
            Random = new(seed);
        }

        /// <summary>
        /// Averages the values of the surrounding cells
        /// </summary>
        internal static float[,] Smooth(float[,] map, int times)
        {
            for (int i = 0; i < times; i++)
            {
                float[,] newMap = new float[map.GetLength(0), map.GetLength(1)];

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {

                        float sum = 0;
                        int count = 0;

                        for (int xx = -1; xx <= 1; xx++)
                        {
                            for (int yy = -1; yy <= 1; yy++)
                            {
                                if (x + xx < 0 || x + xx >= map.GetLength(0) || y + yy < 0 || y + yy >= map.GetLength(1))
                                    continue;

                                sum += map[x + xx, y + yy];
                                count++;
                            }
                        }

                        newMap[x, y] = sum / count;
                    }
                }

                map = newMap;
            }

            return map;
        }
    }
}
