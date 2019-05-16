using System;

namespace Raymarcher
{
    public class OpFunc
    {
        public static float Union(float a, float b)
        {
            return Math.Min(a, b);
        }

        public static float Substraction(float a, float b)
        {
            return Math.Max(a, -b);
        }

        public static float Intersection(float a, float b)
        {
            return Math.Max(a, b);
        }
    }
}