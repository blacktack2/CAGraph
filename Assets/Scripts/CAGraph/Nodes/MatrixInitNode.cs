using System;
using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Input/Matrix", 0)]
    public class MatrixInitNode : BaseNode
    {
        [SerializeField, Output] private Types.Matrix01 _Matrix01Out;
        [SerializeField, Output] private Types.MatrixContinuous _MatrixContinuousOut;
        [SerializeField, Output] private Types.MatrixUInt _MatrixUIntOut;

        [SerializeField, Range(2, Types.Matrix01.maxMatrixSize)]
        private int _MatrixWidth = 100, _MatrixHeight = 100;

        public enum MatrixType { Boolean, Continuous, Integer }
        [SerializeField]
        private MatrixType _MatrixType = MatrixType.Boolean;

        private Types.Matrix _MatrixBuffer;

        // private Types.Matrix01 _Matrix01OutBuffer;
        // private Types.MatrixContinuous _MatrixContinuousOutBuffer;
        // private Types.MatrixUInt _MatrixUIntOutBuffer;

        private void Reset()
        {
            name = "Matrix";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_Matrix01Out" && _MatrixType == MatrixType.Boolean)
            {
                UpdateMatrix();
                return (Types.Matrix01) _MatrixBuffer;
            }
            else if (port.fieldName == "_MatrixContinuousOut" && _MatrixType == MatrixType.Continuous)
            {
                UpdateMatrix();
                return (Types.MatrixContinuous) _MatrixBuffer;
            }
            else if (port.fieldName == "_MatrixUIntOut" && _MatrixType == MatrixType.Integer)
            {
                UpdateMatrix();
                return (Types.MatrixUInt) _MatrixBuffer;
            }
            return null;
        }

        public void UpdateMatrix()
        {
            // switch (_MatrixType)
            // {
            //     case MatrixType.Boolean:
            //         if (MatrixChanged(_Matrix01OutBuffer))
            //             _Matrix01OutBuffer = new Types.Matrix01(_MatrixWidth, _MatrixHeight);
            //         break;
            //     case MatrixType.Continuous:
            //         if (MatrixChanged(_MatrixContinuousOutBuffer))
            //             _MatrixContinuousOutBuffer = new Types.MatrixContinuous(_MatrixWidth, _MatrixHeight);
            //         break;
            //     case MatrixType.Integer:
            //         if (MatrixChanged(_MatrixUIntOutBuffer))
            //             _MatrixUIntOutBuffer = new Types.MatrixUInt(_MatrixWidth, _MatrixHeight);
            //         break;
            // }
            switch (_MatrixType)
            {
                case MatrixType.Boolean:
                    if (MatrixChanged<Types.Matrix01>(_MatrixBuffer))
                        _MatrixBuffer = new Types.Matrix01(_MatrixWidth, _MatrixHeight);
                    break;
                case MatrixType.Continuous:
                    if (MatrixChanged<Types.MatrixContinuous>(_MatrixBuffer))
                        _MatrixBuffer = new Types.MatrixContinuous(_MatrixWidth, _MatrixHeight);
                    break;
                case MatrixType.Integer:
                    if (MatrixChanged<Types.MatrixUInt>(_MatrixBuffer))
                        _MatrixBuffer = new Types.MatrixUInt(_MatrixWidth, _MatrixHeight);
                    break;
            }
        }

        private bool MatrixChanged<T>(Types.Matrix matrix) where T : Types.Matrix
        {
            return matrix == null || !(matrix is T) || _MatrixWidth != matrix.width || _MatrixHeight != matrix.height;
        }
    }
}
