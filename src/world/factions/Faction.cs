﻿namespace ProceduralRPG.src.world.factions
{
    internal class Faction
    {

        internal string Name { get; private set; }
        internal Color PrimaryColor { get; private set; }
        internal Color SecondaryColor { get; private set; }

        internal Faction(string name, Color color, Color secondaryColor)
        {
            Name = name;
            PrimaryColor = color;
            SecondaryColor = secondaryColor;
        }

    }
}