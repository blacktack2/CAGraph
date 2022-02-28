using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapInitNode))]
    public class TileMapInitNodeEditor : BaseNodeEditor<Nodes.TileMapInitNode>
    {
        private SerializedProperty _TileMapBoolOut, _TileMapContOut, _TileMapUintOut, _TileMapWidth, _TileMapHeight, _TileMapType;

        private string _CurrentPreview;

        protected override void OnNodeEnable()
        {
            _TileMapBoolOut = serializedObject.FindProperty("_TileMapBoolOut");
            _TileMapContOut = serializedObject.FindProperty("_TileMapContOut");
            _TileMapUintOut = serializedObject.FindProperty("_TileMapUintOut");
            _TileMapWidth   = serializedObject.FindProperty("_TileMapWidth");
            _TileMapHeight  = serializedObject.FindProperty("_TileMapHeight");
            _TileMapType    = serializedObject.FindProperty("_TileMapType");
        }

        protected override void NodeInputGUI()
        {
            switch ((Nodes.TileMapInitNode.TileMapType) _TileMapType.enumValueIndex)
            {
                case Nodes.TileMapInitNode.TileMapType.Boolean:
                    graph.CAEditorUtilities.PortFieldMinLabel(_TileMapBoolOut);
                    break;
                case Nodes.TileMapInitNode.TileMapType.Continuous:
                    graph.CAEditorUtilities.PortFieldMinLabel(_TileMapContOut);
                    break;
                case Nodes.TileMapInitNode.TileMapType.Integer:
                    graph.CAEditorUtilities.PortFieldMinLabel(_TileMapUintOut);
                    break;
            }
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_TileMapType, new GUIContent("type:"));
            SetPreview();

            EditorGUILayout.BeginHorizontal();
            
            graph.CAEditorUtilities.PropertyFieldMinLabel(
                _TileMapWidth, new GUIContent("width:"), true, GUILayout.Width(contentWidth / 2));
            graph.CAEditorUtilities.PropertyFieldMinLabel(
                _TileMapHeight, new GUIContent("height:"), true, GUILayout.Width(contentWidth / 2));
            
            EditorGUILayout.EndHorizontal();
            
            _Node.UpdateTileMap();
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
