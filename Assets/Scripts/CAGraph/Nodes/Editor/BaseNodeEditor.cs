using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    public abstract class BaseNodeEditor<T> : NodeEditor where T : Nodes.BaseNode
    {
        private struct Preview
        {
            public string portName;
            public bool isShowing;
        };

        protected CAGraph graph;
        protected T _Node;
        protected Utilities.CAEditorUtilities _EditorUtils;

        protected GUIStyle _PreviewToggleStyle;
        protected GUIStyle _PreviewStyle;

        protected int contentWidth {get {return GetWidth() - (GetBodyStyle().padding.left + GetBodyStyle().padding.right);}}

        private List<Preview> _MatrixPreviews;


        public override void OnCreate()
        {
            _Node = target as T;
            graph = (CAGraph) _Node.graph;
            _EditorUtils = graph.CAEditorUtilities;
            _MatrixPreviews = new List<Preview>();

            _PreviewStyle = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            _PreviewToggleStyle = new GUIStyle("Foldout") {alignment = TextAnchor.MiddleCenter};

            OnNodeEnable();
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeInputGUI();
            NodeBodyGUI();
            serializedObject.ApplyModifiedProperties();
            RenderPreviews();
        }

        private void RenderPreviews()
        {
            for (int i = 0; i < _MatrixPreviews.Count; i++)
            {
                Preview preview = _MatrixPreviews[i];
                EditorGUIUtility.labelWidth = contentWidth / 2;
                preview.isShowing = EditorGUILayout.Toggle(" ", preview.isShowing, _PreviewToggleStyle);
                if (preview.isShowing)
                {
                    Types.Matrix matrix = (Types.Matrix) _Node.GetOutputPort("_MatrixOut").GetOutputValue();
                    if (matrix == null)
                    {
                        EditorGUILayout.LabelField("Cells: null");
                        EditorGUILayout.LabelField(
                            new GUIContent(_EditorUtils.nullPreview),
                            _PreviewStyle, 
                            GUILayout.Height(Utilities.CAEditorUtilities.previewWidth)
                        );
                    }
                    else
                    {
                        EditorGUILayout.LabelField(string.Format("Cells: {0} ({1}x{2})", matrix.GetCells().Length, matrix.width, matrix.height));
                        if (matrix.preview == null)
                            matrix.UpdatePreview();
                        EditorGUILayout.LabelField(
                            new GUIContent(matrix.preview),
                            _PreviewStyle,
                            GUILayout.Height(Utilities.CAEditorUtilities.previewWidth)
                        );
                    }
                }
                EditorGUIUtility.labelWidth = 0;
                _MatrixPreviews[i] = preview;
            }
        }

        protected void AddPreview(string portName)
        {
            AddPreview(portName, true);
        }

        protected void AddPreview(string portName, bool startShowing)
        {
            _MatrixPreviews.Add(new Preview {portName = portName, isShowing = startShowing});
        }

        protected abstract void OnNodeEnable();
        protected abstract void NodeInputGUI();
        protected abstract void NodeBodyGUI();
    }
}
