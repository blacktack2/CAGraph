using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CAGraph.Types
{
    [Serializable]
    public class MatrixUInt : Matrix
    {
        [SerializeField]
        protected uint[] _Cells;

        public MatrixUInt(int width, int height) : base(width, height)
        {
        }
        
        protected override void Reset()
        {
            _Cells = new uint[width * height];

            UpdatePreview();
        }

        public new uint[] GetCells()
        {
            uint[] cells = new uint[_Cells.Length];
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

        public void SetCells(uint[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept matrix of different size (Expected size: {}, got {})",
                    width * height,
                    cells.Length
                ));

            Array.Copy(cells, _Cells, _Cells.Length);

            UpdateID();
            UpdatePreview();
        }
        protected override void SetConvertibleCells(IConvertible[] cells)
        {
            uint[] uintCells = new uint[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                uintCells[i] = (uint) cells[i];
            SetCells(uintCells);
        }

        protected override Color GetColorOf(int pixelAt)
        {
            uint value = _Cells[pixelAt];
            if (value == 0)
                return Color.black;
            if (value == 1)
                return Color.white;
            Random.State state = Random.state;
            Random.InitState((int) value);
            Color c = Random.ColorHSV(0f, 1f);
            Random.state = state;
            return c;
        }

        public new MatrixUInt Clone()
        {
            MatrixUInt m = new MatrixUInt(width, height);
            m.SetCells(_Cells);
            return m;
        }
        protected override Matrix GetClone()
        {
            return this.Clone();
        }
    }
}
