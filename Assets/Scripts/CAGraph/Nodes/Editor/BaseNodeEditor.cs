using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    /// <summary> Base editor class containing operations and parameters common
    /// to all CAGraph node editors. </summary>
    /// <typeparam name="T"> Reference to the implementations node type.
    /// <example><code>
    /// public class MyNodeEditor : BaseNodeEditor&lt; MyNode &gt;
    /// </code></example> </typeparam>
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
                    Types.Matrix matrix = (Types.Matrix) _Node.GetOutputPort(preview.portName).GetOutputValue();
                    if (matrix == null)
                    {
                        // Render the null preview if no matrix can be received
                        EditorGUILayout.LabelField("Cells: null");
                        EditorGUILayout.LabelField(
                            new GUIContent(_EditorUtils.nullPreview),
                            _PreviewStyle, 
                            GUILayout.Height(Utilities.CAEditorUtilities.previewWidth)
                        );
                    }
                    else
                    {
                        EditorGUILayout.LabelField(
                            string.Format("Cells: {0} ({1}x{2})", matrix.GetCells().Length, matrix.width, matrix.height));
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

        /// <summary> Add matrix output port to list of previews to be shown
        /// at the bottom of the node. </summary>
        /// <param name="portname"> Name of the port from which the matrix to
        /// be previewed can be retrieved. </param>
        protected void AddPreview(string portName)
        {
            AddPreview(portName, true);
        }
    
        /// <summary> Add matrix output port to list of previews to be shown
        /// at the bottom of the node. </summary>
        /// <param name="portname"> Name of the port from which the matrix to
        /// be previewed can be retrieved. </param>
        /// <param name="startShowing"> <c>true</c> to unfold the preview by
        /// default, <c>false</c> to start the preview hidden. </param>
        protected void AddPreview(string portName, bool startShowing)
        {
            _MatrixPreviews.Add(new Preview {portName = portName, isShowing = startShowing});
        }

        /// <summary> Remove the first preview with a port name matching
        /// <paramref name="portName" />. </summary>
        /// <param name="portName"> Name of the port to stop previewing. Does
        /// nothing if no such port is found. </param>
        protected void RemovePreview(string portName)
        {
            for (int i = 0; i < _MatrixPreviews.Count; i++)
            {
                if (_MatrixPreviews[i].portName == portName)
                {
                    _MatrixPreviews.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary> Called during OnEnable. Init code and SerializedProperty
        /// references should go here. </summary>
        protected abstract void OnNodeEnable();
        /// <summary> Called during OnBodyGUI before NodeBodyGUI. Input and
        /// output port draw calls should go here. </summary>
        protected abstract void NodeInputGUI();
        /// <summary> Called during OnBodyGUI after NodeInputGUI, and before
        /// previews are rendered. Property draw calls should go here.
        /// </summary>
        protected abstract void NodeBodyGUI();
    }
}
