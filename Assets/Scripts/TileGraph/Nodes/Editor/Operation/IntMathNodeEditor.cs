using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.IntMathNode))]
    public class IntMathNodeEditor : BaseNodeEditor<Nodes.IntMathNode>
    {
        private SerializedProperty _A, _B, _Out, _Operation;

        protected override void OnNodeEnable()
        {
            _A   = serializedObject.FindProperty("_A");
            _B   = serializedObject.FindProperty("_B");
            _Out = serializedObject.FindProperty("_Out");

            _Operation = serializedObject.FindProperty("_Operation");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            graph.editorUtilities.PortFieldMinLabel(_A);
            graph.editorUtilities.PortFieldMinLabel(_B);
            EditorGUILayout.EndVertical();

            graph.editorUtilities.PortFieldMinLabel(_Out);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_Operation);
        }
    }
}
