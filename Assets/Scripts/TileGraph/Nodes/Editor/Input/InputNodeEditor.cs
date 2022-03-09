using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    public abstract class InputNodeEditor<NodeT> : InputOutputNodeEditor<NodeT> where NodeT : Nodes.BaseNode, Nodes.IInputOutputNode
    {
        protected override void NodeBodyGUI()
        {
            base.NodeBodyGUI();
            graph.editorUtilities.PropertyFieldMinLabel(_Value, new GUIContent("value"));
        }
    }
}
