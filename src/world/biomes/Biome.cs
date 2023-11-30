using Microsoft.Xna.Framework;
using System;

namespace ProceduralRPG.src.world.biomes
{
    internal class Biome
    {

        internal BiomeId Id { get; private set; }

        internal Color Color { get; private set; }

        /// <returns>On a scale of 0 - 1, how close of a match chunk is</returns>
        internal Func<Chunk, float> GetScore { get; private set; }

        internal Biome(BiomeId id, Color color, Func<Chunk, float> getScore)
        {
            Id = id;
            Color = color;
            GetScore = getScore;
        }

    }
}
