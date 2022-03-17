using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContVoronoiNode))]
    public class TileMapContVoronoiNodeEditor : BaseNodeEditor<Nodes.TileMapContVoronoiNode>
    {
        private SerializedProperty _TileMapIn, _ScaleX, _ScaleY, _OffsetX, _OffsetY, _TileMapOut, _RelativeScale;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn     = serializedObject.FindProperty("_TileMapIn");
            _ScaleX        = serializedObject.FindProperty("_ScaleX");
            _ScaleY        = serializedObject.FindProperty("_ScaleY");
            _OffsetX       = serializedObject.FindProperty("_OffsetX");
            _OffsetY       = serializedObject.FindProperty("_OffsetY");
            _TileMapOut    = serializedObject.FindProperty("_TileMapOut");

            _RelativeScale = serializedObject.FindProperty("_RelativeScale");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_ScaleX);
            graph.editorUtilities.PortFieldMinLabel(_ScaleY);
            graph.editorUtilities.PortFieldMinLabel(_OffsetX);
            graph.editorUtilities.PortFieldMinLabel(_OffsetY);
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_RelativeScale, new GUIContent("Relative Scale"));
        }
    }
}