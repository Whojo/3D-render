using System.Collections.Generic;

namespace Raymarcher
{
    public class RoundBox : Cube
    {
        private float radius;

        public RoundBox() : this(new Vector3(0f, 0f, 0f),
            new Vector3(0.8f, 0.8f, 0.8f), 0.2f)
        {
            
        }
        public RoundBox(Vector3 center, Vector3 dim, float radius) : base(center, dim)
        {
            this.radius = radius;
        }

        public override float Evaluate(Vector3 point)
        {
            return base.Evaluate(point) - radius;
        }

        public override SDF Clone(List<float> args, SDF lChild, SDF rChild)
        {
            RoundBox ret = new RoundBox(new Vector3(args[0], args[1], args[2]),
                new Vector3(args[3], args[4], args[5]),
                args[6]);

            return ret;
        }
    }
}