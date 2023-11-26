﻿using Microsoft.Xna.Framework;
using System.Linq;

namespace ProceduralRPG.src.world
{
    internal class Chunk
    {

        internal World World { get; private set; }

        internal Vector2 Pos { get; private set; }

        internal TectonicPlate? plate;

        /// <summary>
        /// In meters
        /// </summary>
        internal int elevation;

        internal Chunk(World world, Vector2 pos)
        {
            World = world;
            Pos = pos;

            elevation = world.Settings.defaultElevation;
        }

        internal Chunk[] GetAdjacent()
        {
            Vector2[] adjacent = new Vector2[]
            {
                new(0, 1),
                new(1, 1),
                new(1, 0),
                new(1, -1),
                new(0, -1),
                new(-1, -1),
                new(-1, 0),
                new(-1, 1)
            };

            for (int i = 0; i < adjacent.Length; i++)
                adjacent[i] += Pos;

            Chunk?[] chunks = new Chunk?[adjacent.Length];
            for (int i = 0; i < adjacent.Length; i++)
                chunks[i] = World[adjacent[i]];

            return chunks.Where(c => c != null).ToArray()!;
        }

    }
}