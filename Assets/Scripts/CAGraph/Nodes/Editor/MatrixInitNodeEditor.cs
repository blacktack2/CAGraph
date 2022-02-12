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

    private enum InitMode { EMPTY, RANDOM }
    private InitMode _InitMode = InitMode.EMPTY;

    private int _RandomizerSeed = 0;
    private float _RandomChance = 0.5f;

    void OnFocus()
    {
        LoadPrefs();
    }
    void OnLostFocus()
    {
        SavePrefs();
    }
    void OnDestroy()
    {
        SavePrefs();
    }

    private void LoadPrefs()
    {
        if (EditorPrefs.HasKey("_InitMode"))
            _InitMode = (InitMode) EditorPrefs.GetInt("_InitMode");
        if (EditorPrefs.HasKey("_RandomizerSeed"))
            _RandomizerSeed = EditorPrefs.GetInt("_RandomizerSeed");
        if (EditorPrefs.HasKey("_RandomChance"))
            _RandomChance = EditorPrefs.GetFloat("_RandomChance");
    }
    private void SavePrefs()
    {
        EditorPrefs.SetInt("_InitMode", (int) _InitMode);
        EditorPrefs.SetInt("_RandomizerSeed", _RandomizerSeed);
        EditorPrefs.SetFloat("_RandomChance", _RandomChance);
    }

    public override void OnBodyGUI()
    {
        if (_MatrixInitNode == null)
            _MatrixInitNode = target as MatrixInitNode;

        int width = GetWidth() - (GetBodyStyle().padding.left + GetBodyStyle().padding.right);

        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

        DisplayMatrixBounds(width);
        _MatrixInitNode.UpdateMatrix();
        DisplayMatrixInitOperations(width);

        _ShowPreview = CAEditorUtilities.DisplayPreview(_MatrixInitNode.GetMatrix(), _ShowPreview);

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

    private void DisplayMatrixInitOperations(int width)
    {
        _InitMode = (InitMode) EditorGUILayout.EnumPopup(_InitMode);
        switch (_InitMode)
        {
            case InitMode.EMPTY:
                DisplayInitOptionsEmpty(width);
                break;
            case InitMode.RANDOM:
                DisplayInitOptionsRandom(width);
                break;
        }
    }

    private void DisplayInitOptionsEmpty(int width)
    {
        if (EditorGUILayout.DropdownButton(new GUIContent("Clear"), FocusType.Passive))
        {
            MatrixOperations.ClearMatrix(_MatrixInitNode.GetMatrix());
        }
    }

    private void DisplayInitOptionsRandom(int width)
    {
        EditorGUILayout.BeginHorizontal();
        
        int seed = _RandomizerSeed;
        if (EditorGUILayout.DropdownButton(new GUIContent("Reroll Seed"), FocusType.Passive, GUILayout.Width(width / 2)))
            seed = (int) DateTime.Now.Ticks;
        seed = EditorGUILayout.IntField(seed);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        float chance = Mathf.Clamp01(EditorGUILayout.FloatField("Chance", _RandomChance));
        
        if (seed != _RandomizerSeed || chance != _RandomChance)
        {
            _RandomizerSeed = seed;
            _RandomChance = chance;
            MatrixOperations.RandomizeMatrix(_MatrixInitNode.GetMatrix(), _RandomChance, _RandomizerSeed);
        }
 
        EditorGUILayout.EndHorizontal();
    }
}
