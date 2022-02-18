using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Input/Float", 1)]
    public class FloatNode : BaseNode
    {
        [SerializeField, Output] private float _FloatOut;
        [SerializeField]
        private float _FloatOutBuffer = 0f;
        
        private void Reset()
        {
            name = "Float";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_FloatOut")
                return _FloatOutBuffer;
            return null;
        }
    }
}
