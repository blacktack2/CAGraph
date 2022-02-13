using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Input/Matrix", 0)]
public class MatrixInitNode : Node
{
    [SerializeField]
    private Matrix _Matrix;
    [SerializeField, Range(2, 200)]
    private int _MatrixWidth = 100, _MatrixHeight = 100;

    [SerializeField, Output] private Matrix _MatrixOut;

    private Matrix _MatrixOutBuffer;

    private void Reset()
    {
        name = "Matrix";
    }

    protected override void Init()
    {
        base.Init();
        if (_Matrix == null || _MatrixOut == null)
        {
            _Matrix = new Matrix(_MatrixWidth, _MatrixHeight);
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
            _Matrix = new Matrix(_MatrixWidth, _MatrixHeight);
            _MatrixOutBuffer = _Matrix.Copy();
        }
    }

    public Matrix GetMatrix()
    {
        return _Matrix;
    }
}
