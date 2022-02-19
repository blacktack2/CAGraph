using System;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixRandomizeNode))]
    public class MatrixRandomizeNodeEditor : BaseNodeEditor<Nodes.MatrixRandomizeNode>
    {
        private SerializedProperty _MatrixIn, _Seed, _MatrixOut, _Chance;

        protected override void OnNodeEnable()
        {
            _MatrixIn  = serializedObject.FindProperty("_MatrixIn");
            _Seed      = serializedObject.FindProperty("_Seed");
            _Chance    = serializedObject.FindProperty("_Chance");
            _MatrixOut = serializedObject.FindProperty("_MatrixOut");

            AddPreview("_MatrixOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixOut);

            EditorGUILayout.EndHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_Seed);

            graph.CAEditorUtilities.PortFieldMinLabel(_Chance);
        }

        protected override void NodeBodyGUI()
        {
            
        }
    }
}