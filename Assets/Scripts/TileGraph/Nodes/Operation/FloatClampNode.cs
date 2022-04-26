using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Math/Float Clamp", 5)]
    public class FloatClampNode : BaseNode
    {
        [SerializeField, Input] private float _Var = 0;
        [SerializeField, Input] private float _Min = 0;
        [SerializeField, Input] private float _Max = 0;
        
        [SerializeField, Output] private float _Out = 0;

        private void Reset()
        {
            name = "Float Clamp";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_Out")
            {
                return Mathf.Clamp(GetInputValue<float>("_Var", _Var), GetInputValue<float>("_Min", _Min), GetInputValue<float>("_Max", _Max));
            }
            return null;
        }
    }
}
