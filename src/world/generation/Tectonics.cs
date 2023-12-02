using Microsoft.Xna.Framework;
using System;
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

                int plateCount = Utils.RandInt(world.Settings.tectonics.minPlateCount, world.Settings.tectonics.maxPlateCount);

                List<TectonicPlate> plates = world.TectonicPlates;

                for (int i = 0; i < plateCount; i++)
                {
                    world.TectonicPlates.Add(new(world));
                }

                ExpandPlates(world, new LinkedList<TectonicPlate>(plates));

                ApplyForces(world, world.Settings.tectonics.initialDuration);

                //using (TextureRendererElement map = new(WorldGenerator.instance.menu, new(0.3f, 0f), new(0.5f * 1080 / 1920, 0.5f)))
                //{
                //    BetterTask mapTask = new((t) =>
                //    {
                //        try
                //        {
                //            while (!t.IsCanceled)
                //                Mapping.DisplayElevationMap(world, map);
                //        }
                //        catch (System.Exception e)
                //        {
                //            WorldGenerator.instance.menu.Log("Error displaying elevation map: " + e.Message);
                //            WorldGenerator.instance.menu.Log(e.StackTrace ?? "StackTrace is null!");
                //        }
                //    });
                //    mapTask.Start();

                //    int divisor = 100;
                //    for (int i = 0; i < world.Settings.tectonics.initialDuration / divisor; i++)
                //    {
                //        ApplyForces(world, divisor);
                //    }

                //    mapTask.Cancel();
                //}

                ApplyIntraplateForces(world);

                CalculateAvgElevations(world);

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

            //Vector2 mapPos = new(0.3f, 0f), mapSize = new(0.5f * 1080 / 1920, 0.5f);
            //TextureRendererElement map = new(WorldGenerator.instance.menu, mapPos, mapSize);

            //BetterTask task = new((t) =>
            //{
            //    try
            //    {
            //        while (!t.IsCanceled)
            //            Mapping.DisplayPlateMap(world, map);
            //    }
            //    catch (System.Exception e)
            //    {
            //        WorldGenerator.instance.menu.Log("Error displaying plate map: " + e.Message);
            //        WorldGenerator.instance.menu.Log(e.StackTrace ?? "StackTrace is null!");
            //    }
            //});
            //task.Start();

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

                // Remove the border chunk from the borders of other plates
                foreach (Chunk adjacent in borderChunk.GetAdjacent())
                {
                    if (adjacent.plate == null) continue;

                    adjacent.plate.Borders.Remove(borderChunk);
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

                // Move the plate to the back
                expandingPlates.RemoveFirst();
                if (plate.Borders.Any()) expandingPlates.AddLast(plate);
            }

            //task.Cancel();
            //map.Dispose();

            // Check if every chunk is part of a plate
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    if (world.Chunks[x, y].plate == null)
                    {
                        Chunk chunk = world.Chunks[x, y];

                        // Add the chunk to the nearest plate
                        for (int distance = 1; chunk.plate == null; distance++)
                        {
                            Chunk[] adjacent = chunk.GetAdjacent(distance);
                            foreach (Chunk adj in adjacent)
                            {
                                if (adj.plate != null)
                                {
                                    chunk.plate = adj.plate;
                                    chunk.plate.Chunks.Add(chunk);
                                    chunk.plate.Borders.AddRange(chunk.GetAdjacent());
                                    break;
                                }
                            }
                        }
                    }
                }
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
                         * -1 - 1 = -2
                         *   If we're left of adjacent, this is what we want
                         *   If we're right of adjacent, this is opposite of what we want
                        */

                        Vector2 relativePos = chunk.Pos - adjacent.Pos;

                        // Calculate forces of this collision
                        // Divide by 2 so that the maximum force is 1
                        force += world.Settings.tectonics.collisionForceMult * -1 * relativePos * (chunk.plate!.Movement - adjacent.plate!.Movement) / 2;
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

                    Vector2 platewideForce = platewideForces[chunk.plate!.Id] * world.Settings.tectonics.platewideForceMult;
                    forces[x, y] += platewideForce;
                }
            }

            //Smooth out the forces
            WorldGenerator.instance.menu.Log("Smoothing forces...");
            for (int i = 0; i < world.Settings.tectonics.collisionForceSmoothing; i++)
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
                    int elevationChange = (int)((force.X + force.Y) * world.Settings.tectonics.forceMult * years);

                    if (elevationChange == 0) continue;

                    int newElevation = chunk.elevation + elevationChange;
                    if (newElevation < world.Settings.tectonics.lowExtremeElevation || newElevation > world.Settings.tectonics.highExtremeElevation)
                    {
                        int unscaledElevationChange;
                        if (newElevation < world.Settings.tectonics.lowExtremeElevation) unscaledElevationChange = world.Settings.tectonics.lowExtremeElevation - chunk.elevation;
                        else unscaledElevationChange = world.Settings.tectonics.highExtremeElevation - chunk.elevation;

                        elevationChange = (int)((elevationChange - unscaledElevationChange) * world.Settings.tectonics.extremeElevationMult) + unscaledElevationChange;
                    }

                    chunk.elevation = Math.Max(chunk.elevation + elevationChange, 0);
                }
            }

            WorldGenerator.instance.menu.Log("Applied forces.");
        }

        private static void ApplyIntraplateForces(World world)
        {
            WorldGenerator.instance.menu.Log("Applying intraplate forces...");

            // Calculate the forces for each chunk
            float[,] forces = new float[world.Settings.width, world.Settings.height];
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    Chunk chunk = world.Chunks[x, y];
                    if (chunk.plate == null) continue;

                    forces[x, y] = chunk.plate!.IntraplateForce * Utils.RandFloat();
                }
            }

            Utils.Smooth(forces, world.Settings.tectonics.chunkIntraplateForceSmoothing);

            // Scale the forces , and then change elevation
            for (int y = 0; y < world.Settings.height; y++)
            {
                for (int x = 0; x < world.Settings.width; x++)
                {
                    float force = forces[x, y];
                    Chunk chunk = world.Chunks[x, y];
                    int elevationChange = (int)(force * world.Settings.tectonics.intraplateForceElevationMult);
                    chunk.elevation += elevationChange;
                }
            }
        }

        private static void CalculateAvgElevations(World world)
        {
            WorldGenerator.instance.menu.Log("Calculating average elevations...");

            // Calculate the average elevation for each plate
            foreach (TectonicPlate plate in world.TectonicPlates)
            {
                plate.avgElevation = 0;
                foreach (Chunk chunk in plate.Chunks)
                {
                    plate.avgElevation += chunk.elevation;
                }
                plate.avgElevation /= plate.Chunks.Count;
            }
        }
    }
}