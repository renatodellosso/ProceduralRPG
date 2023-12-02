using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ProceduralRPG.src.world.biomes
{
    internal class BiomeList
    {

        private static float Score(float value, float target, float min, float max)
        {
            float diff = target - value;

            if (diff < 0)
                return Math.Clamp(1 - diff / (min - target), 0, 1);
            return Math.Clamp(1 - diff / (max - target), 0, 1);
        }

        private static float Score(float value, float target, float range) => Score(value, target, target - range, target + range);

        private readonly Biome[] biomes = new Biome[]
        {
            new(BiomeId.DeepOcean, Color.Blue, (chunk) =>
            {
                if (!chunk.IsWater || !chunk.plate!.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel * 0.7f, seaLevel) * Score(chunk.Temperature, 70, 10, 120);
            }),
            new(BiomeId.ShallowOcean, Color.Aqua, (chunk) =>
            {
                if (!chunk.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel * 0.85f, seaLevel * 0.2f) * Score(chunk.Temperature, 70, 10, 120);
            }),
            new(BiomeId.FreshWater, Color.Turquoise, (chunk) =>
            {
                if (!chunk.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel, seaLevel * 0.2f) * (0.5f + 0.5f * Score(chunk.GetMarshiness(), 0.3f, 0.5f)) * Score(chunk.Temperature, 70, 10, 120);
            }),
            new(BiomeId.SeaIce, Color.Lerp(Color.LightGray, Color.Aqua, 0.3f), (chunk) =>
            {
                if (!chunk.IsWater)
                    return 0;

                int seaLevel = World.Instance.Settings.elevation.baseSeaLevel;
                return Score(chunk.elevation, seaLevel * 0.85f, seaLevel) * Score(chunk.Temperature, -20, -100, 20);
            }),
            new(BiomeId.Grassland, Color.YellowGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.GetRockiness(), 0, 250) * Score(chunk.Rainfall, 700, 200) * Score(chunk.Temperature, 70, 25);
            }),
            new(BiomeId.Hills, Color.DarkOliveGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.GetRockiness(), 250, 100, 400) * Score(chunk.Rainfall, 700, 200) * Score(chunk.Temperature, 70, 25);
            }),
            new(BiomeId.Forest, Color.Green, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 800, 250) * Score(chunk.Temperature, 60, 30);
            }),
            new(BiomeId.Taiga, Color.DarkGreen, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 800, 250) * Score(chunk.Temperature, 40, -10, 50);
            }),
            new(BiomeId.Desert, Color.SandyBrown, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 500, 0, 600) * Score(chunk.Temperature, 90, 80, 120);
            }),
            new(BiomeId.ExtremeDesert, Color.Lerp(Color.Yellow, Color.SandyBrown, 0.4f), (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 200, 0, 600) * Score(chunk.Temperature, 110, 95, 150);
            }),
            new(BiomeId.Tundra, Color.White, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 500, 0, 1200) * Score(chunk.Temperature, 30, -35, 45);
            }),
            new(BiomeId.IceSheet, Color.LightGray, (chunk) =>
            {
                if (chunk.IsWater)
                    return 0;
                return Score(chunk.Rainfall, 400, 0, 800) * Score(chunk.Temperature, -60, -100, 0);
            }),
            new(BiomeId.FrigidWasteland, Color.Gray, (chunk) =>
            {
                return Score(chunk.Temperature, -100, -200, -50);
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
