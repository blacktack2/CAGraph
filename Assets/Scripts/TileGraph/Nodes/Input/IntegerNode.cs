using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Input node representing an Integer input. </summary>
    [CreateNodeMenu("Input/Integer", 1)]
    public class IntegerNode : InputNode<int>
    {
        private void Reset()
        {
            name = "Integer";
        }
    }
}
