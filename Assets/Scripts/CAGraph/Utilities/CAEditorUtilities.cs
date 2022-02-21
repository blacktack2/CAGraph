using UnityEditor;
using UnityEngine;
using XNodeEditor;

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

        public void PortFieldMinLabel(SerializedProperty port, GUIContent label = null, bool includeChildren = true, params GUILayoutOption[] options)
        {
            if (label == null)
            {
                EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent(port.name)).x;
                NodeEditorGUILayout.PropertyField(port, includeChildren, options);
            }
            else
            {
                EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(label).x;
                NodeEditorGUILayout.PropertyField(port, label, includeChildren, options);
            }
            EditorGUIUtility.labelWidth = 0;
        }

        public void PropertyFieldMinLabel(SerializedProperty property, GUIContent label = null, bool includeChildren = true, params GUILayoutOption[] options)
        {
            if (label == null)
            {
                EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent(property.name)).x;
                EditorGUILayout.PropertyField(property, includeChildren, options);
            }
            else
            {
                EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(label).x;
                EditorGUILayout.PropertyField(property, label, includeChildren, options);
            }
            EditorGUIUtility.labelWidth = 0;
        }

        public void SetLabelWidthToText(string label)
        {
            SetLabelWidthToText(new GUIContent(label));
        }

        public void SetLabelWidthToText(GUIContent label)
        {
            EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(label).x;
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