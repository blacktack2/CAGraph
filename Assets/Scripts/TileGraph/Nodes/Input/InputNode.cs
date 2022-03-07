using System;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Base input node for sending values from a script to a graph.
    /// </summary>
    public abstract class InputNode<InT> : BaseNode, IInputNode
    {
        [SerializeField, Output] private InT _Value;
        [SerializeField]
        private string _Name;

        protected override void Init()
        {
            base.Init();
            if (_Name == null)
                _Name = _Graph.GenerateInOutName("Input");
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_Value")
                return _Value;
            return null;
        }

        public string GetName()
        {
            return _Name;
        }

        public void SetInput<T>(T input)
        {
            if (typeof(T) != typeof(InT))
                throw new ArgumentException(string.Format("Type '{0}' does not match expected type '{1}'",
                                                          typeof(T).Name, typeof(InT).Name));
            _Value = (InT) ((object) input);
        }
    }
}
