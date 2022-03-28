using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContErosionNode))]
    public class TileMapContErosionNodeEditor : BaseNodeEditor<Nodes.TileMapContErosionNode>
    {
        private SerializedProperty _ThermalErosionTimeScale, _ThermalErosionRate,
            _TalusAngleTangentCoeff, _TalusAngleTangentBias,
            _TileMapIn, _TileMapOut,
            _SedimentCapacity, _MaxErosionDepth,
            _SuspensionRate, _DepositionRate, _SedimentSofteningRate,
            _DeltaTime, _PipeArea, _PipeLength,
            _Evaporation, _RainRate,
            _CellScale, _Iterations;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn      = serializedObject.FindProperty("_TileMapIn");
            _TileMapOut     = serializedObject.FindProperty("_TileMapOut");

            _ThermalErosionTimeScale = serializedObject.FindProperty("_ThermalErosionTimeScale");
            _ThermalErosionRate = serializedObject.FindProperty("_ThermalErosionRate");
            _TalusAngleTangentCoeff = serializedObject.FindProperty("_TalusAngleTangentCoeff");
            _TalusAngleTangentBias = serializedObject.FindProperty("_TalusAngleTangentBias");

            _SedimentCapacity = serializedObject.FindProperty("_SedimentCapacity");
            _MaxErosionDepth = serializedObject.FindProperty("_MaxErosionDepth");
            _SuspensionRate = serializedObject.FindProperty("_SuspensionRate");
            _DepositionRate = serializedObject.FindProperty("_DepositionRate");
            _SedimentSofteningRate = serializedObject.FindProperty("_SedimentSofteningRate");
            
            _DeltaTime = serializedObject.FindProperty("_DeltaTime");
            _PipeArea = serializedObject.FindProperty("_PipeArea");
            _PipeLength = serializedObject.FindProperty("_PipeLength");
            _Evaporation = serializedObject.FindProperty("_Evaporation");
            _RainRate = serializedObject.FindProperty("_RainRate");
            _CellScale = serializedObject.FindProperty("_CellScale");
            _Iterations     = serializedObject.FindProperty("_Iterations");

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
            graph.editorUtilities.PropertyFieldMinLabel(_ThermalErosionTimeScale);
            graph.editorUtilities.PropertyFieldMinLabel(_ThermalErosionRate);
            graph.editorUtilities.PropertyFieldMinLabel(_TalusAngleTangentCoeff);
            graph.editorUtilities.PropertyFieldMinLabel(_TalusAngleTangentBias);
            graph.editorUtilities.PropertyFieldMinLabel(_SedimentCapacity);
            graph.editorUtilities.PropertyFieldMinLabel(_MaxErosionDepth);
            graph.editorUtilities.PropertyFieldMinLabel(_SuspensionRate);
            graph.editorUtilities.PropertyFieldMinLabel(_DepositionRate);
            graph.editorUtilities.PropertyFieldMinLabel(_SedimentSofteningRate);
            graph.editorUtilities.PropertyFieldMinLabel(_DeltaTime);
            graph.editorUtilities.PropertyFieldMinLabel(_PipeArea);
            graph.editorUtilities.PropertyFieldMinLabel(_PipeLength);
            graph.editorUtilities.PropertyFieldMinLabel(_Evaporation);
            graph.editorUtilities.PropertyFieldMinLabel(_RainRate);
            graph.editorUtilities.PropertyFieldMinLabel(_CellScale);
            graph.editorUtilities.PropertyFieldMinLabel(_Iterations);
        }
    }
}