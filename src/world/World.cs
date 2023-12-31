﻿using Microsoft.Xna.Framework;
using ProceduralRPG.src.world.generation;
using System;
using System.Collections.Generic;

namespace ProceduralRPG.src.world
{
    internal class World
    {

        internal static World Instance { get; private set; }

        internal WorldGenerationSettings Settings { get; private set; }

        internal Chunk[,] Chunks { get; private set; }
        internal List<TectonicPlate> TectonicPlates { get; private set; }

        internal World(WorldGenerationSettings settings)
        {
            Instance = this;

            Settings = settings;

            Chunks = new Chunk[settings.width, settings.height];
            TectonicPlates = new();
        }

        internal Chunk? this[int x, int y]
        {
            get
            {
                if (x >= Settings.width)
                    x %= Settings.width;
                else if (x < 0)
                    x = Settings.width - Math.Abs(x % Settings.width);

                if (y < 0 || y >= Settings.height)
                {
                    //x += Settings.height / 2;
                    //if (x >= Settings.width)
                    //    x %= Settings.width;
                    //else if (x < 0)
                    //    x = Settings.width - Math.Abs(x % Settings.width);

                    //if (y >= Settings.height)
                    //    y %= Settings.height;
                    //else if (y < 0)
                    //    y = Settings.height - Math.Abs(y % Settings.height);

                    return null;
                }

                return Chunks[x, y];
            }
            set
            {
                x %= Settings.width;
                y %= Settings.height;

                Chunks[x, y] = value!;
            }
        }

        internal Chunk? this[Vector2 pos]
        {
            get => this[(int)pos.X, (int)pos.Y];
            set => this[(int)pos.X, (int)pos.Y] = value;
        }

    }
}
