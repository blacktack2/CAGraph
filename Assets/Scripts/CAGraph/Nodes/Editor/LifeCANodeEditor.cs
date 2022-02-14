using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.LifeCANode))]
    public class LifeCANodeEditor : NodeEditor
    {
        private Nodes.LifeCANode _LifeCANode;

        private bool _ShowPreview = true;

        public override void OnBodyGUI()
        {
            if (_LifeCANode == null)
                _LifeCANode = target as Nodes.LifeCANode;
            
            serializedObject.Update();
            
            EditorGUILayout.BeginHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixIn"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_MatrixOut"));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Rule:", GUILayout.Width(30));
             
            string[] rules = _LifeCANode.GetRuleStrings();
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 10;
            string born    = EditorGUILayout.TextField("B", rules[0]);
            string survive = EditorGUILayout.TextField("S", rules[1]);
            if (_LifeCANode.CompareNotation(born, survive))
                SetRuleFromNotation(born, survive);

            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = labelWidth;

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_Iterations"), new GUIContent("Iterations"));

            serializedObject.ApplyModifiedProperties();

            _ShowPreview = Utilities.CAEditorUtilities.DisplayPreview(
                (Types.Matrix) _LifeCANode.GetOutputPort("_MatrixOut").GetOutputValue(), _ShowPreview);
        }

        private void SetRuleFromNotation(string born, string survive)
        {
            if (!Regex.IsMatch(survive, "^[0-8]*$") || !Regex.IsMatch(born, "^[0-8]*$"))
                return;

            SerializedProperty rules = serializedObject.FindProperty("_Rules");    
            for (int i = 0; i < 9; i++)
            {
                string n = i.ToString();
                SerializedProperty b = rules.GetArrayElementAtIndex(i);
                SerializedProperty s = rules.GetArrayElementAtIndex(i + 9);
                if (born.Contains(n) != b.boolValue)
                    b.boolValue = !b.boolValue;
                if (survive.Contains(n) != s.boolValue)
                    s.boolValue = !s.boolValue;
            }
        }
    }
}