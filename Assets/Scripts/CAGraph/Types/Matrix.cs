using System;
using UnityEngine;

namespace CAGraph.Types
{
    [Serializable]
    public abstract class Matrix
    {
        public const int maxMatrixSize = 100;

        [SerializeField, HideInInspector, Range(2, maxMatrixSize)]
        public int width, height;

        private long _ID;

        private Texture2D _Preview;
        public Texture2D preview {get {return _Preview;}}

        public Matrix(int width, int height)
        {
            this.width = width;
            this.height = height;

            Reset();
        }

        protected abstract void Reset();

        public void UpdatePreview()
        {
            Texture2D preview = new Texture2D(width, height);
            Color[] pixels = preview.GetPixels(0);

            for (int px = 0; px < pixels.Length; px++)
                pixels[px] = GetColorOf(px);

            preview.SetPixels(pixels);
            preview.Apply();
            // float scale = Mathf.Min((float)Utilities.CAEditorUtilities.previewWidth / (float)width,
            //                         (float)Utilities.CAEditorUtilities.previewWidth / (float)height);
            // int targetWidth = (int) (width * scale);
            // int targetHeight = (int) (height * scale);
            _Preview = ScaleTexture(preview, Utilities.CAEditorUtilities.previewWidth, Utilities.CAEditorUtilities.previewWidth);
        }

        private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = 1f / (float) targetWidth;
            float incY = 1f / (float) targetHeight;
            for (int px = 0; px < rpixels.Length; px++)
                rpixels[px] = source.GetPixelBilinear(
                    incX * ((float) px % targetWidth), incY * ((float) Mathf.Floor(px / targetWidth)));
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }

        public IConvertible[] GetCells()
        {
            return GetConvertibleCells();
        }
        protected abstract IConvertible[] GetConvertibleCells();
        public void SetCells(IConvertible[] cells)
        {
            SetConvertibleCells(cells);
        }
        protected abstract void SetConvertibleCells(IConvertible[] cells);

        public void UpdateID()
        {
            _ID = DateTime.Now.Ticks;
        }

        protected abstract Color GetColorOf(int pixelAt);

        public Matrix Clone()
        {
            return GetClone();
        }
        protected abstract Matrix GetClone();

        public long GetID()
        {
            return _ID;
        }
    }
}
