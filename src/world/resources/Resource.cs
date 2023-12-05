using System;

namespace ProceduralRPG.src.world.resources
{
    internal class Resource
    {

        internal ResourceId? Id { get; private set; }

        internal string Name { get; private set; }

        internal Func<World, float[,]> Generate { get; private set; }

        // Stats

        /// <summary>
        /// Determines whether this resource is edible or not
        /// </summary>
        internal float Nutrition { get; private set; }
        /// <summary>
        /// Determines how easily this resource can be shaped
        /// </summary>
        internal float Malleability { get; private set; }
        /// <summary>
        /// Determines how easily this resource can be broken
        /// </summary>
        internal float Hardness { get; private set; }


        internal Resource(ResourceId id, string name, Func<World, float[,]> generate, float nutrition = 0, float malleability = 0, float hardness = 0)
        {
            Id = id;
            Name = name;
            Generate = generate;

            // Stats
            Nutrition = nutrition;
            Malleability = malleability;
            Hardness = hardness;
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

        internal static float[,] GenerateMineral(World world, float multiplier = 1) => GenerateBasedOnStat(world, (chunk) => chunk.GetRockiness(), 0.2f, multiplier);
    }
}
