using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixReplaceNode))]
    public class MatrixReplaceNodeEditor : BaseNodeEditor<Nodes.MatrixReplaceNode>
    {
        private SerializedProperty _MatrixIn, _MatrixOut, _ToReplace, _Replacement;

        private ReorderableList _ToReplaceList;

        protected override void OnNodeEnable()
        {
            _MatrixIn    = serializedObject.FindProperty("_MatrixIn");
            _MatrixOut   = serializedObject.FindProperty("_MatrixOut");
            _ToReplace   = serializedObject.FindProperty("_ToReplace");
            _Replacement = serializedObject.FindProperty("_Replacement");

            _ToReplaceList = new ReorderableList(serializedObject, _ToReplace, true, true, true, true);
            _ToReplaceList.drawElementCallback = DrawListItems;
            _ToReplaceList.drawHeaderCallback = DrawHeader;

            AddPreview("_MatrixOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            NodeEditorGUILayout.PropertyField(_MatrixIn);
            NodeEditorGUILayout.PropertyField(_MatrixOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            _ToReplaceList.DoLayoutList();            
            NodeEditorGUILayout.PropertyField(_Replacement, new GUIContent("with:"));
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _ToReplaceList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), element, GUIContent.none
            );
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "replace:");
        }
    }
}
