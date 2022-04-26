using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.VectorNode))]
    public class VectorNodeEditor : BaseNodeEditor<Nodes.VectorNode>
    {
        private SerializedProperty _X, _Y, _Z, _Dimension;

        protected override void OnNodeEnable()
        {
            _X = serializedObject.FindProperty("_X");
            _Y = serializedObject.FindProperty("_Y");
            _Z = serializedObject.FindProperty("_Z");

            _Dimension = serializedObject.FindProperty("_Dimension");
        }

        protected override void NodeInputGUI()
        {
            switch (_Dimension.enumValueIndex)
            {
                case (int) Nodes.VectorNode.Dimension.Vec2:
                    graph.editorUtilities.PortFieldMinLabel(_Node.vector2Port);
                    graph.editorUtilities.PortFieldMinLabel(_X);
                    graph.editorUtilities.PortFieldMinLabel(_Y);
                    break;
                case (int) Nodes.VectorNode.Dimension.Vec3:
                    graph.editorUtilities.PortFieldMinLabel(_Node.vector3Port);
                    graph.editorUtilities.PortFieldMinLabel(_X);
                    graph.editorUtilities.PortFieldMinLabel(_Y);
                    graph.editorUtilities.PortFieldMinLabel(_Z);
                    break;
            }
        }

        protected override void NodeBodyGUI()
        {
            int prev = _Dimension.enumValueIndex;
            graph.editorUtilities.PropertyFieldMinLabel(_Dimension);

            serializedObject.ApplyModifiedProperties();
            _Node.UpdateDynamicPorts();
        }
    }
}
