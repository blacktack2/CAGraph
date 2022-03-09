using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.LifeCANode))]
    public class LifeCANodeEditor : BaseNodeEditor<Nodes.LifeCANode>
    {
        private SerializedProperty _TileMapIn, _TileMapOut, _Rules, _Iterations;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");
            _Rules      = serializedObject.FindProperty("_Rules");
            _Iterations = serializedObject.FindProperty("_Iterations");

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
            EditorGUILayout.LabelField("Rule:", GUILayout.Width(30));
            
            string[] rules = _Node.GetRuleStrings();
            EditorGUILayout.BeginHorizontal();

            // Display rules in conventional B/S notation (e.g. Conway's Game of Life should be notated B3S23 or 3/23)
            graph.editorUtilities.SetLabelWidthToText("B");
            string born    = EditorGUILayout.TextField("B", rules[0]);
            string survive = EditorGUILayout.TextField("S", rules[1]);
            EditorGUIUtility.labelWidth = 0;

            if (_Node.CompareNotation(born, survive))
                SetRuleFromNotation(born, survive);

            EditorGUILayout.EndHorizontal();

            graph.editorUtilities.SetLabelWidthToText("Iterations");
            _Iterations.intValue = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Iterations"), _Iterations.intValue), 0, Nodes.LifeCANode.maxIterations);
            EditorGUIUtility.labelWidth = 0;
        }

        private void SetRuleFromNotation(string born, string survive)
        {
            if (!Regex.IsMatch(survive, "^[0-8]*$") || !Regex.IsMatch(born, "^[0-8]*$"))
                return;

            for (int i = 0; i < 9; i++)
            {
                string n = i.ToString();
                SerializedProperty b = _Rules.GetArrayElementAtIndex(i);
                SerializedProperty s = _Rules.GetArrayElementAtIndex(i + 9);
                if (born.Contains(n) != b.boolValue)
                    b.boolValue = !b.boolValue;
                if (survive.Contains(n) != s.boolValue)
                    s.boolValue = !s.boolValue;
            }
        }
    }
}