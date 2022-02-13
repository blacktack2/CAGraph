using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Input/Matrix", 0)]
    public class MatrixInitNode : Node
    {
        [SerializeField]
        private Types.Matrix _Matrix;
        [SerializeField, Range(2, 200)]
        private int _MatrixWidth = 100, _MatrixHeight = 100;

        [SerializeField, Output] private Types.Matrix _MatrixOut;

        private Types.Matrix _MatrixOutBuffer;

        private void Reset()
        {
            name = "Matrix";
        }

        protected override void Init()
        {
            base.Init();
            if (_Matrix == null || _MatrixOut == null)
            {
                _Matrix = new Types.Matrix(_MatrixWidth, _MatrixHeight);
                _MatrixOutBuffer = _Matrix.Copy();
            }
        }

        public override object GetValue(NodePort port)
        {
            return _MatrixOutBuffer;
        }

        public void UpdateMatrix()
        {
            if (_MatrixWidth != _Matrix.width || _MatrixHeight != _Matrix.height)
            {
                _Matrix = new Types.Matrix(_MatrixWidth, _MatrixHeight);
                _MatrixOutBuffer = _Matrix.Copy();
            }
        }

        public Types.Matrix GetMatrix()
        {
            return _Matrix;
        }
    }
}
