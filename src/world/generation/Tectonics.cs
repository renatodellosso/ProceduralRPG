using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralRPG.src.world.generation
{
    internal class Tectonics
    {
        internal static void Generate(World world)
        {
            try
            {
                WorldGenerator.instance.menu.Log("Generating tectonic plates...");

                int plateCount = Utils.RandInt(world.Settings.minPlateCount, world.Settings.maxPlateCount);

                List<TectonicPlate> plates = world.TectonicPlates;

                for (int i = 0; i < plateCount; i++)
                {
                    world.TectonicPlates.Add(new(world));
                }

                LinkedList<TectonicPlate> expandingPlates = new(plates);
                ExpandPlates(world, expandingPlates);

                ApplyForces(world, world.Settings.initialTectonicsLength);

                WorldGenerator.instance.menu.Log("Tectonic plates generated.");
            }
            catch (System.Exception e)
            {
                WorldGenerator.instance.menu.Log("Error generating tectonic plates: " + e.Message);
                WorldGenerator.instance.menu.Log(e.StackTrace ?? "StackTrace is null!");
            }
        }

        private static void ExpandPlates(World world, LinkedList<TectonicPlate> expandingPlates)
        {
            // For every plate, add a random adjacent chunk to it, until there are no more chunks to add
            WorldGenerator.instance.menu.Log("Expanding tectonic plates...");

            while (expandingPlates.Any())
            {
                TectonicPlate plate = expandingPlates.First();

                // If the plate has no chunks, add a random chunk to it
                if (!plate.Chunks.Any())
                {
                    Chunk chunk;
                    do
                    {
                        chunk = world.Chunks[Utils.RandInt(0, world.Settings.width - 1), Utils.RandInt(0, world.Settings.height - 1)];
                    }
                    while (chunk.plate != null);

                    plate.Chunks.Add(chunk);
                    plate.Borders.AddRange(chunk.GetAdjacent());
                }
                else
                {
                    if (!plate.Borders.Any())
                        expandingPlates.RemoveFirst();
                }

                // Remove all borders that are already part of a plate
                for (int i = 0; i < plate.Borders.Count; i++)
                {
                    if (plate.Borders[i].plate != null)
                    {
                        plate.Borders.RemoveAt(i);
                        i--;
                    }
                }

                if (!plate.Borders.Any())
                {
                    if (expandingPlates.Any())
                        expandingPlates.RemoveFirst();
                    continue;
                }

                // Add a random border chunk to the plate
                Chunk borderChunk;
                do
                {
                    borderChunk = plate.Borders[Utils.RandInt(0, plate.Borders.Count - 1)];
                }
                while (borderChunk.plate != null);

                borderChunk.plate = plate;
                plate.Chunks.Add(borderChunk);
                plate.Borders.Remove(borderChunk);
                plate.Borders.AddRange(borderChunk.GetAdjacent());

                // Remove all borders that are already part of a plate
                for (int i = 0; i < plate.Borders.Count; i++)
                {
                    if (plate.Borders[i].plate != null)
                    {
                        plate.Borders.RemoveAt(i);
                        i--;
                    }
                }

                // Move the plate to the back
                expandingPlates.RemoveFirst();
                if (plate.Borders.Any()) expandingPlates.AddLast(plate);
            }
        }

        private static void ApplyForces(World world, int years)
        {
            WorldGenerator.instance.menu.Log($"Applying tectonic forces for {years} years...");

            // Calculate the forces for each chunk
            WorldGenerator.instance.menu.Log("Calculating forces...");
            Vector2[,] forces = new Vector2[world.Settings.width, world.Settings.height];
            Vector2[] platewideForces = new Vector2[world.TectonicPlates.Count];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];
                    if (chunk.plate == null) continue;

                    Vector2 force = new();

                    // Subtract the movement of the chunks being moved into
                    foreach (Chunk adjacent in chunk.GetAdjacent())
                    {
                        if (adjacent.plate == null) continue;

                        /*
                         * Basic theory of what we're doing:
                         * 1 - 1 = 0 (no movement)
                         * 1 - -1 = 2 (Towards each other)
                         * -1 - 1 = -2(Away from each other)
                        */

                        // Calculate forces of this collision
                        Vector2 additionalForce = (chunk.plate!.Movement - adjacent.plate!.Movement) / 2; // Divide by 2 so that the maximum force is 1
                        force += additionalForce;
                    }

                    // Apply the force to the whole plate
                    platewideForces[chunk.plate!.Id] += force;

                    // Set the force in the array to the calculated force
                    // Vector2 is a struct, so we have to do this
                    forces[x, y] = force;
                }
            }

            // Average the platewide forces
            WorldGenerator.instance.menu.Log("Averaging platewide forces...");
            for (int i = 0; i < platewideForces.Length; i++)
            {
                platewideForces[i] /= world.TectonicPlates[i].Chunks.Count;
            }

            // Add the platewide forces to the plates
            WorldGenerator.instance.menu.Log("Applying platewide forces...");
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];
                    if (chunk.plate == null) continue;

                    forces[x, y] += platewideForces[chunk.plate!.Id] * world.Settings.tectonicsPlatewideForceMult;
                }
            }

            // Smooth out the forces
            WorldGenerator.instance.menu.Log("Smoothing forces...");
            for (int i = 0; i < world.Settings.tectonicsSmoothing; i++)
            {
                // Average each force with its adjacent forces

                Vector2[,] newForces = new Vector2[world.Settings.width, world.Settings.height];
                for (int y = 0; y < world.Settings.height; y++)
                {
                    for (int x = 0; x < world.Settings.width; x++)
                    {
                        Chunk chunk = world.Chunks[x, y];
                        Vector2 force = forces[x, y];

                        Chunk[] adjacentChunks = chunk.GetAdjacent();

                        // Add the forces of the adjacent chunks
                        foreach (Chunk adjacent in adjacentChunks)
                        {
                            force += forces[(int)adjacent.Pos.X, (int)adjacent.Pos.Y];
                        }

                        // Average the forces
                        force /= adjacentChunks.Length + 1;

                        newForces[x, y] = force;
                    }
                }
                forces = newForces;
            }

            // Multiply the forces by the time, and then change elevation
            WorldGenerator.instance.menu.Log("Multiplying forces by time...");
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Vector2 force = forces[x, y];
                    Chunk chunk = world.Chunks[x, y];
                    int elevationChange = (int)((force.Y + force.X) * world.Settings.tectonicsForceMult * years);
                    chunk.elevation += elevationChange;
                }
            }

            WorldGenerator.instance.menu.Log("Applied forces.");
        }
    }
}