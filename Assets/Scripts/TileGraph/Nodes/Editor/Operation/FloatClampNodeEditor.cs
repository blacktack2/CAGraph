using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.FloatClampNode))]
    public class FloatClampNodeEditor : BaseNodeEditor<Nodes.FloatClampNode>
    {
        private SerializedProperty _Var, _Min, _Max, _Out;

        protected override void OnNodeEnable()
        {
            _Var = serializedObject.FindProperty("_Var");
            _Min = serializedObject.FindProperty("_Min");
            _Max = serializedObject.FindProperty("_Max");
            _Out = serializedObject.FindProperty("_Out");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            graph.editorUtilities.PortFieldMinLabel(_Var);
            graph.editorUtilities.PortFieldMinLabel(_Min);
            graph.editorUtilities.PortFieldMinLabel(_Max);
            EditorGUILayout.EndVertical();

            graph.editorUtilities.PortFieldMinLabel(_Out);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
        }
    }
}
