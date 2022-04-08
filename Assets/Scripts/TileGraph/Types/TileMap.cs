using System;
using UnityEngine;

namespace TileGraph.Types
{
    /// <summary> 2D array of cells, stored as a flattened array for use in
    /// compute shaders. Cells can be represented by any numeric value
    /// determined by implementation </summary>
    [Serializable]
    public abstract class TileMap
    {
        /// <summary> Maximum width/height of the TileMap (the max number of
        /// total elements will be <paramref name="maxTileMapSize" /> *
        /// <paramref name="maxTileMapSize" />) </summary>
        public const int maxTileMapSize = 150;

        [SerializeField, HideInInspector, Range(2, maxTileMapSize)]
        public int width, height;

        private long _ID;

        private Texture2D _Preview;
        /// <summary> Image representation of this TileMap. </summary>
        public Texture2D preview {get {return _Preview;}}

        public TileMap(int width, int height)
        {
            this.width = width;
            this.height = height;

            Reset();
            UpdateID();
        }

        /// <summary> Re-initialize the TileMap and call for a preview update.
        /// </summary>
        protected void Reset()
        {
            ResetCells();
            UpdatePreview();
        }

        /// <summary> Re-initialize the TileMap. </summary>
        protected abstract void ResetCells();

        /// <summary> Re-generate the preview image based on the current values
        /// of the TileMap. </summary>
        public void UpdatePreview()
        {
            Texture2D preview = new Texture2D(width, height);
            Color[] pixels = preview.GetPixels(0);

            for (int px = 0; px < pixels.Length; px++)
                pixels[px] = GetColorOf(px);

            preview.SetPixels(pixels);
            preview.Apply();
            _Preview = ScaleTexture(preview, Utilities.EditorUtilities.previewWidth);
        }

        /// <returns> A new texture 2D, the same as <paramref name="source" />
        /// but resized to <paramref name="targetSize" /> by
        /// <paramref name="targetSize" /> pixels </returns>
        /// <remarks> Should probably be moved to a Utility class. </remarks>
        private Texture2D ScaleTexture(Texture2D source, int targetSize)
        {
            Texture2D result = new Texture2D(targetSize, targetSize, source.format, true);
            Color[] rpixels = result.GetPixels(0);

            for (int px = 0; px < rpixels.Length; px++)
                rpixels[px] = source.GetPixel(
                    Mathf.RoundToInt((px % targetSize) * source.width / targetSize), Mathf.RoundToInt((px / targetSize) * source.height / targetSize));

            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }

        /// <returns> The cells as a flattened TileMap. </returns>
        /// <remarks> Overwritten by child implementations for type
        /// contravariance </remarks>
        public IConvertible[] GetCells()
        {
            return GetConvertibleCells();
        }
        /// <returns> The cells as a flattened TileMap cast to <c>IConvertible</c>. </returns>
        protected abstract IConvertible[] GetConvertibleCells();

        /// <returns> The cell at the 2D position
        /// <paramref name="x" />,<paramref name="y" />. </returns>
        /// <remarks> Overwritten by child implementations for type
        /// contravariance </remarks>
        public IConvertible GetCellAt(int x, int y)
        {
            return GetConvertibleCellAt(x, y);
        }
        /// <returns> The cell at the 2D position
        /// <paramref name="x" />,<paramref name="y" /> cast to
        /// <c>IConvertible</c>. </returns>
        protected abstract IConvertible GetConvertibleCellAt(int x, int y);

        /// <summary> Set the TileMap to match <paramref name="cells" /> casted
        /// to this implementations data type, and reset the TileMap ID.
        /// </summary>
        /// <exception cref="FormatException"> If the size of
        /// <paramref name="cells" /> does not match the size of this TileMap.
        /// </exception>
        /// <remarks> Overloaded by child implementations for type
        /// contravariance </remarks>
        public void SetCells(IConvertible[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept TileMap of different size (Expected size: {}, got {})",
                    width * height,
                    cells.Length
                ));
            SetConvertibleCells(cells);
        }
        /// <summary> Cast the values of <paramref name="cells" /> to an
        /// appropriate type and set the TileMap to match it. </summary>
        protected abstract void SetConvertibleCells(IConvertible[] cells);

        /// <summary> Set the ID of this TileMap to an arbitrary new value.
        /// </summary>
        public void UpdateID()
        {
            _ID = (long) (Time.time * 1000f);
        }

        /// <returns> Color representation of the value in the flattened TileMap
        /// at <paramref name="pixelAt" />. </returns>
        protected abstract Color GetColorOf(int pixelAt);

        /// <returns> Deep copy of this TileMap with a different ID. </returns>
        /// <remarks> Overwritten by child implementations for type
        /// contravariance </remarks>
        public TileMap Clone()
        {
            return GetClone();
        }
        /// <returns> Deep copy of this TileMap with a different ID. </returns>
        protected abstract TileMap GetClone();

        public long GetID()
        {
            return _ID;
        }
    }
}
