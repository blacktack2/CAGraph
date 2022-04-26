using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;
using static TileGraph.Utilities.FunctionLibrary;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapContErosionNode))]
    public class TileMapContErosionNodeEditor : BaseNodeEditor<Nodes.TileMapContErosionNode>
    {
        private SerializedProperty _TileMapIn, _TileMapOut,
            _Iterations, _Passes;
            
        private ReorderableList _PassList;
        private List<float> heights;
        private bool _ShowList = false;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");
            
            _Iterations = serializedObject.FindProperty("_Iterations");
            _Passes     = serializedObject.FindProperty("_Passes");

            heights = new List<float>(_Passes.arraySize);

            _PassList = new ReorderableList(serializedObject, _Passes);
            _PassList.drawElementCallback = DrawListItems;
            _PassList.elementHeightCallback = ListElementHeight;
            _PassList.onAddCallback = ListAdd;
            _PassList.drawHeaderCallback = DrawHeader;

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_Iterations);

            _ShowList = EditorGUILayout.Foldout(_ShowList, new GUIContent("Passes"));
            if (_ShowList)
                _PassList.DoLayoutList();
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            const int maxRepeats = 10;
            SerializedProperty element   = _Passes.GetArrayElementAtIndex(index);
            SerializedProperty repeats   = element.FindPropertyRelative("repeats");
            SerializedProperty algorithm = element.FindPropertyRelative("algorithm");

            SerializedProperty terrainHardness  = element.FindPropertyRelative("terrainHardness");
            SerializedProperty sedimentHardness = element.FindPropertyRelative("sedimentHardness");
            SerializedProperty depositionRate   = element.FindPropertyRelative("depositionRate");
            SerializedProperty rainRate         = element.FindPropertyRelative("rainRate");
            SerializedProperty rainAmount       = element.FindPropertyRelative("rainAmount");

            SerializedProperty maxSlope    = element.FindPropertyRelative("maxSlope");
            SerializedProperty thermalRate = element.FindPropertyRelative("thermalRate");

            float marginY = EditorGUIUtility.singleLineHeight / 10f;
            float marginX = rect.width / 20f;
            float spacing = EditorGUIUtility.singleLineHeight + marginY;

            Rect fieldRect = new Rect(rect.x + marginX / 2f, rect.y + marginY / 2f, rect.width - marginX, EditorGUIUtility.singleLineHeight);

            EditorGUIUtility.labelWidth = rect.width / 2f;
            algorithm.enumValueIndex = (int) (Erosion.Algorithm) EditorGUI.EnumPopup(fieldRect, (Erosion.Algorithm) algorithm.enumValueIndex);
            fieldRect.y += spacing;
            repeats.intValue = EditorGUI.IntField(fieldRect, "Repeats", repeats.intValue);
            int totalRepeats = TotalRepeats();
            if (totalRepeats > maxRepeats)
            {
                repeats.intValue -= totalRepeats - maxRepeats;
            }
            _PassList.displayRemove = !(totalRepeats >= maxRepeats);

            switch (algorithm.enumValueIndex)
            {
                case (int) Erosion.Algorithm.Hydraulic:
                    fieldRect.y += spacing;
                    terrainHardness.floatValue = EditorGUI.FloatField(fieldRect, "Ter. Hardness", terrainHardness.floatValue);
                    fieldRect.y += spacing;
                    sedimentHardness.floatValue = EditorGUI.FloatField(fieldRect, "Sed. Hardness", sedimentHardness.floatValue);
                    fieldRect.y += spacing;
                    depositionRate.floatValue = EditorGUI.FloatField(fieldRect, "Deposition", depositionRate.floatValue);
                    fieldRect.y += spacing;
                    rainRate.floatValue = EditorGUI.FloatField(fieldRect, "Rain Rate", rainRate.floatValue);
                    fieldRect.y += spacing;
                    rainAmount.floatValue = EditorGUI.FloatField(fieldRect, "Rain Amount", rainAmount.floatValue);
                    break;
                case (int) Erosion.Algorithm.Fluvial: default:
                    break;
                case (int) Erosion.Algorithm.Thermal:
                    fieldRect.y += spacing;
                    maxSlope.floatValue = EditorGUI.FloatField(fieldRect, "Max Slope", maxSlope.floatValue);
                    fieldRect.y += spacing;
                    thermalRate.floatValue = EditorGUI.FloatField(fieldRect, "Rate", thermalRate.floatValue);
                    break;
            }
            EditorGUIUtility.labelWidth = -1;
        }

        private float ListElementHeight(int index)
        {
            return EditorGUIUtility.singleLineHeight * 8.1f;
        }

        private void ListAdd(ReorderableList list)
        {
            int index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
            element.FindPropertyRelative("algorithm").enumValueIndex = (int) Erosion.Algorithm.Hydraulic;
            element.FindPropertyRelative("repeats").intValue         = 1;

            element.FindPropertyRelative("terrainHardness").floatValue  = 1f;
            element.FindPropertyRelative("sedimentHardness").floatValue = 1f;
            element.FindPropertyRelative("depositionRate").floatValue   = 1f;
            element.FindPropertyRelative("rainRate").floatValue         = 0.5f;
            element.FindPropertyRelative("rainAmount").floatValue       = 1f;

            element.FindPropertyRelative("maxSlope").floatValue    = 3.6f;
            element.FindPropertyRelative("thermalRate").floatValue = 0.146f;
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Passes");
        }

        private int TotalRepeats()
        {
            int totalRepeats = 0;
            for (int i = 0; i < _Passes.arraySize; i++)
                totalRepeats += _Passes.GetArrayElementAtIndex(i).FindPropertyRelative("repeats").intValue;
            return totalRepeats;
        }
    }
}
