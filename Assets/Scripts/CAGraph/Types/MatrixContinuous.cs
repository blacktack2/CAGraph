using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAGraph.Types
{
    [Serializable]
    public class MatrixContinuous : Matrix
    {
        [SerializeField]
        protected float[] _Cells;

        public MatrixContinuous(int width, int height) : base(width, height)
        {
        }
        
        protected override void Reset()
        {
            _Cells = new float[width * height];

            UpdatePreview();
        }

        public new float[] GetCells()
        {
            float[] cells = new float[_Cells.Length];
            Array.Copy(_Cells, cells, _Cells.Length);
            return cells;
        }
        protected override IConvertible[] GetConvertibleCells()
        {
            IConvertible[] conCells = new IConvertible[_Cells.Length];
            for (int i = 0; i < _Cells.Length; i++)
                conCells[i] = (IConvertible) _Cells[i];
            return conCells;
        }

        public void SetCells(float[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept matrix of different size (Expected size: {}, got {})",
                    width * height,
                    cells.Length
                ));

            for (int c = 0; c < cells.Length; c++)
                cells[c] = Mathf.Clamp01(cells[c]);
            Array.Copy(cells, _Cells, _Cells.Length);

            UpdateID();
            UpdatePreview();
        }
        protected override void SetConvertibleCells(IConvertible[] cells)
        {
            float[] floatCells = new float[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                floatCells[i] = (float) cells[i];
            SetCells(floatCells);
        }

        protected override Color GetColorOf(int pixelAt)
        {
            return new Color(_Cells[pixelAt], _Cells[pixelAt], _Cells[pixelAt]);
        }

        public new MatrixContinuous Clone()
        {
            MatrixContinuous m = new MatrixContinuous(width, height);
            m.SetCells(_Cells);
            return m;
        }
        protected override Matrix GetClone()
        {
            return this.Clone();
        }
    }
}
