using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ProceduralRPG.src.world.biomes
{
    internal class Biome
    {

        internal BiomeId? Id { get; private set; }

        internal Color Color { get; private set; }

        /// <returns>On a scale of 0 - 1, how close of a match chunk is</returns>
        internal Func<Chunk, float>? GetScore { get; private set; }

        internal float Treecover { get; private set; }
        internal float SoilFertility { get; private set; }

        internal Biome(BiomeId id, Color color, Func<Chunk, float> getScore, float treecover, float soilFertility)
        {
            Id = id;
            Color = color;
            GetScore = getScore;
            Treecover = treecover;
            SoilFertility = soilFertility;
        }

        /// <summary>
        /// Generates a biome based on the weighted average of the biomes
        /// </summary>
        /// <param name="biomes">Key is the biome, float is % weight</param>
        internal Biome(KeyValuePair<Biome, float>[] biomes)
        {
            // Generate a color based on the weighted average of the colors of the biomes
            Color[] colors = new Color[biomes.Length];
            for (int i = 0; i < biomes.Length; i++)
            {
                colors[i] = biomes[i].Key.Color * biomes[i].Value;
            }

            Color color = new();
            foreach (Color c in colors)
            {
                color.R += c.R;
                color.G += c.G;
                color.B += c.B;
                color.A += c.A;
            }

            Color = color;
        }

    }
}
