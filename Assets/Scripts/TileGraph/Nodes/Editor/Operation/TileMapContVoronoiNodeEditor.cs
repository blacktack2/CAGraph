using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContVoronoiNode))]
    public class TileMapContVoronoiNodeEditor : BaseNodeEditor<Nodes.TileMapContVoronoiNode>
    {
        private SerializedProperty _TileMapIn, _Frequency, _Offset, _TileMapOut, _RelativeFrequency, _Advanced, _Threshold;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn         = serializedObject.FindProperty("_TileMapIn");
            _Frequency         = serializedObject.FindProperty("_Frequency");
            _Offset            = serializedObject.FindProperty("_Offset");
            _TileMapOut        = serializedObject.FindProperty("_TileMapOut");

            _RelativeFrequency = serializedObject.FindProperty("_RelativeFrequency");
            _Advanced          = serializedObject.FindProperty("_Advanced");
            _Threshold         = serializedObject.FindProperty("_Threshold");


            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_Frequency);
            graph.editorUtilities.PortFieldMinLabel(_Offset);
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_RelativeFrequency, new GUIContent("Relative Scale"));
            graph.editorUtilities.PropertyFieldMinLabel(_Advanced, new GUIContent("Advanced"));
            serializedObject.ApplyModifiedProperties();
            if (_Advanced.boolValue)
                graph.editorUtilities.PropertyFieldMinLabel(_Threshold, new GUIContent("Threshold"));
        }
    }
}