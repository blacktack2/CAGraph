using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapUintMathNode))]
    public class TileMapUintMathNodeEditor : BaseNodeEditor<Nodes.TileMapUintMathNode>
    {
        private SerializedProperty _TileMapIn, _TileMapBIn, _TileMapOut, _Value, _Operation, _OffsetX, _OffsetY;

        protected override bool GPUToggleable => true;

        protected override void OnNodeEnable()
        {
            _TileMapIn  = serializedObject.FindProperty("_TileMapIn");
            _TileMapBIn = serializedObject.FindProperty("_TileMapBIn");
            _TileMapOut = serializedObject.FindProperty("_TileMapOut");
            
            _Value     = serializedObject.FindProperty("_Value");
            _OffsetX   = serializedObject.FindProperty("_OffsetX");
            _OffsetY   = serializedObject.FindProperty("_OffsetY");
            _Operation = serializedObject.FindProperty("_Operation");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();
            
            graph.editorUtilities.PortFieldMinLabel(_TileMapBIn);
        }

        protected override void NodeBodyGUI()
        {
            if (_Node.GetInputPort("_TileMapBIn").ConnectionCount == 0)
            {
                graph.editorUtilities.PropertyFieldMinLabel(_Value);
            }
            else
            {
                graph.editorUtilities.PropertyFieldMinLabel(_OffsetX);
                graph.editorUtilities.PropertyFieldMinLabel(_OffsetY);
            }
            graph.editorUtilities.PropertyFieldMinLabel(_Operation);
        }
    }
}
