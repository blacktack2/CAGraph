using System;
using UnityEngine;

namespace CAGraph.Types
{
    /// <summary> 2D array of cells, stored as a flattened array for use in
    /// compute shaders. Cells can be represented by any numeric value
    /// determined by implementation </summary>
    [Serializable]
    public abstract class Matrix
    {
        /// <summary> Maximum width/height of the matrix (the max number of
        /// total elements will be <paramref name="maxMatrixSize" /> *
        /// <paramref name="maxMatrixSize" />) </summary>
        public const int maxMatrixSize = 100;

        [SerializeField, HideInInspector, Range(2, maxMatrixSize)]
        public int width, height;

        private long _ID;

        private Texture2D _Preview;
        /// <summary> Image representation of this matrix. </summary>
        public Texture2D preview {get {return _Preview;}}

        public Matrix(int width, int height)
        {
            this.width = width;
            this.height = height;

            Reset();
        }

        /// <summary> Re-initialize the matrix and call for a preview update.
        /// </summary>
        protected void Reset()
        {
            ResetCells();
            UpdatePreview();
        }

        /// <summary> Re-initialize the matrix. </summary>
        protected abstract void ResetCells();

        /// <summary> Re-generate the preview image based on the current values
        /// of the matrix. </summary>
        public void UpdatePreview()
        {
            Texture2D preview = new Texture2D(width, height);
            Color[] pixels = preview.GetPixels(0);

            for (int px = 0; px < pixels.Length; px++)
                pixels[px] = GetColorOf(px);

            preview.SetPixels(pixels);
            preview.Apply();
            _Preview = ScaleTexture(preview, Utilities.CAEditorUtilities.previewWidth);
        }

        /// <returns> A new texture 2D, the same as <paramref name="source" />
        /// but resized to <paramref name="targetSize" /> by
        /// <paramref name="targetSize" /> pixels </returns>
        /// <remarks> Should probably be moved to a Utility class. </remarks>
        private Texture2D ScaleTexture(Texture2D source, int targetSize)
        {
            Texture2D result = new Texture2D(targetSize, targetSize, source.format, true);
            Color[] rpixels = result.GetPixels(0);

            float inc = 1f / (float) targetSize;
            for (int px = 0; px < rpixels.Length; px++)
                rpixels[px] = source.GetPixelBilinear(
                    inc * ((float) px % targetSize), inc * ((float) Mathf.Floor(px / targetSize)));

            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }

        /// <returns> The cells as a flattened matrix. </returns>
        /// <remarks> Overwritten by child implementations for type
        /// contravariance </remarks>
        public IConvertible[] GetCells()
        {
            return GetConvertibleCells();
        }
        /// <returns> The cells as a flattened matrix cast to <c>IConvertible</c>. </returns>
        protected abstract IConvertible[] GetConvertibleCells();

        /// <summary> Set the matrix to match <paramref name="cells" /> casted
        /// to this implementations data type, and reset the matrix ID.
        /// </summary>
        /// <exception cref="FormatException"> If the size of
        /// <paramref name="cells" /> does not match the size of this matrix.
        /// </exception>
        /// <remarks> Overloaded by child implementations for type
        /// contravariance </remarks>
        public void SetCells(IConvertible[] cells)
        {
            if (cells.Length != width * height)
                throw new FormatException(string.Format(
                    "Cannot accept matrix of different size (Expected size: {}, got {})",
                    width * height,
                    cells.Length
                ));
            SetConvertibleCells(cells);
        }
        /// <summary> Cast the values of <paramref name="cells" /> to an
        /// appropriate type and set the matrix to match it. </summary>
        protected abstract void SetConvertibleCells(IConvertible[] cells);

        /// <summary> Set the ID of this matrix to an arbitrary new value.
        /// </summary>
        public void UpdateID()
        {
            _ID = DateTime.Now.Ticks;
        }

        /// <returns> Color representation of the value in the flattened matrix
        /// at <paramref name="pixelAt" />. </returns>
        protected abstract Color GetColorOf(int pixelAt);

        /// <returns> Deep copy of this matrix with a different ID. </returns>
        /// <remarks> Overwritten by child implementations for type
        /// contravariance </remarks>
        public Matrix Clone()
        {
            return GetClone();
        }
        /// <returns> Deep copy of this matrix with a different ID. </returns>
        protected abstract Matrix GetClone();

        public long GetID()
        {
            return _ID;
        }
    }
}
