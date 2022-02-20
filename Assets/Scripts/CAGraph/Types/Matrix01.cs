using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAGraph.Types
{
    [Serializable]
    public class Matrix01 : Matrix
    {
        [SerializeField]
        protected int[] _Cells;

        public Matrix01(int width, int height) : base(width, height)
        {
        }
        
        protected override void Reset()
        {
            _Cells = new int[width * height];

            UpdatePreview();
        }

        public new int[] GetCells()
        {
            int[] cells = new int[_Cells.Length];
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

        public void SetCells(int[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept matrix of different size (Expected size: {}, got {})",
                    width * height,
                    cells.Length
                ));

            for (int c = 0; c < cells.Length; c++)
                cells[c] = (int) Mathf.Clamp01(cells[c]);
            Array.Copy(cells, _Cells, _Cells.Length);

            UpdateID();
            UpdatePreview();
        }
        protected override void SetConvertibleCells(IConvertible[] cells)
        {
            int[] intCells = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                intCells[i] = (int) cells[i];
            SetCells(intCells);
        }

        protected override Color GetColorOf(int pixelAt)
        {
            return _Cells[pixelAt] == 0 ? Color.black : Color.white;
        }

        public new Matrix01 Clone()
        {
            Matrix01 m = new Matrix01(width, height);
            m.SetCells(_Cells);
            return m;
        }
        protected override Matrix GetClone()
        {
            return this.Clone();
        }
    }
}
