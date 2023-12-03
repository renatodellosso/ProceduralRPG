namespace ProceduralRPG.src.world.generation
{
    internal struct WorldGenerationSettings
    {

        internal int width, height;

        internal class Tectonics
        {
            internal required int minPlateCount, maxPlateCount;
            /// <summary>
            /// How many years the tectonic plates will move for before continuing with other steps
            /// </summary>
            internal required int initialDuration;
            internal required int lowExtremeElevation, highExtremeElevation;
            internal required float extremeElevationMult;
            internal required int collisionForceSmoothing, forceMult, chunkIntraplateForceSmoothing, intraplateForceElevationMult;
            internal required float collisionForceMult, platewideForceMult, minIntraplateForceMult, maxIntraplateForceMult;
        }
        internal Tectonics tectonics;

        internal class Elevation
        {
            internal required int defaultElevation, baseSeaLevel, mountainLevel, peakLevel;
        }
        internal Elevation elevation;
        // Note about elevation: I find 10,000 works well for sea level and 12500 for snow level

        internal int baseTemperature;

        internal int rainfallSmoothing, baseRainfall;

        internal int maxBiomesPerChunk;

        internal int minInitialFactions, maxInitialFactions;

        public WorldGenerationSettings()
        {
            //Utils.SeedRand(0);

            width = 500;
            height = 300;

            tectonics = new()
            {
                minPlateCount = 15,
                maxPlateCount = 20,

                initialDuration = 1000,

                lowExtremeElevation = 5000,
                highExtremeElevation = 15000,
                extremeElevationMult = 0.175f,

                collisionForceSmoothing = 25,
                forceMult = 100,

                chunkIntraplateForceSmoothing = 250,
                intraplateForceElevationMult = 250,

                collisionForceMult = 0.045f,
                platewideForceMult = 5f,

                minIntraplateForceMult = 0.5f,
                maxIntraplateForceMult = 3.0f
            };

            elevation = new()
            {
                defaultElevation = 9800,
                baseSeaLevel = 10000,
                mountainLevel = 12500,
                peakLevel = 15000
            };

            baseTemperature = 115;

            rainfallSmoothing = 50;
            baseRainfall = 1250;

            maxBiomesPerChunk = 3;

            minInitialFactions = 1;
            maxInitialFactions = 3;
        }

    }
}