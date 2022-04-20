using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ErosionTest))]
public class ErosionTestEditor : Editor
{
    private ErosionTest _ErosionTest;
    void OnEnable()
    {
        _ErosionTest = target as ErosionTest;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reload Terrain"))
            _ErosionTest.UpdateTerrain();
    }
}
