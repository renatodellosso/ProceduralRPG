namespace ProceduralRPG.src.world.generation
{
    internal class WorldGenerationSettings
    {

        internal int width, height;

        internal int minPlateCount, maxPlateCount;
        /// <summary>
        /// How many years the tectonic plates will move for before continuing with other steps
        /// </summary>
        internal int initialTectonicsLength;
        internal int tectonicsSmoothing, tectonicsForceMult;
        internal float tectonicsPlatewideForceMult;

        internal int defaultElevation;
        // Note about elevation: I find 10,000 works well for sea level and 12500 for snow level

        internal int baseTemperature;

        internal WorldGenerationSettings()
        {
            width = 450;
            height = 300;

            minPlateCount = 10;
            maxPlateCount = 15;
            initialTectonicsLength = 1000;
            tectonicsSmoothing = 50;
            tectonicsForceMult = 100;
            tectonicsPlatewideForceMult = 0.1f;

            defaultElevation = 10000;

            baseTemperature = 110;
        }

    }
}