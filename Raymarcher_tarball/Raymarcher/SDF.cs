using System.Collections.Generic;
using System.Data;

namespace Raymarcher
{
    public abstract class SDF
    {
        public abstract float Evaluate(Vector3 point);

        public abstract SDF Clone(List<float> args, SDF lChild, SDF rChild);
    }
}