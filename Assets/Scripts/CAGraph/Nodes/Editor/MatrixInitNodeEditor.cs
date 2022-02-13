using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(MatrixInitNode))]
public class MatrixInitNodeEditor : NodeEditor
{
    private MatrixInitNode _MatrixInitNode;

    private bool _ShowPreview = true;

    public override void OnBodyGUI()
    {
        if (_MatrixInitNode == null)
            _MatrixInitNode = target as MatrixInitNode;

        int width = GetWidth() - (GetBodyStyle().padding.left + GetBodyStyle().padding.right);

        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

        DisplayMatrixBounds(width);
        _MatrixInitNode.UpdateMatrix();
        
        _ShowPreview = CAEditorUtilities.DisplayPreview((Matrix) _MatrixInitNode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayMatrixBounds(int width)
    {
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = width / 4;
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixWidth"), new GUIContent("width"), GUILayout.Width(width / 2));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixHeight"), new GUIContent("height"), GUILayout.Width(width / 2));
        
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = labelWidth;
    }
}
