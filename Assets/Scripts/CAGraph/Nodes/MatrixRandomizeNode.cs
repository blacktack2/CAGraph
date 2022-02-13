using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class MatrixRandomizeNode : Node
{
    [SerializeField]
    private int _Seed = 0;
    [SerializeField, Range(0f, 1f)]
    private float _Chance = 0.5f;
    
    private int _CurrentSeed = 1;
    private float _CurrentChance = 0f;

    [SerializeField, Input] private Matrix _MatrixIn;
    [SerializeField, Output] private Matrix _MatrixOut;

    private long _MatrixInIDBuffer = 0L;
    private Matrix _MatrixOutBuffer;

    private void Reset()
    {
        name = "Randomize Matrix";
    }

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "_MatrixOut")
            return GetRandomizedMatrix();
        return null;
    }

    private Matrix GetRandomizedMatrix()
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
            Randomize();
        }
        else if (_CurrentSeed != _Seed || _CurrentChance != _Chance)
        {
            Randomize();
        }
        return _MatrixOutBuffer;
    }

    private void Randomize()
    {
        _CurrentSeed = _Seed;
        _CurrentChance = _Chance;
        MatrixOperations.RandomizeMatrix(_MatrixOutBuffer, _Chance, _Seed);
    }

    public void SetSeed(int seed)
    {
        _Seed = seed;
    }
}
