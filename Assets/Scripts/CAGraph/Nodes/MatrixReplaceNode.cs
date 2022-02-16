using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Operations/Matrix/Replace", 10)]
    public class MatrixReplaceNode : Node
    {
        [SerializeField, Input] private Types.Matrix _MatrixIn;
        [SerializeField, Output] private Types.Matrix _MatrixOut;

        [SerializeField]
        private List<int> _ToReplace = new List<int>();
        [SerializeField]
        private int _Replacement = 0;

        public bool toReplaceChanged = false;
        private List<int> _CurrentToReplace = null;
        private int _CurrentReplacement = 0;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix _MatrixOutBuffer;

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
                return GetReplacedMatrix();
            return null;
        }

        private Types.Matrix GetReplacedMatrix()
        {
            Types.Matrix matrix = GetInputValue<Types.Matrix>("_MatrixIn");
            if (matrix == null)
            {
                _MatrixInIDBuffer = 0L;
                _MatrixOutBuffer = null;
                return null;
            }
            else if (_MatrixOutBuffer == null || matrix.id != _MatrixInIDBuffer)
            {
                _MatrixInIDBuffer = matrix.id;
                _MatrixOutBuffer = matrix.Copy();
                Replace();
            }
            else if (_Replacement != _CurrentReplacement || toReplaceChanged)
            {
                _MatrixOutBuffer = matrix.Copy();
                Replace();
            }
            return _MatrixOutBuffer;
        }

        private void Replace()
        {
            _CurrentReplacement = _Replacement;
            toReplaceChanged = false;
            Utilities.MatrixOperations.ReplaceMatrixValues(_MatrixOutBuffer, _ToReplace, _Replacement);
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
