using BetterTasks;
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

            BetterTask task = new(instance.GenerateWorld);
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

                menu.Log("<color=Green>World generation complete!</>");
            }
            catch (System.Exception e)
            {
                menu.Log("<color=Red>Task failed.</>");
                menu.Log(e.Message);
                menu.Log(e.StackTrace ?? "StackTrace is null!");
            }
        }

    }
}
