using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CAEditorUtilities
{
    private static GUIStyle _PreviewToggleStyle;
    private static GUIStyle _PreviewStyle;

    private static Texture2D _NullPreview;

    public static readonly int previewWidth = 150;

    public static bool DisplayPreview(Matrix matrix, bool show)
    {
        if (_PreviewToggleStyle == null)
            _PreviewToggleStyle = new GUIStyle("Foldout");
        if (_PreviewStyle == null)
            _PreviewStyle = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
        bool showOut = EditorGUILayout.Toggle(" ", show, _PreviewToggleStyle);
        if (showOut)
        {
            if (matrix == null)
            {
                if (_NullPreview == null)
                    InitNullPreview();
                EditorGUILayout.LabelField(new GUIContent(_NullPreview), _PreviewStyle, GUILayout.Height(previewWidth));
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent(matrix.preview), _PreviewStyle, GUILayout.Height(previewWidth));
            }
        }
        return showOut;
    }

    public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = 1f / (float) targetWidth;
        float incY = 1f / (float) targetHeight;
        for (int px = 0; px < rpixels.Length; px++)
            rpixels[px] = source.GetPixelBilinear(incX * ((float) px % targetWidth), incY * ((float) Mathf.Floor(px / targetWidth)));
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    private static void InitNullPreview()
    {
        _NullPreview = new Texture2D(previewWidth, previewWidth);
        Color[] pixels = _NullPreview.GetPixels();
        for (int px = 0; px < pixels.Length; px++)
            pixels[px] = Color.magenta;
        _NullPreview.SetPixels(pixels);
        _NullPreview.Apply();
    }
}
