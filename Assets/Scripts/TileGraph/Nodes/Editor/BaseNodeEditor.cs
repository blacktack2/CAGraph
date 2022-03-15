using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace TileGraph.Editors
{
    /// <summary> Base editor class containing operations and parameters common
    /// to all CAGraph node editors. </summary>
    /// <typeparam name="T"> Reference to the implementations node type.
    /// <example><code>
    /// public class MyNodeEditor : BaseNodeEditor&lt; MyNode &gt;
    /// </code></example> </typeparam>
    public abstract class BaseNodeEditor<T> : NodeEditor where T : Nodes.BaseNode
    {
        protected const int _ListWidth = 150;
        private struct Preview
        {
            public string portName;
            public bool isShowing;
        };

        protected TileGraph graph;
        protected T _Node;
        protected Utilities.EditorUtilities _EditorUtils;

        protected GUIStyle _PreviewToggleStyle, _PreviewStyle, _SquareButtonStyle;
        protected GUIContent _GPUActiveIcon, _GPUInactiveIcon, _CPUActiveIcon, _CPUInactiveIcon, _ReloadIcon;

        public static readonly float squareButtonSize = EditorGUIUtility.singleLineHeight * 2;

        protected int contentWidth {get {return GetWidth() - (GetBodyStyle().padding.left + GetBodyStyle().padding.right);}}

        private List<Preview> _TileMapPreviews;

        protected virtual bool GPUToggleable => false;

        public override void OnCreate()
        {
            _Node = target as T;
            graph = (TileGraph) _Node.graph;
            _EditorUtils = graph.editorUtilities;
            _TileMapPreviews = new List<Preview>();

            _PreviewStyle = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            _PreviewToggleStyle = new GUIStyle("Foldout") {alignment = TextAnchor.MiddleCenter};
            _SquareButtonStyle = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleRight};

            _GPUActiveIcon = new GUIContent(Resources.Load<Texture>("Icons/gpu_active"));
            _GPUInactiveIcon = new GUIContent(Resources.Load<Texture>("Icons/gpu_inactive"));
            _CPUActiveIcon = new GUIContent(Resources.Load<Texture>("Icons/cpu_active"));
            _CPUInactiveIcon = new GUIContent(Resources.Load<Texture>("Icons/cpu_inactive"));
            _ReloadIcon = new GUIContent(Resources.Load<Texture>("Icons/reload"));

            OnNodeEnable();
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NodeInputGUI();
            NodeBodyGUI();

            serializedObject.ApplyModifiedProperties();

            RenderPreviews();

            if (GPUToggleable)
            {
                RenderGPUToggle();
            }
        }

        private void RenderPreviews()
        {
            for (int i = 0; i < _TileMapPreviews.Count; i++)
            {
                Preview preview = _TileMapPreviews[i];
                EditorGUIUtility.labelWidth = contentWidth / 2;
                preview.isShowing = EditorGUILayout.Toggle(" ", preview.isShowing, _PreviewToggleStyle);
                if (preview.isShowing)
                {
                    Types.TileMap tileMap = (Types.TileMap) _Node.GetOutputPort(preview.portName).GetOutputValue();
                    if (tileMap == null)
                    {
                        // Render the null preview if no TileMap can be received
                        EditorGUILayout.LabelField("Cells: null");
                        EditorGUILayout.LabelField(
                            new GUIContent(_EditorUtils.nullPreview),
                            _PreviewStyle, 
                            GUILayout.Height(Utilities.EditorUtilities.previewWidth)
                        );
                    }
                    else
                    {
                        EditorGUILayout.LabelField(
                            string.Format("Cells: {0} ({1}x{2})", tileMap.GetCells().Length, tileMap.width, tileMap.height));
                        if (tileMap.preview == null)
                            tileMap.UpdatePreview();
                        EditorGUILayout.LabelField(
                            new GUIContent(tileMap.preview),
                            _PreviewStyle,
                            GUILayout.Height(Utilities.EditorUtilities.previewWidth)
                        );
                    }
                }
                EditorGUIUtility.labelWidth = 0;
                _TileMapPreviews[i] = preview;
            }
        }

        private void RenderGPUToggle()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button(_ReloadIcon, _SquareButtonStyle, GUILayout.Width(squareButtonSize), GUILayout.Height(squareButtonSize)))
                _Node.ResetGPUEnabled();
            if (GUILayout.Button(GetGPUToggleIcon(), _SquareButtonStyle, GUILayout.Width(squareButtonSize), GUILayout.Height(squareButtonSize)))
                _Node.ToggleGPUEnabled();

            EditorGUILayout.EndHorizontal();
        }

        private GUIContent GetGPUToggleIcon()
        {
            if (_Node.isGPUOverriden)
            {
                if (_Node.isGPUEnabled)
                    return _GPUActiveIcon;
                else
                    return _CPUActiveIcon;
            }
            else
            {
                if (graph.GPUEnabledGlobal)
                    return _GPUInactiveIcon;
                else
                    return _CPUInactiveIcon;
            }
        }

        /// <summary> Add TileMap output port to list of previews to be shown
        /// at the bottom of the node. </summary>
        /// <param name="portname"> Name of the port from which the TileMap to
        /// be previewed can be retrieved. </param>
        protected void AddPreview(string portName)
        {
            AddPreview(portName, true);
        }
    
        /// <summary> Add TileMap output port to list of previews to be shown
        /// at the bottom of the node. </summary>
        /// <param name="portname"> Name of the port from which the TileMap to
        /// be previewed can be retrieved. </param>
        /// <param name="startShowing"> <c>true</c> to unfold the preview by
        /// default, <c>false</c> to start the preview hidden. </param>
        protected void AddPreview(string portName, bool startShowing)
        {
            _TileMapPreviews.Add(new Preview {portName = portName, isShowing = startShowing});
        }

        /// <summary> Remove the first preview with a port name matching
        /// <paramref name="portName" />. </summary>
        /// <param name="portName"> Name of the port to stop previewing. Does
        /// nothing if no such port is found. </param>
        protected void RemovePreview(string portName)
        {
            for (int i = 0; i < _TileMapPreviews.Count; i++)
            {
                if (_TileMapPreviews[i].portName == portName)
                {
                    _TileMapPreviews.RemoveAt(i);
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
