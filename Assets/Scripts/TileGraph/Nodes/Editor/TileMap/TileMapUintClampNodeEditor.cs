using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapUintClampNode))]
    public class TileMapUintClampNodeEditor : BaseNodeEditor<Nodes.TileMapUintClampNode>
    {
        private SerializedProperty _TileMapIn, _TileMapOut, _Min, _Max;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");

            _Min = serializedObject.FindProperty("_Min");
            _Max = serializedObject.FindProperty("_Max");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_Min);
            graph.editorUtilities.PropertyFieldMinLabel(_Max);
        }
    }
}
