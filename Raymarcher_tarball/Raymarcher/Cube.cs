using System;
using System.Collections.Generic;

namespace Raymarcher
{
    public class Cube : SDF
    {
        private Vector3 dim;
        private Vector3 center;

        public Cube() : this(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f))
        {
        }

        public Cube(Vector3 center, Vector3 dim)
        {
            this.dim = dim;
            this.center = center;
        }
        public override float Evaluate(Vector3 point)
        {
            Vector3 p = point - center; //Tranform point to local space
            Vector3 b = dim * 0.5f; //Use half dimensions for the SDF
            Vector3 d = p.Abs() - b;

            return Vector3.Max(d, 0f).Magnitude() + Math.Min(Math.Max(d.x, Math.Max(d.y, d.z)), 0f);
        }

        public override SDF Clone(List<float> args, SDF lChild, SDF rChild)
        {
            Cube ret = new Cube(new Vector3(args[0], args[1], args[2]),
                new Vector3(args[3], args[4], args[5]));

            return ret;
        }
    }
}