using System.Collections.Generic;

namespace Raymarcher
{
    public delegate float OperatorEval(float a, float b);
    
    public class Operator : SDF
    {
        public SDF Left { get; set; }
        public SDF Right { get; set; }

        public OperatorEval evalFunc;

        public Operator(OperatorEval func)
        {
            evalFunc = func;
        }
        public override float Evaluate(Vector3 point)
        {
            return evalFunc(Left.Evaluate(point), Right.Evaluate(point));
        }

        public override SDF Clone(List<float> args, SDF lChild, SDF rChild)
        {
            Operator ret = new Operator(evalFunc);
            ret.Left = lChild;
            ret.Right = rChild;

            return ret;
        }
    }
}