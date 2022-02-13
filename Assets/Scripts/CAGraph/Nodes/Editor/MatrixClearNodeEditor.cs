using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(MatrixClearNode))]
public class MatrixClearNodeEditor : NodeEditor
{
    private MatrixClearNode _MatrixClearNode;

    private bool _ShowPreview = true;

    public override void OnBodyGUI()
    {
        if (_MatrixClearNode == null)
            _MatrixClearNode = target as MatrixClearNode;
        
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixIn"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

        EditorGUILayout.EndHorizontal();

        _ShowPreview = CAEditorUtilities.DisplayPreview((Matrix) _MatrixClearNode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);
        
        serializedObject.ApplyModifiedProperties();
    }
}
