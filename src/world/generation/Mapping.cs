using MenuEngine.src;
using MenuEngine.src.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProceduralRPG.src.world.generation
{
    internal static class Mapping
    {

        internal static void DisplayPlateMap(World world, TextureRendererElement map)
        {
            WorldGenerator.instance.menu.Log("Creating plate map...");
            Texture2D texture = new(Engine.Instance.GraphicsDevice, world.Settings.width, world.Settings.height);

            Color[] colors = new Color[world.Settings.width * world.Settings.height];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];

                    int plateId = chunk.plate?.Id ?? 0;

                    Color[] plateColors = new Color[]
                    {
                        Color.Gray, Color.Aquamarine, Color.Plum, Color.Salmon, Color.Pink, Color.RoyalBlue, Color.Beige, Color.BlueViolet, Color.Orange, Color.Orchid,
                        Color.HotPink, Color.Honeydew, Color.Purple, Color.PeachPuff, Color.Olive, Color.MediumOrchid, Color.OliveDrab, Color.DeepPink
                    };
                    Color color = plateColors[plateId % plateColors.Length];

                    colors[x + y * world.Settings.width] = color;
                }
            }

            texture.SetData(colors);
            map.Texture = texture;
        }

        internal static void DisplayElevationmap(World world, TextureRendererElement map)
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

                    if (elevation < world.Settings.defaultElevation)
                        color = Color.Lerp(Color.DarkBlue, Color.Aqua, elevation / (float)world.Settings.defaultElevation);
                    else if (elevation < 12500)
                        color = Color.Lerp(Color.YellowGreen, Color.DarkGreen, (elevation - world.Settings.defaultElevation) / 2500f);
                    else
                        color = Color.Lerp(Color.Gray, Color.White, (elevation - 12500) / 2500f); ;

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
    }
}
