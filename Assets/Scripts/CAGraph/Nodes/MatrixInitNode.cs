using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class MatrixInitNode : Node
{
    [SerializeField]
    private Matrix _Matrix;
    [SerializeField, Range(2, 200)]
    private int _MatrixWidth = 100, _MatrixHeight = 100;

    [SerializeField, Output] private Matrix _MatrixOut;

    protected override void Init()
    {
        base.Init();
        if (_Matrix == null)
            _Matrix = new Matrix(_MatrixWidth, _MatrixHeight);
    }

    public override object GetValue(NodePort port)
    {
        return _Matrix.Copy();
    }

    public void UpdateMatrix()
    {
        if (_MatrixWidth != _Matrix.width || _MatrixHeight != _Matrix.height)
        {
            _Matrix.width = _MatrixWidth;
            _Matrix.height = _MatrixHeight;
            _Matrix.Reset();
        }
    }

    public Matrix GetMatrix()
    {
        return _Matrix;
    }
}
