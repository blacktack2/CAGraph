using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Input node representing a Float input. </summary>
    [CreateNodeMenu("Input/Float", 2)]
    public class FloatNode : InputNode<float>
    {
        private void Reset()
        {
            name = "Float";
        }
    }
}
