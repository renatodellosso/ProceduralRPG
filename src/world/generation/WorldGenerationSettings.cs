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
        internal int tectonicsSmoothing, tectonicsForceMult, chunkIntraplateForceSmoothing, intraplateForceElevationMult;
        internal float tectonicsPlatewideForceMult, minIntraplateForceMult, maxIntraplateForceMult;

        internal int defaultElevation;
        // Note about elevation: I find 10,000 works well for sea level and 12500 for snow level

        internal int baseTemperature;

        internal int rainfallSmoothing, baseRainfall;

        internal int minInitialFactions, maxInitialFactions;

        internal WorldGenerationSettings()
        {
            width = 450;
            height = 300;

            minPlateCount = 10;
            maxPlateCount = 15;
            initialTectonicsLength = 1000;
            tectonicsSmoothing = 50;
            tectonicsForceMult = 100;
            chunkIntraplateForceSmoothing = 250;
            intraplateForceElevationMult = 250;
            tectonicsPlatewideForceMult = 0.1f;
            minIntraplateForceMult = 0.5f;
            maxIntraplateForceMult = 3.0f;

            defaultElevation = 10000;

            baseTemperature = 110;

            rainfallSmoothing = 50;
            baseRainfall = 1250;

            minInitialFactions = 1;
            maxInitialFactions = 3;
        }

    }
}