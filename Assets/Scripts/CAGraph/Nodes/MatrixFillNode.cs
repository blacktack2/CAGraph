using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    /// <summary> Operation node for filling a <paramref name="Matrix" /> with
    /// a single value. </summary>
    [CreateNodeMenu("Operations/Matrix/Fill", 10)]
    public class MatrixFillNode : BaseNode
    {
        [SerializeField, Input] private Types.Matrix01 _MatrixIn;
        [SerializeField, Output] private Types.Matrix01 _MatrixOut;

        /// <summary> Value to fill the matrix with. </summary>
        [SerializeField]
        private int _FillValue = 0;

        private int _CurrentFillValue = -1;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix01 _MatrixOutBuffer;

        private void Reset()
        {
            name = "Fill Matrix";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_MatrixOut")
            {
                GetMatrixInput(
                    "_MatrixIn", "_MatrixOut",
                    ref _MatrixOutBuffer, ref _MatrixInIDBuffer,
                    _FillValue != _CurrentFillValue
                );
                return _MatrixOutBuffer;
            }
            return null;
        }
        
        protected override void UpdateMatrixOutput(string portName)
        {
            if (portName == "_MatrixOut")
            {
                _CurrentFillValue = _FillValue;
                Utilities.MatrixOperations.FillMatrix(_MatrixOutBuffer, _FillValue);
            }
        }
    }
}
