using BetterTasks;
using Microsoft.Xna.Framework;
using ProceduralRPG.src.elements.menus;
using System.Threading;

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

            BetterTask task = new(instance.GenerateWorld, ThreadPriority.Highest);
            task.Start();

            return task;
        }

        private void GenerateWorld()
        {
            try
            {
                menu.Log("Task started.");

                world = new(settings);

                for (int x = 0; x < settings.width; x++)
                {
                    for (int y = 0; y < settings.height; y++)
                    {
                        world.Chunks[x, y] = new(world, new(x, y));
                    }
                }
                menu.Log("Chunks initialized.");

                Tectonics.Generate(world);
                GenerateRainfall();

                Vector2 mapPos = new(0.3f, 0f), mapSize = new(0.5f * 1080 / 1920, 0.5f);
                Mapping.DisplayPlateMap(world, new(menu, mapPos, mapSize));
                Mapping.DisplayElevationMap(world, new(menu, new(mapPos.X + mapSize.X, mapPos.Y), mapSize));
                Mapping.DisplayRainfallMap(world, new(menu, new(mapPos.X, mapPos.Y + mapSize.Y), mapSize));
                Mapping.DisplayTemperatureMap(world, new(menu, new(mapPos.X + mapSize.X, mapPos.Y + mapSize.Y), mapSize));

                menu.Log("<color=Green>World generation complete!</>");
                menu.titleElement.SetText("<color=Green>World generation complete!</>");
            }
            catch (System.Exception e)
            {
                menu.Log("<color=Red>Task failed.</>");
                menu.Log(e.Message);
                menu.Log(e.StackTrace ?? "StackTrace is null!");
            }
        }

        private void GenerateRainfall()
        {
            menu.Log("Generating rainfall...");
            float[,] rainfall = new float[settings.width, settings.height];

            // Generate random noise
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    rainfall[x, y] = Utils.RandFloat();
                }
            }

            rainfall = Utils.Smooth(rainfall, settings.rainfallSmoothing);

            // Calculate rainfall for each chunk
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    world.Chunks[x, y].baseRainfallMult = rainfall[x, y];
                }
            }
        }

    }
}
