using System.Collections.Generic;

namespace ProceduralRPG.src.world.resources
{
    internal class ResourceList
    {

        private readonly Resource[] resources = new Resource[]
        {
            new Resource(ResourceId.Grain, "Grain", (world) => Resource.GenerateBasedOnStat(world, (chunk) => chunk.Biome!.Treecover), nutrition: new(0.4f, 1.2f)),
            new Resource(ResourceId.Wood, "Wood", (world) => Resource.GenerateBasedOnStat(world, (chunk) => chunk.Biome!.SoilFertility), hardness: new(2f, 3f)),
            new Resource(ResourceId.Stone, "Stone", Resource.GenerateMineral, hardness: new(2f, 5f)),
            new Resource(ResourceId.Copper, "Copper", Resource.GenerateMineral, hardness: new(2.5f, 3f)),
            new Resource(ResourceId.Tin, "Tin", Resource.GenerateMineral, hardness: new(1.2f, 1.8f)),
            new Resource(ResourceId.Iron, "Iron", Resource.GenerateMineral, hardness: new(3.7f, 4.3f)),
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
