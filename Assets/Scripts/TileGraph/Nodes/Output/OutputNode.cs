using System;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Base output node for sending values to a script. </summary>
    public abstract class OutputNode<OutT> : BaseNode, IOutputNode
    {
        [SerializeField, Input] private OutT _Value;
        [SerializeField]
        private string _Name;

        protected override void Init()
        {
            base.Init();
            if (_Name == null)
                _Name = _Graph.GenerateInOutName("Output");
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }

        public string GetName()
        {
            return _Name;
        }

        public Type GetValueType()
        {
            return typeof(OutT);
        }

        public T GetOutput<T>()
        {
            if (typeof(T) != typeof(OutT))
                throw new ArgumentException(string.Format("Type '{0}' does not match expected type '{1}'",
                                                          typeof(T).Name, typeof(OutT).Name));
            return GetInputPort("_Value").GetInputValue<T>();
        }
    }
}
