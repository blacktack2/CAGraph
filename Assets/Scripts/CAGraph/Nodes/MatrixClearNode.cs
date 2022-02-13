using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Operations/Matrix/Clear", 10)]
public class MatrixClearNode : Node
{
    [SerializeField, Input] private Matrix _MatrixIn;
    [SerializeField, Output] private Matrix _MatrixOut;

    private long _MatrixInIDBuffer = 0L;
    private Matrix _MatrixOutBuffer;

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

    private Matrix GetClearedMatrix()
    {
        Matrix matrix = GetInputValue<Matrix>("_MatrixIn");
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
            MatrixOperations.ClearMatrix(_MatrixOutBuffer);
        }
        return _MatrixOutBuffer;
    }
}
