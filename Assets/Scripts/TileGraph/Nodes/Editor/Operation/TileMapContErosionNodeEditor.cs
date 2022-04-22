using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContErosionNode))]
    public class TileMapContErosionNodeEditor : BaseNodeEditor<Nodes.TileMapContErosionNode>
    {
        private SerializedProperty _TileMapIn, _TileMapOut, _Iterations, _Algorithm, _TerrainHardness, _SedimentHardness, _DepositionRate, _RainRate, _RainAmount;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");
            
            _Iterations = serializedObject.FindProperty("_Iterations");
            _Algorithm  = serializedObject.FindProperty("_Algorithm");
            
            _TerrainHardness  = serializedObject.FindProperty("_TerrainHardness");
            _SedimentHardness = serializedObject.FindProperty("_SedimentHardness");
            _DepositionRate   = serializedObject.FindProperty("_DepositionRate");
            _RainRate         = serializedObject.FindProperty("_RainRate");
            _RainAmount       = serializedObject.FindProperty("_RainAmount");

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
            graph.editorUtilities.PropertyFieldMinLabel(_Iterations);
            graph.editorUtilities.PropertyFieldMinLabel(_Algorithm);

            graph.editorUtilities.PropertyFieldMinLabel(_TerrainHardness);
            graph.editorUtilities.PropertyFieldMinLabel(_SedimentHardness);
            graph.editorUtilities.PropertyFieldMinLabel(_DepositionRate);
            graph.editorUtilities.PropertyFieldMinLabel(_RainRate);
            graph.editorUtilities.PropertyFieldMinLabel(_RainAmount);
        }
    }
}
