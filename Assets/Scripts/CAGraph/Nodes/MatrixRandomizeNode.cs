using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class MatrixRandomizeNode : Node
{
    [SerializeField]
    private int _Seed = 0;
    [SerializeField]
    private float _Chance = 0.5f;

    [SerializeField, Input] private Matrix _MatrixIn;
    [SerializeField, Output] private Matrix _MatrixOut;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "_MatrixOut")
            return RandomizeMatrix(GetInputValue<Matrix>("_MatrixIn"));
        return null;
    }

    private Matrix RandomizeMatrix(Matrix matrix)
    {
        if (matrix == null)
            return null;
        MatrixOperations.RandomizeMatrix(matrix, _Chance, _Seed);
        return matrix;
    }
}
