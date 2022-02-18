using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.LifeCANode))]
    public class LifeCANodeEditor : BaseNodeEditor<Nodes.LifeCANode>
    {
        private SerializedProperty _MatrixIn, _MatrixOut, _Rules, _Iterations;

        // public override void OnBodyGUI()
        // {
        //     if (_LifeCANode == null)
        //         _LifeCANode = target as Nodes.LifeCANode;
            
        //     serializedObject.Update();
            
        //     EditorGUILayout.BeginHorizontal();

        //     NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixIn"));
        //     NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

        //     EditorGUILayout.EndHorizontal();

        //     EditorGUILayout.LabelField("Rule:", GUILayout.Width(30));
             
        //     string[] rules = _LifeCANode.GetRuleStrings();
        //     float labelWidth = EditorGUIUtility.labelWidth;
        //     EditorGUILayout.BeginHorizontal();

        //     EditorGUIUtility.labelWidth = 10;
        //     string born    = EditorGUILayout.TextField("B", rules[0]);
        //     string survive = EditorGUILayout.TextField("S", rules[1]);
        //     if (_LifeCANode.CompareNotation(born, survive))
        //         SetRuleFromNotation(born, survive);

        //     EditorGUILayout.EndHorizontal();
        //     EditorGUIUtility.labelWidth = labelWidth;

        //     NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_Iterations"), new GUIContent("Iterations"));

        //     serializedObject.ApplyModifiedProperties();

        //     _ShowPreview = Utilities.CAEditorUtilities.DisplayPreview(
        //         (Types.Matrix) _LifeCANode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);
        // }

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