using System;
using UnityEngine;

namespace TileGraph.Types
{
    /// <summary> 2D array of cells, stored as a flattened array for use in
    /// compute shaders. Cells are represented as integers with 0 and 1 as the
    /// only accepted value. Integers are used instead of booleans to allow
    /// transfer of the array to a compute shader. </summary>
    [Serializable]
    public class TileMapBool : TileMap
    {
        [SerializeField]
        protected int[] _Cells;

        public TileMapBool(int width, int height) : base(width, height)
        {
        }

        protected override void ResetCells()
        {
            _Cells = new int[width * height];
        }

        /// <returns> The cells as a flattened TileMap of integers (values 0-1).
        /// </returns>
        /// <seealso cref="base.GetCells()" />
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

        /// <summary> Set the TileMap to match <paramref name="cells" /> with
        /// values clamped to 0 and 1, and reset the TileMap ID. </summary>
        /// <seealso cref="base.SetCells(IConvertible[] cells)" />
        public void SetCells(int[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept TileMap of different size (Expected size: {}, got {})",
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

        /// <returns> Black/white representation of the cell at
        /// <paramref name="pixelAt" /> (0=black, 1=white). </returns>
        protected override Color GetColorOf(int pixelAt)
        {
            return _Cells[pixelAt] == 0 ? Color.black : Color.white;
        }

        public new TileMapBool Clone()
        {
            TileMapBool m = new TileMapBool(width, height);
            m.SetCells(_Cells);
            return m;
        }
        protected override TileMap GetClone()
        {
            return this.Clone();
        }
    }
}
