using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Operations/Matrix/Clear", 10)]
    public class MatrixClearNode : Node
    {
        [SerializeField, Input] private Types.Matrix _MatrixIn;
        [SerializeField, Output] private Types.Matrix _MatrixOut;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix _MatrixOutBuffer;

        private void Reset()
        {
            name = "Clear Matrix";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_MatrixOut")
                return GetClearedMatrix();
            return null;
        }

        private Types.Matrix GetClearedMatrix()
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
                Utilities.MatrixOperations.ClearMatrix(_MatrixOutBuffer);
            }
            return _MatrixOutBuffer;
        }
    }
}
