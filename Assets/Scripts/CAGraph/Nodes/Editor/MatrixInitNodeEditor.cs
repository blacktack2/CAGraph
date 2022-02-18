using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixInitNode))]
    public class MatrixInitNodeEditor : BaseNodeEditor<Nodes.MatrixInitNode>
    {
        private SerializedProperty _MatrixOut, _MatrixWidth, _MatrixHeight;

        protected override void OnNodeEnable()
        {
            _MatrixOut    = serializedObject.FindProperty("_MatrixOut");
            _MatrixWidth  = serializedObject.FindProperty("_MatrixWidth");
            _MatrixHeight = serializedObject.FindProperty("_MatrixHeight");

            AddPreview("_MatrixOut");
        }

        protected override void NodeInputGUI()
        {
            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixOut);
        }

        protected override void NodeBodyGUI()
        {
            EditorGUIUtility.labelWidth = contentWidth / 4;
            EditorGUILayout.BeginHorizontal();
            
            graph.CAEditorUtilities.PropertyFieldMinLabel(
                _MatrixWidth, new GUIContent("width"), true, GUILayout.Width(contentWidth / 2));
            graph.CAEditorUtilities.PropertyFieldMinLabel(
                _MatrixHeight, new GUIContent("height"), true, GUILayout.Width(contentWidth / 2));
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUIUtility.labelWidth = 0;
            _Node.UpdateMatrix();
        }

        private void DisplayMatrixBounds(int width)
        {
            
        }
    }
}
