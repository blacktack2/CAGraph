using UnityEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContRandomizeNode))]
    public class TileMapContRandomizeNodeEditor : BaseNodeEditor<Nodes.TileMapContRandomizeNode>
    {
        private SerializedProperty _TileMapIn, _Seed, _TileMapOut;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _Seed       = serializedObject.FindProperty("_Seed");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_Seed);
        }

        protected override void NodeBodyGUI()
        {
            
        }
    }
}