using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixFillNode))]
    public class MatrixFillNodeEditor : BaseNodeEditor<Nodes.MatrixFillNode>
    {
        private SerializedProperty _MatrixIn, _MatrixOut, _FillValue;

        protected override void OnNodeEnable()
        {
            _MatrixIn  = serializedObject.FindProperty("_MatrixIn");
            _MatrixOut = serializedObject.FindProperty("_MatrixOut");
            _FillValue = serializedObject.FindProperty("_FillValue");

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
            NodeEditorGUILayout.PropertyField(_FillValue, new GUIContent("fill value"));
        }
    }
}