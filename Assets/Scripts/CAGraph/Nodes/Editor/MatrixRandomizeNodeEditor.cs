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

            NodeEditorGUILayout.PropertyField(_MatrixIn);
            NodeEditorGUILayout.PropertyField(_MatrixOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            EditorGUIUtility.labelWidth = contentWidth / 4;
            EditorGUILayout.BeginHorizontal();

            if (EditorGUILayout.DropdownButton(new GUIContent("Seed"), FocusType.Passive, GUILayout.Width(contentWidth / 2)))
                _Node.SetSeed((int) DateTime.Now.Ticks);
            NodeEditorGUILayout.PropertyField(_Seed, new GUIContent(), true, GUILayout.Width(contentWidth / 2));

            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = 0;

            NodeEditorGUILayout.PropertyField(_Chance);

        }
    }
}