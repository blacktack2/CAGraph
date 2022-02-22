using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.LifeCANode))]
    public class LifeCANodeEditor : BaseNodeEditor<Nodes.LifeCANode>
    {
        private SerializedProperty _MatrixIn, _MatrixOut, _Rules, _Iterations;

        protected override void OnNodeEnable()
        {
            _MatrixIn   = serializedObject.FindProperty("_MatrixIn");
            _MatrixOut  = serializedObject.FindProperty("_MatrixOut");
            _Rules  = serializedObject.FindProperty("_Rules");
            _Iterations = serializedObject.FindProperty("_Iterations");

            AddPreview("_MatrixOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixOut);

            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            EditorGUILayout.LabelField("Rule:", GUILayout.Width(30));
            
            string[] rules = _Node.GetRuleStrings();
            EditorGUILayout.BeginHorizontal();

            // Display rules in conventional B/S notation (e.g. Conway's Game of Life should be notated B3S23 or 3/23)
            graph.CAEditorUtilities.SetLabelWidthToText("B");
            string born    = EditorGUILayout.TextField("B", rules[0]);
            string survive = EditorGUILayout.TextField("S", rules[1]);
            EditorGUIUtility.labelWidth = 0;

            if (_Node.CompareNotation(born, survive))
                SetRuleFromNotation(born, survive);

            EditorGUILayout.EndHorizontal();

            graph.CAEditorUtilities.PropertyFieldMinLabel(_Iterations, new GUIContent("Iterations"));
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