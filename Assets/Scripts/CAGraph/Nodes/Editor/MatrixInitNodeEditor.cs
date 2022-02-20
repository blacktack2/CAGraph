using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.MatrixInitNode))]
    public class MatrixInitNodeEditor : BaseNodeEditor<Nodes.MatrixInitNode>
    {
        private SerializedProperty _Matrix01Out, _MatrixContinuousOut, _MatrixUIntOut, _MatrixWidth, _MatrixHeight, _MatrixType;

        private string _CurrentPreview;

        protected override void OnNodeEnable()
        {
            _Matrix01Out            = serializedObject.FindProperty("_Matrix01Out");
            _MatrixContinuousOut    = serializedObject.FindProperty("_MatrixContinuousOut");
            _MatrixUIntOut          = serializedObject.FindProperty("_MatrixUIntOut");
            _MatrixWidth            = serializedObject.FindProperty("_MatrixWidth");
            _MatrixHeight           = serializedObject.FindProperty("_MatrixHeight");
            _MatrixType           = serializedObject.FindProperty("_MatrixType");
        }

        protected override void NodeInputGUI()
        {
            switch ((Nodes.MatrixInitNode.MatrixType) _MatrixType.enumValueIndex)
            {
                case Nodes.MatrixInitNode.MatrixType.Boolean:
                    graph.CAEditorUtilities.PortFieldMinLabel(_Matrix01Out);
                    break;
                case Nodes.MatrixInitNode.MatrixType.Continuous:
                    graph.CAEditorUtilities.PortFieldMinLabel(_MatrixContinuousOut);
                    break;
                case Nodes.MatrixInitNode.MatrixType.Integer:
                    graph.CAEditorUtilities.PortFieldMinLabel(_MatrixUIntOut);
                    break;
            }
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_MatrixType, new GUIContent("type:"));
            SetPreview();

            EditorGUILayout.BeginHorizontal();
            
            graph.CAEditorUtilities.PropertyFieldMinLabel(
                _MatrixWidth, new GUIContent("width:"), true, GUILayout.Width(contentWidth / 2));
            graph.CAEditorUtilities.PropertyFieldMinLabel(
                _MatrixHeight, new GUIContent("height:"), true, GUILayout.Width(contentWidth / 2));
            
            EditorGUILayout.EndHorizontal();
            
            _Node.UpdateMatrix();
        }

        private void SetPreview()
        {
            switch ((Nodes.MatrixInitNode.MatrixType) _MatrixType.enumValueIndex)
            {
                case Nodes.MatrixInitNode.MatrixType.Boolean:
                    if (_CurrentPreview != "_Matrix01Out")
                    {
                        RemovePreview(_CurrentPreview);
                        AddPreview("_Matrix01Out");
                        _CurrentPreview = "_Matrix01Out";
                    }
                    break;
                case Nodes.MatrixInitNode.MatrixType.Continuous:
                    if (_CurrentPreview != "_MatrixContinuousOut")
                    {
                        RemovePreview(_CurrentPreview);
                        AddPreview("_MatrixContinuousOut");
                        _CurrentPreview = "_MatrixContinuousOut";
                    }
                    break;
                case Nodes.MatrixInitNode.MatrixType.Integer:
                    if (_CurrentPreview != "_MatrixUIntOut")
                    {
                        RemovePreview(_CurrentPreview);
                        AddPreview("_MatrixUIntOut");
                        _CurrentPreview = "_MatrixUIntOut";
                    }
                    break;
            }
        }
    }
}
