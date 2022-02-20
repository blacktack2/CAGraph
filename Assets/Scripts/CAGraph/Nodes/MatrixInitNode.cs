using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Input/Matrix", 0)]
    public class MatrixInitNode : BaseNode
    {
        [SerializeField]
        private Types.Matrix01 _Matrix;
        [SerializeField, Range(2, Types.Matrix01.maxMatrixSize)]
        private int _MatrixWidth = 100, _MatrixHeight = 100;

        [SerializeField, Output] private Types.Matrix01 _MatrixOut;

        private Types.Matrix01 _MatrixOutBuffer;

        private void Reset()
        {
            name = "Matrix";
        }

        protected override void Init()
        {
            base.Init();
            if (_Matrix == null || _MatrixOut == null)
            {
                ResetMatrix();
            }
        }

        public override object GetValue(NodePort port)
        {
            return _MatrixOutBuffer;
        }

        public void UpdateMatrix()
        {
            if (_Matrix == null || _MatrixWidth != _Matrix.width || _MatrixHeight != _Matrix.height)
            {
                ResetMatrix();
            }
        }

        private void ResetMatrix()
        {
            _Matrix = new Types.Matrix01(_MatrixWidth, _MatrixHeight);
            _MatrixOutBuffer = _Matrix.Clone();
        }
    }
}
