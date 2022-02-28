using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileGraph.Types
{
    /// <summary> 2D array of cells, stored as a flattened array for use in
    /// compute shaders. Cells are represented as unsigned integers </summary>
    [Serializable]
    public class TileMapUint : TileMap
    {
        [SerializeField]
        protected uint[] _Cells;

        public TileMapUint(int width, int height) : base(width, height)
        {
        }
        
        protected override void ResetCells()
        {
            _Cells = new uint[width * height];
        }

        /// <returns> The cells as a flattened TileMap of unsigned integers
        /// </returns>
        /// <seealso cref="base.GetCells()" />
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

        /// <summary> Set the TileMap to match <paramref name="cells" />.
        /// </summary>
        /// <seealso cref="base.SetCells(IConvertible[] cells)" />
        public void SetCells(uint[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept TileMap of different size (Expected size: {}, got {})",
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

        /// <returns> Color representation of the cell at
        /// <paramref name="pixelAt" /> (0=black, 1=white, >1=random
        /// high-saturation color). </returns>
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

        public new TileMapUint Clone()
        {
            TileMapUint m = new TileMapUint(width, height);
            m.SetCells(_Cells);
            return m;
        }
        protected override TileMap GetClone()
        {
            return this.Clone();
        }
    }
}
