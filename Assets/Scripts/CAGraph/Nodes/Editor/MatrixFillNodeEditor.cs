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

            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_FillValue, new GUIContent("fill value"));
        }
    }
}