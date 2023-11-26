using BetterTasks;

namespace ProceduralRPG.src.worldgeneration
{
    internal class WorldGenerator
    {

        private static WorldGenerator instance;

        internal static BetterTask Start()
        {
            instance = new();

            BetterTask task = new(instance.GenerateWorld);
            task.Start();

            return task;
        }

        private void GenerateWorld()
        {

        }

    }
}
