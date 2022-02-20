using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Operations/Matrix/Replace", 10)]
    public class MatrixReplaceNode : BaseNode
    {
        [SerializeField, Input] private Types.Matrix01 _MatrixIn;
        [SerializeField, Output] private Types.Matrix01 _MatrixOut;

        [SerializeField]
        private List<int> _ToReplace = new List<int>();
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
                Utilities.MatrixOperations.ReplaceMatrixValues<Types.Matrix01, int>(_MatrixOutBuffer, _ToReplace, _Replacement);
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
