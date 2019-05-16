using System.Collections.Generic;

namespace Raymarcher
{
    public class Sphere : SDF
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }

        public Sphere()
        {
            Center = new Vector3(0f, 0f, 0f);
            Radius = 1f;
        }

        public Sphere(Vector3 c, float r)
        {
            Center = c;
            Radius = r;
        }
        public override float Evaluate(Vector3 point)
        {
            point = point - Center;
            return (point).Magnitude() - Radius;
        }

        public override SDF Clone(List<float> args, SDF lChild, SDF rChild)
        {
            Sphere ret = new Sphere(new Vector3(args[0], args[1], args[2]), args[3]);

            return ret;
        }
    }
}