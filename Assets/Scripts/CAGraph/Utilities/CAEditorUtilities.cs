using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Utilities
{
    /// <summary> Utility class for general Node Editor operations. </summary>
    public class CAEditorUtilities
    {
        private Texture2D _NullPreview;
        /// <summary> Default preview to show if no other image can be
        /// obtained/generated </summary>
        public Texture2D nullPreview {get {return _NullPreview;}}

        /// <summary> Image size of all node previews. </summary>
        public static readonly int previewWidth = 150;

        public void Enable()
        {
            InitNullPreview();
        }

        /// <summary> Create a <c>NodeEditorGUILayout.PropertyField</c> with
        /// the label width set to match the size of the label text </summary>
        /// <seealso cref="NodeEditorGUILayout.PropertyField(SerializedProperty port, GUIContent label)" />
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

        /// <summary> Create a <c>EditorGUILayout.PropertyField</c> with
        /// the label width set to match the size of the label text </summary>
        /// <seealso cref="EditorGUILayout.PropertyField(SerializedProperty port, GUIContent label)" />
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

        /// <summary> Set label width to match the width of the text in
        /// <paramref name="label" /> </summary>
        public void SetLabelWidthToText(string label)
        {
            SetLabelWidthToText(new GUIContent(label));
        }
        /// <summary> Set label width to match the width of the text in
        /// <paramref name="label" /> </summary>
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