using System;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixRandomizeNode))]
    public class MatrixRandomizeNodeEditor : NodeEditor
    {
        private Nodes.MatrixRandomizeNode _MatrixRandomizeNode;

        private bool _ShowPreview = true;

        public override void OnBodyGUI()
        {
            if (_MatrixRandomizeNode == null)
                _MatrixRandomizeNode = target as Nodes.MatrixRandomizeNode;

            int width = GetWidth() - (GetBodyStyle().padding.left + GetBodyStyle().padding.right);

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixIn"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

            EditorGUILayout.EndHorizontal();

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width / 4;
            EditorGUILayout.BeginHorizontal();

            if (EditorGUILayout.DropdownButton(new GUIContent("Seed"), FocusType.Passive, GUILayout.Width(width / 2)))
                _MatrixRandomizeNode.SetSeed((int) DateTime.Now.Ticks);
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_Seed"),
                                              new GUIContent(), true, GUILayout.Width(width / 2));

            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = labelWidth;

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_Chance"));

            _ShowPreview = Utilities.CAEditorUtilities.DisplayPreview(
                (Types.Matrix) _MatrixRandomizeNode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);

            serializedObject.ApplyModifiedProperties();
        }
    }
}