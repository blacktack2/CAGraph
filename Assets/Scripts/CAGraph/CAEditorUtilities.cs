using UnityEditor;
using UnityEngine;

namespace CAGraph.Utilities
{
    public class CAEditorUtilities
    {
        private Texture2D _NullPreview;
        public Texture2D nullPreview {get {return _NullPreview;}}

        public static readonly int previewWidth = 150;

        public void Enable()
        {
            InitNullPreview();
        }

        private void InitNullPreview()
        {
            _NullPreview = new Texture2D(previewWidth, previewWidth);
            Color[] pixels = _NullPreview.GetPixels();
            for (int px = 0; px < pixels.Length; px++)
                pixels[px] = Color.magenta;
            _NullPreview.SetPixels(pixels);
            _NullPreview.Apply();
        }
    }
}