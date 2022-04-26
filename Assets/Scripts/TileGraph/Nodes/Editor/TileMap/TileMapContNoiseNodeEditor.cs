using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContNoiseNode))]
    public class TileMapContNoiseNodeEditor : BaseNodeEditor<Nodes.TileMapContNoiseNode>
    {
        private SerializedProperty _TileMapIn, _Frequency, _Offset, _IntOffset, _TileMapOut,
                                   _RelativeFrequency, _Algorithm, _Advanced,
                                   _Detail, _Octaves, _Lacunarity, _Persistence;

        protected override bool GPUToggleable => true;

        private ReorderableList _OctaveParamList;
        private List<object> _OctaveParams;

        private bool _ShowList = false;

        protected override void OnNodeEnable()
        {
            _TileMapIn         = serializedObject.FindProperty("_TileMapIn");
            _Frequency         = serializedObject.FindProperty("_Frequency");
            _Offset            = serializedObject.FindProperty("_Offset");
            _IntOffset         = serializedObject.FindProperty("_IntOffset");
            _TileMapOut        = serializedObject.FindProperty("_TileMapOut");

            _RelativeFrequency = serializedObject.FindProperty("_RelativeFrequency");
            _Algorithm         = serializedObject.FindProperty("_Algorithm");
            _Advanced          = serializedObject.FindProperty("_Advanced");

            _Detail            = serializedObject.FindProperty("_Detail");
            _Octaves           = serializedObject.FindProperty("_Octaves");
            _Lacunarity        = serializedObject.FindProperty("_Lacunarity");
            _Persistence       = serializedObject.FindProperty("_Persistence");

            _OctaveParams = new List<object>();
            for (int i = 0; i < _Octaves.intValue - 1; i++)
            {
                _OctaveParams.Add(null);
            }

            _OctaveParamList = new ReorderableList(_OctaveParams, typeof(object), false, true, false, false);
            _OctaveParamList.drawElementCallback = DrawListItems;
            _OctaveParamList.drawHeaderCallback = DrawHeader;

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            bool isWhite = _Algorithm.enumValueIndex == (int) Utilities.FunctionLibrary.Noise.Algorithm.White;

            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            if (!isWhite)
                graph.editorUtilities.PortFieldMinLabel(_Frequency);

            if (isWhite)
                graph.editorUtilities.PortFieldMinLabel(_IntOffset, new GUIContent("Offset"));
            else
                graph.editorUtilities.PortFieldMinLabel(_Offset, new GUIContent("Offset"));
        }

        protected override void NodeBodyGUI()
        {
            bool isWhite = _Algorithm.enumValueIndex == (int) Utilities.FunctionLibrary.Noise.Algorithm.White;
            if (!isWhite)
                graph.editorUtilities.PropertyFieldMinLabel(_RelativeFrequency, new GUIContent("Relative Scale"));
            graph.editorUtilities.PropertyFieldMinLabel(_Algorithm, new GUIContent("Algorithm"));
            if (!isWhite)
            {
                graph.editorUtilities.PropertyFieldMinLabel(_Advanced, new GUIContent("Advanced"));
                serializedObject.ApplyModifiedProperties();
                if (_Advanced.boolValue)
                {
                    EditorGUILayout.BeginHorizontal();

                    graph.editorUtilities.SetLabelWidthToText("Octaves");
                    _ShowList = EditorGUILayout.Foldout(_ShowList, new GUIContent("Octaves"));
                    EditorGUIUtility.labelWidth = 0;

                    serializedObject.ApplyModifiedProperties();
                    while (_Octaves.intValue - 1 > _OctaveParamList.list.Count)
                        _OctaveParamList.list.Add(null);
                    while (_Octaves.intValue - 1 < _OctaveParamList.list.Count)
                        _OctaveParamList.list.RemoveAt(_OctaveParamList.list.Count - 1);
                    graph.editorUtilities.PropertyFieldMinLabel(_Octaves, new GUIContent(""));

                    EditorGUILayout.EndHorizontal();

                    if (_ShowList)
                        _OctaveParamList.DoLayoutList();
                }
                else
                {
                    graph.editorUtilities.PropertyFieldMinLabel(_Detail);
                }
            }
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty lacunarity = _Lacunarity.GetArrayElementAtIndex(index);
            SerializedProperty persistence = _Persistence.GetArrayElementAtIndex(index);

            lacunarity.floatValue = EditorGUI.FloatField(
                new Rect(rect.x, rect.y, _ListWidth / 2, EditorGUIUtility.singleLineHeight), lacunarity.floatValue
            );
            persistence.floatValue = EditorGUI.FloatField(
                new Rect(rect.x + _ListWidth / 2, rect.y, _ListWidth / 2, EditorGUIUtility.singleLineHeight), persistence.floatValue
            );
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, _ListWidth / 2, EditorGUIUtility.singleLineHeight), "Lacunarity");
            EditorGUI.LabelField(new Rect(rect.x + _ListWidth / 2, rect.y, _ListWidth / 2, EditorGUIUtility.singleLineHeight), "Persistence");
        }
    }
}