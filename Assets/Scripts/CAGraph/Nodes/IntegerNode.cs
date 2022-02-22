using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    /// <summary> Constant node representing an Integer input. </summary>
    [CreateNodeMenu("Constants/Integer", 1)]
    public class IntegerNode : BaseNode
    {
        [SerializeField, Output] private int _IntegerOut = 0;
        
        private void Reset()
        {
            name = "Integer";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_IntegerOut")
                return _IntegerOut;
            return null;
        }
    }
}
