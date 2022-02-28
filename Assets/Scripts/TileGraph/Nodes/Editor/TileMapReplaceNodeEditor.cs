using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapReplaceNode))]
    public class TileMapReplaceNodeEditor : BaseNodeEditor<Nodes.TileMapReplaceNode>
    {
        private SerializedProperty _TileMapIn, _TileMapOut, _ToReplace, _Replacement;

        private ReorderableList _ToReplaceList;

        protected override void OnNodeEnable()
        {
            _TileMapIn   = serializedObject.FindProperty("_TileMapIn");
            _TileMapOut  = serializedObject.FindProperty("_TileMapOut");
            _ToReplace   = serializedObject.FindProperty("_ToReplace");
            _Replacement = serializedObject.FindProperty("_Replacement");

            _ToReplaceList = new ReorderableList(serializedObject, _ToReplace, true, true, true, true);
            _ToReplaceList.drawElementCallback = DrawListItems;
            _ToReplaceList.drawHeaderCallback = DrawHeader;

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            _ToReplaceList.DoLayoutList();            
            graph.CAEditorUtilities.PropertyFieldMinLabel(_Replacement, new GUIContent("with:"));
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
