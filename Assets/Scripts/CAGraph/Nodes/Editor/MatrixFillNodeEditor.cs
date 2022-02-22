using UnityEditor;
using UnityEngine;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixFillNode))]
    public class MatrixFillNodeEditor : BaseNodeEditor<Nodes.MatrixFillNode>
    {
        private SerializedProperty _MatrixIn, _FillValue, _MatrixOut;

        protected override void OnNodeEnable()
        {
            _MatrixIn  = serializedObject.FindProperty("_MatrixIn");
            _FillValue = serializedObject.FindProperty("_FillValue");
            _MatrixOut = serializedObject.FindProperty("_MatrixOut");

            AddPreview("_MatrixOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixIn);
            graph.CAEditorUtilities.PortFieldMinLabel(_MatrixOut);

            EditorGUILayout.EndHorizontal();

            graph.CAEditorUtilities.PortFieldMinLabel(_FillValue, new GUIContent("fill value"));
        }

        protected override void NodeBodyGUI()
        {
        }
    }
}