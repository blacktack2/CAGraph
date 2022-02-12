using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(MatrixRandomizeNode))]
public class MatrixRandomizeNodeEditor : NodeEditor
{
    private MatrixRandomizeNode _MatrixRandomizeNode;

    private bool _ShowPreview = true;

    public override void OnBodyGUI()
    {
        if (_MatrixRandomizeNode == null)
            _MatrixRandomizeNode = target as MatrixRandomizeNode;

        int width = GetWidth() - (GetBodyStyle().padding.left + GetBodyStyle().padding.right);

        EditorGUILayout.BeginHorizontal();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixIn"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

        EditorGUILayout.EndHorizontal();

        _ShowPreview = CAEditorUtilities.DisplayPreview((Matrix) _MatrixRandomizeNode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);
    }
}
