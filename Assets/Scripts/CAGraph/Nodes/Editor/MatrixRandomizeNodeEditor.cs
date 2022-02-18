using System;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixRandomizeNode))]
    public class MatrixRandomizeNodeEditor : BaseNodeEditor<Nodes.MatrixRandomizeNode>
    {
        private SerializedProperty _MatrixIn, _MatrixOut, _Seed, _Chance;

        protected override void OnNodeEnable()
        {
            _MatrixIn  = serializedObject.FindProperty("_MatrixIn");
            _MatrixOut = serializedObject.FindProperty("_MatrixOut");
            _Seed      = serializedObject.FindProperty("_Seed");
            _Chance    = serializedObject.FindProperty("_Chance");

            AddPreview("_MatrixOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            EditorGUIUtility.labelWidth = contentWidth / 4;
            EditorGUILayout.BeginHorizontal();

            if (EditorGUILayout.DropdownButton(new GUIContent("Seed"), FocusType.Passive, GUILayout.Width(contentWidth / 2)))
                _Node.SetSeed((int) DateTime.Now.Ticks);
            graph.CAEditorUtilities.PropertyFieldMinLabel(_Seed, new GUIContent(), true, GUILayout.Width(contentWidth / 2));

            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = 0;

            graph.CAEditorUtilities.PropertyFieldMinLabel(_Chance);

        }
    }
}