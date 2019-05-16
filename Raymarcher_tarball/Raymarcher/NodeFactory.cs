using System;
using System.Collections.Generic;

namespace Raymarcher
{
    public class NodeData
    {
        public string name;
        public List<float> args;
        public SDF lChild;
        public SDF rChild;

        public NodeData()
        {
            args = new List<float>();
            name = "";
            lChild = null;
            rChild = null;
        }
    }
    
    public class NodeFactory
    {
        private static Dictionary<string, SDF> typelist;

        static NodeFactory()
        {
            typelist = new Dictionary<string, SDF>();
            typelist.Add("sphere", new Sphere());
            typelist.Add("box", new Cube());
            typelist.Add("rbox", new RoundBox());
            typelist.Add("union", new Operator(OpFunc.Union));
            typelist.Add("inter", new Operator(OpFunc.Intersection));
            typelist.Add("sub", new Operator(OpFunc.Substraction));
        }

        public static SDF CreateNode(NodeData data)
        {
            return typelist[data.name].Clone(data.args, data.lChild, data.rChild);
        }
    }
}