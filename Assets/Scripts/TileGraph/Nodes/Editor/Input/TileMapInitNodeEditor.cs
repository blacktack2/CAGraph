using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapInitNode))]
    public class TileMapInitNodeEditor : BaseNodeEditor<Nodes.TileMapInitNode>
    {
        private SerializedProperty _TileMapWidth, _TileMapHeight, _TileMapBoolOut, _TileMapContOut, _TileMapUintOut, _TileMapType;

        private string _CurrentPreview;

        protected override void OnNodeEnable()
        {
            _TileMapWidth   = serializedObject.FindProperty("_TileMapWidth");
            _TileMapHeight  = serializedObject.FindProperty("_TileMapHeight");
            _TileMapBoolOut = serializedObject.FindProperty("_TileMapBoolOut");
            _TileMapContOut = serializedObject.FindProperty("_TileMapContOut");
            _TileMapUintOut = serializedObject.FindProperty("_TileMapUintOut");
            _TileMapType    = serializedObject.FindProperty("_TileMapType");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapWidth, new GUIContent("width "));
            graph.CAEditorUtilities.PortFieldMinLabel(_TileMapHeight, new GUIContent("height"));
            EditorGUILayout.EndVertical();

            switch ((Nodes.TileMapInitNode.TileMapType) _TileMapType.enumValueIndex)
            {
                case Nodes.TileMapInitNode.TileMapType.Boolean:
                    graph.CAEditorUtilities.PortFieldMinLabel(_TileMapBoolOut, new GUIContent("tilemap out"));
                    break;
                case Nodes.TileMapInitNode.TileMapType.Continuous:
                    graph.CAEditorUtilities.PortFieldMinLabel(_TileMapContOut, new GUIContent("tilemap out"));
                    break;
                case Nodes.TileMapInitNode.TileMapType.Integer:
                    graph.CAEditorUtilities.PortFieldMinLabel(_TileMapUintOut, new GUIContent("tilemap out"));
                    break;
            }
            EditorGUILayout.EndHorizontal();
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_TileMapType, new GUIContent("type:"));
            SetPreview();
        }

        private void SetPreview()
        {
            switch ((Nodes.TileMapInitNode.TileMapType) _TileMapType.enumValueIndex)
            {
                case Nodes.TileMapInitNode.TileMapType.Boolean:
                    if (_CurrentPreview != "_TileMapBoolOut")
                    {
                        RemovePreview(_CurrentPreview);
                        AddPreview("_TileMapBoolOut");
                        _CurrentPreview = "_TileMapBoolOut";
                    }
                    break;
                case Nodes.TileMapInitNode.TileMapType.Continuous:
                    if (_CurrentPreview != "_TileMapContOut")
                    {
                        RemovePreview(_CurrentPreview);
                        AddPreview("_TileMapContOut");
                        _CurrentPreview = "_TileMapContOut";
                    }
                    break;
                case Nodes.TileMapInitNode.TileMapType.Integer:
                    if (_CurrentPreview != "_TileMapUintOut")
                    {
                        RemovePreview(_CurrentPreview);
                        AddPreview("_TileMapUintOut");
                        _CurrentPreview = "_TileMapUintOut";
                    }
                    break;
            }
        }
    }
}
