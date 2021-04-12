
using UnityEngine;

namespace Constants
{
    public static class Numbers
    {
        public const float TAU = 6.283185307179586f;
    }

    public static class SortingLayers
    {
        public const string hexagon = "Hexagon";
        public const string chosenHexagon = "Chosen Hexagon";
        public const string dot = "Dot";
    }

    public static class Tags
    {
        public const string hexagon = "Hexagon";
        public const string player = "Player";
        public const string fallingHexagon = "Falling Hexagon";
    }

    public static class Layers
    {
        public static readonly int hexagon = LayerMask.NameToLayer("Hexagon");
        public static readonly int dot = LayerMask.NameToLayer("Dot");
        public static readonly int Default = LayerMask.NameToLayer("Default");
        public static readonly int chosenHexagon = LayerMask.NameToLayer("Chosen Hexagon");
    }

}

