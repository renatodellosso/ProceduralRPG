using Microsoft.Xna.Framework;
using ProceduralRPG.src.world.biomes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProceduralRPG.src.world
{
    internal class Chunk
    {

        internal World World { get; private set; }

        internal Vector2 Pos { get; private set; }

        internal TectonicPlate? plate;

        /// <summary>
        /// In meters
        /// </summary>
        internal int elevation;

        /// <summary>
        /// In degrees Fahrenheit
        /// </summary>
        internal int Temperature
        {
            get
            {
                int temp = World.Settings.baseTemperature;

                if (elevation > World.Settings.elevation.defaultElevation * 0.8f)
                    temp -= (int)(0.015f * Math.Abs(elevation - World.Settings.elevation.defaultElevation));
                else
                    temp -= (int)(0.001f * Math.Abs(elevation - World.Settings.elevation.defaultElevation));
                temp -= (int)(120 * Math.Abs(Pos.Y - World.Settings.height / 2) / (World.Settings.height / 2));

                return temp;
            }
        }

        internal float baseRainfallMult;
        /// <summary>
        /// In ml
        /// </summary>
        internal int Rainfall
        {
            get
            {
                float rainfall = baseRainfallMult * World.Settings.baseRainfall;

                rainfall *= 1.2f - (elevation - World.Settings.elevation.defaultElevation + 650) / 6500f * 0.7f;
                //rainfall *= 1.4f - Math.Abs(Temperature - 55) / 65f;

                return Math.Max((int)rainfall, 0);
            }
        }

        /// <summary>
        /// A biome generated from the weighted average of the biomes in the chunk
        /// </summary>
        internal Biome? Biome { get; private set; }

        internal bool IsWater => elevation < World.Settings.elevation.baseSeaLevel;
        internal bool IsMountain => elevation > World.Settings.elevation.mountainLevel;

        internal Chunk(World world, Vector2 pos)
        {
            World = world;
            Pos = pos;

            elevation = world.Settings.elevation.defaultElevation;
        }

        internal Chunk[] GetAdjacent(int distance = 1)
        {
            List<Chunk?> chunks = new();

            for (int x = (int)Pos.X - distance; x <= Pos.X + distance; x++)
            {
                for (int y = (int)Pos.Y - distance; y <= Pos.Y + distance; y++)
                {
                    if (x == Pos.X && y == Pos.Y)
                        continue;

                    chunks.Add(World[x, y]);
                }
            }

            return chunks.Where(c => c != null).ToArray()!;
        }

        /// <returns>What percent of tiles within distance 2 are water when their plate is not water</returns>
        internal float GetMarshiness()
        {
            Chunk[] adjacent = GetAdjacent(2);
            int waterCount = 0;

            foreach (Chunk chunk in adjacent)
            {
                if (chunk!.IsWater && !chunk.plate!.IsWater)
                    waterCount++;
            }

            return waterCount / (float)adjacent.Length;
        }

        /// <returns>Average difference in elevation between this chunk and adjacent chunks</returns>
        internal int GetRockiness()
        {
            Chunk[] adjacent = GetAdjacent(2);
            int rockiness = 0;

            foreach (Chunk chunk in adjacent)
            {
                rockiness += Math.Abs(chunk!.elevation - elevation);
            }

            return rockiness / adjacent.Length;
        }

        internal void CalculateBiomes()
        {
            try
            {
                List<KeyValuePair<BiomeId, float>> biomeScores = new();

                foreach (Biome biome in BiomeList.GetAllBiomes())
                {
                    biomeScores.Add(new(biome.Id!.Value, biome.GetScore!.Invoke(this)));
                }

                biomeScores = biomeScores.OrderByDescending(i => i.Value).ToList();

                List<KeyValuePair<BiomeId, float>> biomeIds = new();

                // Convert scores to percentages of the total score
                float totalWeight = 0;
                for (int i = 0; i < biomeScores.Count && i < World.Settings.maxBiomesPerChunk; i++)
                {
                    KeyValuePair<BiomeId, float> biomeScore = biomeScores[i];

                    if (biomeScore.Value <= 0)
                        break;

                    biomeIds.Add(new(biomeScore.Key, biomeScore.Value));
                    totalWeight += biomeScore.Value;
                }

                for (int i = 0; i < biomeIds.Count; i++)
                {
                    biomeIds[i] = new(biomeIds[i].Key, biomeIds[i].Value / totalWeight);
                }

                if (biomeIds.Count == 0)
                {
                    Debug.WriteLine($"No biomes at {Pos.X}, {Pos.Y}: \n\tTemp: {Temperature}, Rainfall: {Rainfall}, Elevation: {elevation}, " +
                                $"Rockiness: {GetRockiness()}, Marshiness: {GetMarshiness()}, IsWater: {IsWater}, Plate.IsWater: {plate!.IsWater}");
                    return;
                }

                // Generate biome
                KeyValuePair<Biome, float>[] biomes = new KeyValuePair<Biome, float>[biomeIds.Count];
                for (int i = 0; i < biomeIds.Count; i++)
                {
                    biomes[i] = new KeyValuePair<Biome, float>(BiomeList.Get(biomeIds[i].Key), biomeIds[i].Value);
                }

                Biome = new(biomes);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

    }
}
