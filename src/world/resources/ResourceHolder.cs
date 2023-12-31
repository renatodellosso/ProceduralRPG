﻿namespace ProceduralRPG.src.world.resources
{
    internal class ResourceHolder
    {

        internal ResourceId Id { get; private set; }

        internal float Amount { get; private set; }
        internal float Quality { get; private set; }

        internal Resource Resource => ResourceList.Get(Id);

        internal float Nutrition => Resource.Nutrition[Quality];
        internal float Hardness => Resource.Hardness[Quality];

        internal ResourceHolder(ResourceId id, float amount, float quality)
        {
            Id = id;
            Amount = amount;
            Quality = quality;
        }

    }
}
