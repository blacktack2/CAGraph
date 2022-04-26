using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Math/Int Math", 5)]
    public class IntMathNode : BaseNode
    {
        [SerializeField, Input] private int _A = 0;
        [SerializeField, Input] private int _B = 0;
        
        [SerializeField, Output] private int _Out = 0;

        [SerializeField, NodeEnum]
        private Operation _Operation = Operation.Add;

        public enum Operation
        {
            Add, Subtract,
            Multiply, Divide,
            Min, Max
        }

        private void Reset()
        {
            name = "Integer Math";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_Out")
            {
                return Calculate();
            }
            return null;
        }

        private int Calculate()
        {
            int a = GetInputValue<int>("_A", _A);
            int b = GetInputValue<int>("_B", _B);

            switch (_Operation)
            {
                case Operation.Add: default:
                    return a + b;
                case Operation.Subtract:
                    return a - b;
                case Operation.Multiply:
                    return a * b;
                case Operation.Divide:
                    return a / b;
                case Operation.Min:
                    return Mathf.Min(a, b);
                case Operation.Max:
                    return Mathf.Max(a, b);
            }
        }
    }
}
