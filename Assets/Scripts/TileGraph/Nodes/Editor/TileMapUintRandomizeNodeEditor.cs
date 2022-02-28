using UnityEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapUintRandomizeNode))]
    public class TileMapUintRandomizeNodeEditor : BaseNodeEditor<Nodes.TileMapUintRandomizeNode>
    {
        private SerializedProperty _TileMapIn, _Seed, _Max, _TileMapOut;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _Seed       = serializedObject.FindProperty("_Seed");
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

            graph.CAEditorUtilities.PortFieldMinLabel(_Seed);

            graph.CAEditorUtilities.PortFieldMinLabel(_Max);
        }

        protected override void NodeBodyGUI()
        {
            
        }
    }
}