using MenuEngine.src;
using MenuEngine.src.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProceduralRPG.src.world.biomes;
using System;
using System.Collections.Generic;

namespace ProceduralRPG.src.world.generation
{
    internal static class Mapping
    {

        internal static void DisplayPlateMap(World world, TextureRendererElement map)
        {
            WorldGenerator.instance.menu.Log("Creating plate map...");
            Texture2D texture = new(Engine.Instance.GraphicsDevice, world.Settings.width, world.Settings.height);

            Color[] colors = new Color[world.Settings.width * world.Settings.height];
            foreach (TectonicPlate plate in world.TectonicPlates)
            {
                Chunk[] chunks = plate.Chunks.ToArray();
                foreach (Chunk chunk in chunks)
                {
                    int plateId = chunk.plate?.Id ?? 0;

                    Color[] plateColors = new Color[]
                    {
                        Color.HotPink, Color.Aquamarine, Color.Plum, Color.Salmon, Color.Pink, Color.RoyalBlue, Color.Beige, Color.BlueViolet, Color.Orange, Color.Orchid,
                        Color.Honeydew, Color.Purple, Color.PeachPuff, Color.Olive, Color.MediumOrchid, Color.OliveDrab, Color.DeepPink
                    };
                    Color color = plateColors[plateId % plateColors.Length];

                    colors[(int)chunk.Pos.X + (int)chunk.Pos.Y * world.Settings.width] = color;
                }

                chunks = plate.Borders.ToArray();
                foreach (Chunk chunk in chunks)
                {
                    if (chunk == null)
                    {
                        WorldGenerator.instance.menu.Log("Chunk is null!");
                        continue;
                    }
                    colors[(int)chunk.Pos.X + (int)chunk.Pos.Y * world.Settings.width] = Color.Red;
                }
            }

            texture.SetData(colors);
            map.Texture = texture;
        }

        internal static void DisplayElevationMap(World world, TextureRendererElement map)
        {
            WorldGenerator.instance.menu.Log("Creating elevation map..."); Texture2D texture = new(Engine.Instance.GraphicsDevice,
                world.Settings.width, world.Settings.height);

            Color[] colors = new Color[world.Settings.width * world.Settings.height];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];

                    int elevation = chunk.elevation;
                    Color color;
                    WorldGenerationSettings.Elevation settings = world.Settings.elevation;

                    // Make sure to keep at least 1 float in each division to prevent integer division
                    if (elevation < settings.baseSeaLevel / 2)
                        color = Color.Lerp(Color.Black, Color.DarkBlue, elevation / (settings.baseSeaLevel / 2f));
                    else if (elevation < settings.baseSeaLevel)
                        color = Color.Lerp(Color.DarkBlue, Color.Aqua, (elevation - (settings.baseSeaLevel / 2f)) / (settings.baseSeaLevel / 2));
                    else if (elevation < settings.mountainLevel)
                        color = Color.Lerp(Color.YellowGreen, Color.DarkGreen, (elevation - settings.baseSeaLevel) / (float)(settings.mountainLevel - settings.baseSeaLevel));
                    else if (elevation < settings.peakLevel)
                        color = Color.Lerp(Color.SlateGray, Color.LightGray, (elevation - settings.mountainLevel) / (float)(settings.peakLevel - settings.mountainLevel));
                    else
                        color = Color.Lerp(Color.White, Color.Lerp(Color.DarkSlateGray, Color.DarkSlateBlue, 0.6f), (elevation - settings.peakLevel) / 10000f);

                    colors[x + y * world.Settings.width] = color;
                }
            }

            texture.SetData(colors);
            map.Texture = texture;
        }

        internal static void DisplayRawElevationMap(World world, TextureRendererElement map)
        {
            WorldGenerator.instance.menu.Log("Creating raw elevation map..."); Texture2D texture = new(Engine.Instance.GraphicsDevice,
                world.Settings.width, world.Settings.height);

            Color[] colors = new Color[world.Settings.width * world.Settings.height];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];

                    Color color = Color.Lerp(Color.Black, Color.White, chunk.elevation / 15000f);

                    colors[x + y * world.Settings.width] = color;
                }
            }

            texture.SetData(colors);
            map.Texture = texture;
        }

        internal static void DisplayTemperatureMap(World world, TextureRendererElement map)
        {
            WorldGenerator.instance.menu.Log("Creating temperature map...");

            Texture2D texture = new(Engine.Instance.GraphicsDevice, world.Settings.width, world.Settings.height);

            Color[] colors = new Color[world.Settings.width * world.Settings.height];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];

                    int temperature = chunk.Temperature;
                    Color color;

                    if (temperature < 0)
                        color = Color.DarkBlue;
                    else if (temperature < 10)
                        color = Color.DarkCyan;
                    else if (temperature < 20)
                        color = Color.Cyan;
                    else if (temperature < 30)
                        color = Color.Teal;
                    else if (temperature < 40)
                        color = Color.DarkGreen;
                    else if (temperature < 50)
                        color = Color.Green;
                    else if (temperature < 60)
                        color = Color.LightGreen;
                    else if (temperature < 70)
                        color = Color.GreenYellow;
                    else if (temperature < 80)
                        color = Color.Yellow;
                    else if (temperature < 90)
                        color = Color.Orange;
                    else if (temperature < 100)
                        color = Color.Red;
                    else
                        color = Color.DarkRed;

                    colors[x + y * world.Settings.width] = color;
                }
            }

            texture.SetData(colors);
            map.Texture = texture;
        }

        internal static void DisplayRainfallMap(World world, TextureRendererElement map)
        {
            WorldGenerator.instance.menu.Log("Creating rainfall map...");

            Texture2D texture = new(Engine.Instance.GraphicsDevice, world.Settings.width, world.Settings.height);

            Color[] colors = new Color[world.Settings.width * world.Settings.height];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];

                    int rainfall = chunk.Rainfall;

                    Color color;
                    if (rainfall < 250)
                        color = Color.Lerp(Color.Red, Color.Orange, rainfall / 250f);
                    else if (rainfall < 500)
                        color = Color.Lerp(Color.Orange, Color.Yellow, (rainfall - 250) / 250f);
                    else if (rainfall < 750)
                        color = Color.Lerp(Color.Yellow, Color.Green, (rainfall - 500) / 250f);
                    else if (rainfall < 1000)
                        color = Color.Lerp(Color.Green, Color.Blue, (rainfall - 750) / 250f);
                    else
                        color = Color.Lerp(Color.Blue, Color.White, (rainfall - 1000) / 500f);

                    colors[x + y * world.Settings.width] = color;
                }
            }

            texture.SetData(colors);
            map.Texture = texture;
        }

        internal static void DisplayBiomeMap(World world, TextureRendererElement map)
        {
            WorldGenerator.instance.menu.Log("Creating rainfall map...");

            Texture2D texture = new(Engine.Instance.GraphicsDevice, world.Settings.width, world.Settings.height);

            Color[] colors = new Color[world.Settings.width * world.Settings.height];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    try
                    {
                        Chunk chunk = world.Chunks[x, y];
                        KeyValuePair<Biome, float>[] biomes = chunk.Biomes;

                        Color[] colorArray = new Color[biomes.Length];

                        for (int i = 0; i < biomes.Length; i++)
                        {
                            Biome biome = biomes[i].Key;
                            float value = biomes[i].Value;

                            colorArray[i] = biome.Color * value;
                        }

                        Color color = new();

                        foreach (Color c in colorArray)
                        {
                            color.R += c.R;
                            color.G += c.G;
                            color.B += c.B;
                            color.A += c.A;
                        }

                        colors[x + y * world.Settings.width] = color;
                    }
                    catch (Exception e)
                    {
                        WorldGenerator.instance.menu.Log(e.ToString());
                    }
                }
            }

            texture.SetData(colors);
            map.Texture = texture;
        }
    }
}
