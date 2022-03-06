using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Input node representing a Float input. </summary>
    [CreateNodeMenu("Input/Float", 1)]
    public class FloatNode : BaseNode
    {
        [SerializeField, Output] private float _FloatOut = 0f;
        
        private void Reset()
        {
            name = "Float";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_FloatOut")
                return _FloatOut;
            return null;
        }
    }
}
