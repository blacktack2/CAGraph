using UnityEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapRandomizeNode))]
    public class TileMapRandomizeNodeEditor : BaseNodeEditor<Nodes.TileMapRandomizeNode>
    {
        private SerializedProperty _TileMapIn, _Seed, _Chance, _TileMapOut;

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

            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_Seed);

            graph.CAEditorUtilities.PortFieldMinLabel(_Chance);
        }

        protected override void NodeBodyGUI()
        {
            
        }
    }
}