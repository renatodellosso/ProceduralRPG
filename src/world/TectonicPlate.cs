using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ProceduralRPG.src.world
{
    internal class TectonicPlate
    {

        internal int Id { get; private set; }

        internal List<Chunk> Chunks { get; private set; }
        internal List<Chunk> Borders { get; private set; }

        internal Vector2 Movement { get; private set; }

        internal TectonicPlate(World world)
        {
            Id = world.TectonicPlates.Count;
            world.TectonicPlates.Add(this);

            Chunks = new();
            Borders = new();

            Movement = new((float)Utils.RandFloat(1, -1), (float)Utils.RandFloat(1, -1));
        }

    }
}
