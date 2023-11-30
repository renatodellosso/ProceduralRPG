using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ProceduralRPG.src.world.biomes
{
    internal class BiomeList
    {
        private readonly Biome[] biomes = new Biome[]
        {
            new(BiomeId.Ocean, Color.Blue, (chunk) =>
            {
                return chunk.IsWater && chunk.plate!.IsWater ? 1 : 0;
            }),
            new(BiomeId.Grassland, Color.YellowGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return 1 - chunk.GetRockiness();
            })
        };

        private readonly Dictionary<BiomeId, Biome> biomeDict = new();

        private readonly static BiomeList instance = new();

        private BiomeList()
        {
            foreach (Biome biome in biomes)
                biomeDict.Add(biome.Id, biome);
        }

        internal static Biome Get(BiomeId id) => instance.biomeDict[id];

        internal static Biome[] GetAllBiomes() => instance.biomes;
    }
}
