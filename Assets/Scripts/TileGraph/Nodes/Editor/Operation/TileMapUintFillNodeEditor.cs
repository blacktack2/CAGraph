using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapUintFillNode))]
    public class TileMapUintFillNodeEditor : BaseNodeEditor<Nodes.TileMapUintFillNode>
    {
        private SerializedProperty _TileMapIn, _FillValue, _TileMapOut;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _FillValue  = serializedObject.FindProperty("_FillValue");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_FillValue, new GUIContent("fill value"));
        }

        protected override void NodeBodyGUI()
        {
        }
    }
}