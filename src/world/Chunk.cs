using Microsoft.Xna.Framework;
using System;
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

        /// <summary>
        /// In degrees Fahrenheit
        /// </summary>
        internal int Temperature
        {
            get
            {
                int temp = World.Settings.baseTemperature;

                temp -= (int)(0.015f * Math.Abs(elevation - World.Settings.defaultElevation));
                temp -= (int)(120 * Math.Abs(Pos.Y - World.Settings.height / 2) / (World.Settings.height / 2));

                return temp;
            }
        }

        internal float baseRainfallMult;
        /// <summary>
        /// In ml
        /// </summary>
        internal int Rainfall
        {
            get
            {
                float rainfall = baseRainfallMult * World.Settings.baseRainfall;

                rainfall *= 1.2f - (elevation - World.Settings.defaultElevation + 650) / 6500f * 0.7f;
                rainfall *= 1.4f - Math.Abs(Temperature - 55) / 65f;

                return Math.Max((int)rainfall, 0);
            }
        }

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
