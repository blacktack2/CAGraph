using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Input/Integer", 1)]
    public class IntegerNode : BaseNode
    {
        [SerializeField, Output] private int _IntegerOut;
        [SerializeField]
        private int _IntegerOutBuffer = 0;
        
        private void Reset()
        {
            name = "Integer";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_IntegerOut")
                return _IntegerOutBuffer;
            return null;
        }
    }
}
