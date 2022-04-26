using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Math/Int Clamp", 5)]
    public class IntClampNode : BaseNode
    {
        [SerializeField, Input] private int _Var = 0;
        [SerializeField, Input] private int _Min = 0;
        [SerializeField, Input] private int _Max = 0;
        
        [SerializeField, Output] private int _Out = 0;

        private void Reset()
        {
            name = "Integer Clamp";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_Out")
            {
                return Mathf.Clamp(GetInputValue<int>("_Var", _Var), GetInputValue<int>("_Min", _Min), GetInputValue<int>("_Max", _Max));
            }
            return null;
        }
    }
}
