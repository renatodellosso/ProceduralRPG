using BetterTasks;
using MenuEngine.src.elements;
using Microsoft.Xna.Framework;
using ProceduralRPG.src.elements.menus;

namespace ProceduralRPG.src.world.generation
{
    internal class WorldGenerator
    {

        internal static WorldGenerator instance;

        internal WorldGenerationMenu menu;
        internal WorldGenerationSettings settings;

        internal World world;

        internal static BetterTask Start(WorldGenerationMenu menu, WorldGenerationSettings settings)
        {
            menu.Log("Starting world generation...");

            instance = new()
            {
                menu = menu,
                settings = settings
            };

            BetterTask task = new(instance.GenerateWorld, System.Threading.ThreadPriority.Highest);
            task.Start();
            return task;
        }

        private void GenerateWorld()
        {
            try
            {
                menu.Log("Task started.");

                World world = new(settings);
                TextureRendererElement? map = null;

                do
                {

                    for (int x = 0; x < world.Settings.width; x++)
                    {
                        for (int y = 0; y < world.Settings.height; y++)
                        {
                            world.Chunks[x, y] = new(world, new(x, y));
                        }
                    }
                    menu.Log("Chunks initialized.");

                    Tectonics.Generate(world);
                    GenerateRainfall(world);

                    CalculateBiomes(world);

                    map?.Dispose();
                    Vector2 mapPos = new(0.3f, 0f), mapSize = new(0.5f * 1080 / 1920, 0.5f);
                    map = new(menu, mapPos, mapSize);

                    Mapping.DisplayBiomeMap(world, map);

                    menu.Log("<color=Green>World generation complete!</>");
                    menu.titleElement.SetText("<color=Green>World generation complete!</>");
                } while (false);
            }
            catch (System.Exception e)
            {
                menu.Log("<color=Red>Task failed.</>");
                menu.Log(e.Message);
                menu.Log(e.StackTrace ?? "StackTrace is null!");
            }
        }

        private void GenerateRainfall(World world)
        {
            menu.Log("Generating rainfall...");
            float[,] rainfall = new float[world.Settings.width, world.Settings.height];

            // Generate random noise
            for (int x = 0; x < world.Settings.width; x++)
            {
                for (int y = 0; y < world.Settings.height; y++)
                {
                    rainfall[x, y] = Utils.RandFloat();
                }
            }

            rainfall = Utils.Smooth(rainfall, world.Settings.rainfallSmoothing);

            // Calculate rainfall for each chunk
            for (int x = 0; x < world.Settings.width; x++)
            {
                for (int y = 0; y < world.Settings.height; y++)
                {
                    world.Chunks[x, y].baseRainfallMult = rainfall[x, y];
                }
            }
        }

        private void CalculateBiomes(World world)
        {
            menu.Log("Calculating biomes...");

            // Calculate biome for each chunk
            for (int x = 0; x < world.Settings.width; x++)
            {
                for (int y = 0; y < world.Settings.height; y++)
                {
                    world.Chunks[x, y].CalculateBiomes();
                }
            }
        }

    }
}
