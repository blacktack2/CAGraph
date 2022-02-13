using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Operations/Matrix/Fill", 10)]
    public class MatrixFillNode : Node
    {
        [SerializeField, Input] private Types.Matrix _MatrixIn;
        [SerializeField, Output] private Types.Matrix _MatrixOut;

        [SerializeField]
        private int _FillValue = 0;

        private int _CurrentFillValue = -1;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix _MatrixOutBuffer;

        private void Reset()
        {
            name = "Fill Matrix";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_MatrixOut")
                return GetFilledMatrix();
            return null;
        }

        private Types.Matrix GetFilledMatrix()
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
                Fill();
            }
            else if (_FillValue != _CurrentFillValue)
            {
                Fill();
            }
            return _MatrixOutBuffer;
        }

        private void Fill()
        {
            _CurrentFillValue = _FillValue;
            Utilities.MatrixOperations.FillMatrix(_MatrixOutBuffer, _FillValue);
        }
    }
}
