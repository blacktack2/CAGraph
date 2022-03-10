using UnityEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapBoolRandomizeNode))]
    public class TileMapBoolRandomizeNodeEditor : BaseNodeEditor<Nodes.TileMapBoolRandomizeNode>
    {
        private SerializedProperty _TileMapIn, _Seed, _Chance, _TileMapOut;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _Seed       = serializedObject.FindProperty("_Seed");
            _Chance     = serializedObject.FindProperty("_Chance");
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

            graph.editorUtilities.PortFieldMinLabel(_Chance);
        }

        protected override void NodeBodyGUI()
        {
            
        }
    }
}