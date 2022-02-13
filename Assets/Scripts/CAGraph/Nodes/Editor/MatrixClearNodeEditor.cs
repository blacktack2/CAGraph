using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixClearNode))]
    public class MatrixClearNodeEditor : NodeEditor
    {
        private Nodes.MatrixClearNode _MatrixClearNode;

        private bool _ShowPreview = true;

        public override void OnBodyGUI()
        {
            if (_MatrixClearNode == null)
                _MatrixClearNode = target as Nodes.MatrixClearNode;
            
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixIn"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

            EditorGUILayout.EndHorizontal();

            _ShowPreview = Utilities.CAEditorUtilities.DisplayPreview(
                (Types.Matrix) _MatrixClearNode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}