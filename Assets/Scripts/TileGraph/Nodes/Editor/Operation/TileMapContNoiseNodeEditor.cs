using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContNoiseNode))]
    public class TileMapContNoiseNodeEditor : BaseNodeEditor<Nodes.TileMapContNoiseNode>
    {
        private SerializedProperty _TileMapIn, _NoiseScaleX, _NoiseScaleY, _OffsetX, _OffsetY, _TileMapOut, _RelativeScale;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn     = serializedObject.FindProperty("_TileMapIn");
            _NoiseScaleX   = serializedObject.FindProperty("_NoiseScaleX");
            _NoiseScaleY   = serializedObject.FindProperty("_NoiseScaleY");
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

            graph.editorUtilities.PortFieldMinLabel(_NoiseScaleX);
            graph.editorUtilities.PortFieldMinLabel(_NoiseScaleY);
            graph.editorUtilities.PortFieldMinLabel(_OffsetX);
            graph.editorUtilities.PortFieldMinLabel(_OffsetY);
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_RelativeScale, new GUIContent("Relative Scale"));
        }
    }
}