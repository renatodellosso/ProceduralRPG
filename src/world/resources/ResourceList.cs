using System.Collections.Generic;

namespace ProceduralRPG.src.world.resources
{
    internal class ResourceList
    {

        private readonly Resource[] resources = new Resource[]
        {
            new Resource(ResourceId.Grain, "Grain", (world) => Resource.GenerateBasedOnStat(world, (chunk) => chunk.Biome!.Treecover), nutrition: 1),
            new Resource(ResourceId.Wood, "Wood", (world) => Resource.GenerateBasedOnStat(world, (chunk) => chunk.Biome!.SoilFertility), hardness: 0.4f),
            new Resource(ResourceId.Stone, "Stone", Resource.GenerateMineral, hardness: 0.6f),
            new Resource(ResourceId.Copper, "Copper", Resource.GenerateMineral),
            new Resource(ResourceId.Tin, "Tin", Resource.GenerateMineral),
            new Resource(ResourceId.Iron, "Iron", Resource.GenerateMineral),
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
