using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ProceduralRPG.src.world
{
    internal class TectonicPlate
    {

        internal int Id { get; private set; }

        private World world;

        internal List<Chunk> Chunks { get; private set; }
        internal List<Chunk> Borders { get; private set; }

        internal Vector2 Movement { get; private set; }

        /// <summary>
        /// Multiplier on the random elevation change applied to each chunk
        /// </summary>
        internal float IntraplateForce { get; private set; }

        internal int avgElevation;

        internal bool IsWater => avgElevation < world.Settings.elevation.baseSeaLevel;
        internal bool IsMountain => avgElevation > world.Settings.elevation.mountainLevel;

        internal TectonicPlate(World world)
        {
            Id = world.TectonicPlates.Count;
            world.TectonicPlates.Add(this);

            this.world = world;

            Chunks = new();
            Borders = new();

            Movement = new((float)Utils.RandFloat(-1, 1), (float)Utils.RandFloat(-1, 1));

            IntraplateForce = (float)Utils.RandFloat(world.Settings.tectonics.minIntraplateForceMult, world.Settings.tectonics.maxIntraplateForceMult);
        }

    }
}
