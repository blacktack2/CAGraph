using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapUintToBoolNode))]
    public class TileMapUintToBoolNodeEditor : BaseNodeEditor<Nodes.TileMapUintToBoolNode>
    {
        private SerializedProperty _TileMapIn, _Threshold, _TileMapOut;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _Threshold  = serializedObject.FindProperty("_Threshold");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_Threshold);
        }

        protected override void NodeBodyGUI()
        {
        }
    }
}
