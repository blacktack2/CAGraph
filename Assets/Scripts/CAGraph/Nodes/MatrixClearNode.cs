using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class MatrixClearNode : Node
{
    [SerializeField, Input] private Matrix _MatrixIn;
    [SerializeField, Output] private Matrix _MatrixOut;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "_MatrixOut")
            return ClearMatrix(GetInputValue<Matrix>("_MatrixIn"));
        return null;
    }

    private Matrix ClearMatrix(Matrix matrix)
    {
        if (matrix == null)
            return null;
        MatrixOperations.ClearMatrix(matrix);
        return matrix;
    }
}
