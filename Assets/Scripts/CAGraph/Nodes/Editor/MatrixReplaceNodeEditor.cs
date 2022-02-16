using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixReplaceNode))]
    public class MatrixReplaceNodeEditor : NodeEditor
    {
        private Nodes.MatrixReplaceNode _MatrixReplaceNode;

        private ReorderableList _ToReplaceList;

        private bool _ShowPreview = true;

        public override void OnBodyGUI()
        {
            if (_MatrixReplaceNode == null)
                _MatrixReplaceNode = target as Nodes.MatrixReplaceNode;
            if (_ToReplaceList == null)
            {
                _ToReplaceList = new ReorderableList(serializedObject, serializedObject.FindProperty("_ToReplace"), true, true, true, true);
                _ToReplaceList.drawElementCallback = DrawListItems;
                _ToReplaceList.drawHeaderCallback = DrawHeader;
            }
            
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixIn"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

            EditorGUILayout.EndHorizontal();

            _ToReplaceList.DoLayoutList();            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_Replacement"), new GUIContent("with:"));
            
            serializedObject.ApplyModifiedProperties();

            _ShowPreview = Utilities.CAEditorUtilities.DisplayPreview(
                (Types.Matrix) _MatrixReplaceNode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _ToReplaceList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), element, GUIContent.none
            );
            _MatrixReplaceNode.toReplaceChanged |= EditorGUI.EndChangeCheck();
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "replace:");
        }
    }
}
