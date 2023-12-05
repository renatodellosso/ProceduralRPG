using System.Collections.Generic;

namespace ProceduralRPG.src.world.resources
{
    internal class ResourceList
    {

        private readonly Resource[] resources = new Resource[]
        {
            new Resource(ResourceId.Grain, "Grain", (world) => Resource.GenerateBasedOnStat(world, (chunk) => chunk.Biome!.Treecover), nutrition: 1),
            new Resource(ResourceId.Wood, "Wood", (world) => Resource.GenerateBasedOnStat(world, (chunk) => chunk.Biome!.SoilFertility)),
            new Resource(ResourceId.Stone, "Stone", (world) => Resource.GenerateMineral(world)),
            new Resource(ResourceId.Copper, "Copper", (world) => Resource.GenerateMineral(world)),
            new Resource(ResourceId.Tin, "Tin", (world) => Resource.GenerateMineral(world)),
            new Resource(ResourceId.Iron, "Iron", (world) => Resource.GenerateMineral(world)),
        };

        private readonly Dictionary<ResourceId, Resource> resourceDict;

        private readonly static ResourceList instance = new();

        private ResourceList()
        {
            resourceDict = new();

            foreach (Resource resource in resources)
                resourceDict.Add(resource.Id!.Value, resource);
        }

        internal static Resource Get(ResourceId id) => instance.resourceDict[id];

        internal static Resource[] GetAllResources() => instance.resources;

    }
}
