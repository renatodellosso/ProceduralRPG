using MenuEngine.src;
using MenuEngine.src.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProceduralRPG.src.world.biomes;
using System;

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
                        continue;

                    if (chunk.plate == null)
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

                    if (temperature < -150)
                        color = Color.Lerp(Color.Black, Color.DeepPink, -(temperature + 150) / 100f);
                    else if (temperature < -140)
                        color = Color.DeepPink;
                    else if (temperature < -130)
                        color = Color.Pink;
                    else if (temperature < -120)
                        color = Color.HotPink;
                    else if (temperature < -110)
                        color = Color.LightPink;
                    else if (temperature < -100)
                        color = Color.Magenta;
                    else if (temperature < -90)
                        color = Color.DarkMagenta;
                    else if (temperature < -80)
                        color = Color.Purple;
                    else if (temperature < -70)
                        color = Color.DarkViolet;
                    else if (temperature < -60)
                        color = Color.Fuchsia;
                    else if (temperature < -50)
                        color = Color.DarkGray;
                    else if (temperature < -40)
                        color = Color.Gray;
                    else if (temperature < -30)
                        color = Color.LightGray;
                    else if (temperature < -20)
                        color = Color.DarkBlue;
                    else if (temperature < -10)
                        color = Color.Blue;
                    else if (temperature < 0)
                        color = Color.DeepSkyBlue;
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
                        color = Color.LimeGreen;
                    else if (temperature < 70)
                        color = Color.Lime;
                    else if (temperature < 80)
                        color = Color.Yellow;
                    else if (temperature < 90)
                        color = Color.Orange;
                    else if (temperature < 100)
                        color = Color.OrangeRed;
                    else if (temperature < 110)
                        color = Color.Red;
                    else if (temperature < 120)
                        color = Color.DarkRed;
                    else if (temperature < 130)
                        color = Color.Brown;
                    else if (temperature < 140)
                        color = Color.SaddleBrown;
                    else if (temperature < 150)
                        color = Color.SandyBrown;
                    else if (temperature < 160)
                        color = Color.Peru;
                    else if (temperature < 170)
                        color = Color.Chocolate;
                    else if (temperature < 180)
                        color = Color.Sienna;
                    else if (temperature < 190)
                        color = Color.Maroon;
                    else
                        color = Color.DarkGoldenrod;

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
                    if (rainfall < 100)
                        color = Color.Lerp(Color.Black, Color.Red, rainfall / 100f);
                    else if (rainfall < 250)
                        color = Color.Lerp(Color.Red, Color.Orange, (rainfall - 100) / 250f);
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
            bool logMissingBiomes = false;
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    try
                    {
                        Chunk chunk = world.Chunks[x, y];
                        Biome? biome = chunk.Biome;

                        if (biome == null)
                        {
                            logMissingBiomes = true;
                            colors[x + y * world.Settings.width] = Color.HotPink;
                            continue;
                        }

                        colors[x + y * world.Settings.width] = biome.Color;
                    }
                    catch (Exception e)
                    {
                        WorldGenerator.instance.menu.Log(e.ToString());
                    }
                }
            }

            texture.SetData(colors);
            map.Texture = texture;

            if (logMissingBiomes)
                WorldGenerator.instance.menu.Log("<color=Red>Some chunks lacked biomes!</>");
        }
    }
}
