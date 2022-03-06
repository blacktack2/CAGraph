using System;
using UnityEngine;

namespace TileGraph.Types
{
    /// <summary> 2D array of cells, stored as a flattened array for use in
    /// compute shaders. Cells are represented as floats between 0 and 1.
    /// (inclusive) </summary>
    [Serializable]
    public class TileMapCont : TileMap
    {
        [SerializeField]
        protected float[] _Cells;

        public TileMapCont(int width, int height) : base(width, height)
        {
        }
        
        protected override void ResetCells()
        {
            _Cells = new float[width * height];
        }

        /// <returns> The cells as a flattened TileMap of floats (values 0-1).
        /// </returns>
        /// <seealso cref="base.GetCells()" />
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

        /// <returns> The cell at the 2D position
        /// <paramref name="x" />,<paramref name="y" /> as a float between 0-1.
        /// </returns>
        public new float GetCellAt(int x, int y)
        {
            return _Cells[x + y * width];
        }
        protected override IConvertible GetConvertibleCellAt(int x, int y)
        {
            return (IConvertible) GetCellAt(x, y);
        }

        /// <summary> Set the TileMap to match <paramref name="cells" /> with
        /// values clamped to 0 and 1, and reset the TileMap ID. </summary>
        /// <seealso cref="base.SetCells(IConvertible[] cells)" />
        public void SetCells(float[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept TileMap of different size (Expected size: {}, got {})",
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

        /// <returns> Grayscale representation of the cell at
        /// <paramref name="pixelAt" /> (0=black, 1=white). </returns>
        protected override Color GetColorOf(int pixelAt)
        {
            return new Color(_Cells[pixelAt], _Cells[pixelAt], _Cells[pixelAt]);
        }

        public new TileMapCont Clone()
        {
            TileMapCont m = new TileMapCont(width, height);
            m.SetCells(_Cells);
            return m;
        }
        protected override TileMap GetClone()
        {
            return this.Clone();
        }
    }
}
