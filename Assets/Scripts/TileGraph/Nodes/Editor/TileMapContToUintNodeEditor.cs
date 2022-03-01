using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContToUintNode))]
    public class TileMapContToUintNodeEditor : BaseNodeEditor<Nodes.TileMapContToUintNode>
    {
        private SerializedProperty _TileMapIn, _Max, _TileMapOut;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _Max        = serializedObject.FindProperty("_Max");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();
            
            graph.CAEditorUtilities.PortFieldMinLabel(_Max);
        }

        protected override void NodeBodyGUI()
        {
        }
    }
}
