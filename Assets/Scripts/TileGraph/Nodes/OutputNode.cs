using System;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Base output node for sending values to a script. </summary>
    public abstract class OutputNode<OutT> : BaseNode, IOutputNode
    {
        [SerializeField, Input] private OutT _In;
        [SerializeField]
        private string _OutputName;

        protected override void Init()
        {
            base.Init();
            if (_OutputName == null)
                _OutputName = _Graph.GenerateOutputName();
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }

        public string GetName()
        {
            return _OutputName;
        }

        public T GetOutput<T>()
        {
            if (typeof(T) != typeof(OutT))
                throw new ArgumentException(string.Format("Type '{}' does not match expected type '{}'",
                                                          typeof(T).Name, typeof(OutT).Name));
            return GetInputPort("_In").GetInputValue<T>();
        }
    }
}
