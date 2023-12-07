using System;

namespace ProceduralRPG.src.world.resources
{
    internal class Resource
    {

        internal struct Stat
        {
            internal float Min { get; private set; }
            internal float Max { get; private set; }

            internal float Avg => (Min + Max) / 2;

            internal Stat(float min, float max)
            {
                Min = min;
                Max = max;
            }

            /// <summary>
            /// Lerps between <see cref="Min"/> and <see cref="Max"/> based on quality
            /// </summary>
            internal float this[float quality]
            {
                get => Utils.Lerp(Min, Max, quality);
            }
        }

        internal ResourceId? Id { get; private set; }

        internal string Name { get; private set; }

        internal Func<World, float[,]> GenerateAmount { get; private set; }
        internal Func<World, float[,]> GenerateQuality { get; private set; }

        // Stats

        /// <summary>
        /// Determines whether this resource is edible or not
        /// </summary>
        internal Stat Nutrition { get; private set; }
        /// <summary>
        /// Determines how easily this resource can be broken, in Mohs
        /// </summary>
        internal Stat Hardness { get; private set; }

        internal Resource(ResourceId id, string name, Func<World, float[,]> generateAmt, Func<World, float[,]>? generateQuality = null, Stat? nutrition = null, Stat? hardness = null)
        {
            Id = id;
            Name = name;

            GenerateAmount = generateAmt;
            GenerateQuality = generateQuality ?? GenerateConstant;

            // Stats
            Nutrition = nutrition ?? new(0, 0);
            Hardness = hardness ?? new(0, 0);
        }

        internal static float[,] GenerateBasedOnStat(World world, Func<Chunk, float> getStat, float maxNoise = 0, float multiplier = 1)
        {
            float[,] values = new float[world.Settings.width, world.Settings.width];

            for (int x = 0; x < world.Settings.width; x++)
            {
                for (int y = 0; y < world.Settings.height; y++)
                {
                    values[x, y] = getStat(world[x, y]!) * multiplier;
                }
            }

            if (maxNoise > 0)
                Utils.AddRandomNoise(values, maxNoise * multiplier);

            return values;
        }

        /// <summary>
        /// Generates based on the rockiness of the chunk
        /// </summary>
        internal static float[,] GenerateMineral(World world, float multiplier) => GenerateBasedOnStat(world, (chunk) => chunk.GetRockiness(), 0.2f, multiplier);
        /// <summary>
        /// Generates based on the rockiness of the chunk
        /// </summary>
        internal static float[,] GenerateMineral(World world) => GenerateMineral(world, 1);

        internal static float[,] GenerateConstant(World world, float value)
        {
            float[,] values = new float[world.Settings.width, world.Settings.width];

            for (int x = 0; x < world.Settings.width; x++)
            {
                for (int y = 0; y < world.Settings.height; y++)
                {
                    values[x, y] = value;
                }
            }

            return values;
        }

        internal static float[,] GenerateConstant(World world) => GenerateConstant(world, 1);
    }
}
