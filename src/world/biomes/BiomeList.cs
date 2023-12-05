using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ProceduralRPG.src.world.biomes
{
    internal class BiomeList
    {

        private static float Score(float value, float target, float min, float max)
        {
            float diff = value - target;
            float range;
            if (diff < 0)
                range = min - target; // Will be negative, but so will diff
            else
                range = max - target;

            return 1 - Math.Clamp(diff / range, 0, 1);
        }

        private static float Score(float value, float target, float range) => Score(value, target, target - range, target + range);

        private readonly Biome[] biomes = new Biome[]
        {
            // Water biomes
            new(BiomeId.DeepOcean, Color.Blue, (chunk) =>
            {
                if (!chunk.IsWater || !chunk.plate!.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel * 0.7f, seaLevel) * Score(chunk.Temperature, 70, 10, 130);
            }, 0, 0),
            new(BiomeId.ShallowOcean, Color.Aqua, (chunk) =>
            {
                if (!chunk.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Math.Max(Score(chunk.elevation, seaLevel * 0.85f, seaLevel * 0.2f), Score(chunk.GetMarshiness(), 0.25f, 1f)) * Score(chunk.Temperature, 70, 10, 130);
            }, 0, 0),
            new(BiomeId.FreshWater, Color.Turquoise, (chunk) =>
            {
                if (!chunk.IsWater || chunk.plate!.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel, seaLevel * 0.2f) * (0.5f + 0.5f * Score(chunk.GetMarshiness(), 0f, 0.5f)) * Score(chunk.Temperature, 40, 70);
            }, 0, 0),
            new(BiomeId.SeaIce, Color.Lerp(Color.LightGray, Color.Aqua, 0.3f), (chunk) =>
            {
                if (!chunk.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel * 0.85f, seaLevel) * Score(chunk.Temperature, -20, -100, 20);
            }, 0, 0),
            new(BiomeId.OceanicAbyss, Color.DarkBlue, (chunk) =>
            {
                if (!chunk.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel * 0.1f, 0, seaLevel * 0.3f) * Score(chunk.Temperature, 70, 10, 120);
            }, 0, 0),

            // Misc land biomes
            new(BiomeId.Grassland, Color.YellowGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.GetRockiness(), 0, 250) * Score(chunk.Rainfall, 700, 150, 850) * Score(chunk.Temperature, 70, 40, 90);
            }, 0.2f, 1),
            new(BiomeId.Hills, Color.DarkOliveGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.GetRockiness(), 250, 100, 400) * Score(chunk.Rainfall, 700, 200) * Score(chunk.Temperature, 70, 25);
            }, 0.15f, 1),
            new(BiomeId.AridShrubland, Color.Olive, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.GetRockiness(), 100, 250) * Score(chunk.Rainfall, 400, 750, 100) * Score(chunk.Temperature, 80, 50, 115);
            }, 0.1f, 0.6f),
            new(BiomeId.Mountains, Color.Lerp(Color.Gray, Color.DarkGreen, 0.8f), (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.GetRockiness(), 350, 150, int.MaxValue) * Score(chunk.Temperature, 75, 25);
            }, 0.7f, 0.7f),
            new(BiomeId.AridMountains, Color.DarkOliveGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.GetRockiness(), 350, 150, int.MaxValue) * Score(chunk.Rainfall, 500, 400) * Score(chunk.Temperature, 80, 50, 115);
            }, 0.05f, 0.4f),
            new(BiomeId.Beach, Color.Lerp(Color.SandyBrown, Color.Yellow, 0.4f), (chunk) =>
            {
                if (chunk.IsWater || !chunk.plate!.IsWater)
                    return 0;
                return Score(chunk.Temperature, 70, 25);
            }, 0.3f, 0.5f),
            new(BiomeId.Swamp, Color.Lerp(Color.Red, Color.Olive, 0.5f), (chunk) =>
            {
                if (chunk.plate!.IsWater)
                    return 0;
                return Score(chunk.GetMarshiness(), 0.5f, 0.5f) * Score(chunk.Rainfall, 700, 200) * Score(chunk.Temperature, 80, 70, 115);
            }, 0.4f, 0.4f),

            // Forest land biomes
            new(BiomeId.Forest, Color.Green, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;

                // We use 10,000 so as to weight Rainforest more as rainfall increases
                return Score(chunk.Rainfall, 800, 250, 10000) * Score(chunk.Temperature, 60, 40, 76);
            }, 1f, 1f),
            new(BiomeId.Taiga, Color.Lerp(Color.White, Color.DarkGreen, 0.5f), (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 800, 250) * Score(chunk.Temperature, 45, -10, 50);
            }, 1f, 0.6f),
            new(BiomeId.Rainforest, Color.DarkOliveGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 1200, 850, int.MaxValue) * Score(chunk.Temperature, 85, 75, 115);
            }, 1.3f, 0.8f),

            // Desert land biomes
            new(BiomeId.Desert, Color.SandyBrown, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Math.Max(Score(chunk.Rainfall, 450, -1, 600), Score(chunk.Temperature, 110, 90, 140)) * Score(chunk.Temperature, 90, 80, 130);
            }, 0f, 0f),
            new(BiomeId.ExtremeDesert, Color.Lerp(Color.Yellow, Color.SandyBrown, 0.4f), (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 100, int.MinValue, 600) * Score(chunk.Temperature, 110, 95, int.MaxValue);
            }, 0f, 0f),
            new(BiomeId.ColdDesert, Color.Lerp(Color.SandyBrown, Color.White, 0.5f), (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 450, -1, 600) * Score(chunk.Temperature, 40, -10, 55);
            }, 0f, 0f),

            // Cold land biomes
            new(BiomeId.Tundra, Color.White, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 400, -1, int.MaxValue) * Score(chunk.Temperature, 30, -35, 45);
            }, 0.1f, 0.1f),
            new(BiomeId.IceSheet, Color.LightGray, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;

                // The min for rainfall is -200, so 0-rainfall chunks get weight
                return Score(chunk.Rainfall, 400, -200, int.MaxValue) * Score(chunk.Temperature, -60, -100, 5);
            }, 0f, 0f),
            new(BiomeId.FrigidWasteland, Color.Gray, (chunk) =>
            {
                return Score(chunk.Temperature, -100, int.MinValue, -50);
            }, 0f, 0f)
        };

        private readonly Dictionary<BiomeId, Biome> biomeDict;

        private readonly static BiomeList instance = new();

        private BiomeList()
        {
            biomeDict = new();

            foreach (Biome biome in biomes)
                biomeDict.Add(biome.Id!.Value, biome);
        }

        internal static Biome Get(BiomeId id) => instance.biomeDict[id];

        internal static Biome[] GetAllBiomes() => instance.biomes;
    }
}
