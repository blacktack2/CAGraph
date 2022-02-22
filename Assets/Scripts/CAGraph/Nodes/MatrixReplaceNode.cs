using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    /// <summary> Operation node for replacing specific values in a
    /// <paramref name="Matrix" /> with another value. </summary>
    [CreateNodeMenu("Operations/Matrix/Replace", 10)]
    public class MatrixReplaceNode : BaseNode
    {
        [SerializeField, Input] private Types.Matrix01 _MatrixIn;
        [SerializeField, Output] private Types.Matrix01 _MatrixOut;

        /// <summary> List of all values to be replaced by
        /// <paramref name="_Replacement" />. </summary>
        [SerializeField]
        private List<int> _ToReplace = new List<int>();
        /// <summary> Value to replace contents of
        /// <paramref name="_ToReplace" /> with. </summary>
        [SerializeField]
        private int _Replacement = 0;

        public bool toReplaceChanged = false;
        private List<int> _CurrentToReplace = null;
        private int _CurrentReplacement = 0;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix01 _MatrixOutBuffer;

        private void OnValidate()
        {
            toReplaceChanged = !CompareLists(_ToReplace, _CurrentToReplace);
        }

        private void Reset()
        {
            name = "Replace Value";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_MatrixOut")
            {
                GetMatrixInput(
                    "_MatrixIn", "_MatrixOut",
                    ref _MatrixOutBuffer, ref _MatrixInIDBuffer,
                    _Replacement != _CurrentReplacement || toReplaceChanged
                );
                return _MatrixOutBuffer;
            }
            return null;
        }

        protected override void UpdateMatrixOutput(string portName)
        {
            if (portName == "_MatrixOut")
            {
                _CurrentReplacement = _Replacement;
                toReplaceChanged = false;
                // Cast contents of _ToReplace to IConvertible
                List<IConvertible> toReplace = new List<IConvertible>();
                foreach (int v in _ToReplace)
                    toReplace.Add(v);
                Utilities.MatrixOperations.ReplaceMatrixValues(_MatrixOutBuffer, toReplace, _Replacement);
            }
        }

        private bool CompareLists<T>(List<T> a, List<T> b)
        {
            if (a == b)
                return true;
            if (a == null || b == null)
                return false;
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
                if (!a[i].Equals(b[i]))
                    return false;
            return true;
        }
    }
}
