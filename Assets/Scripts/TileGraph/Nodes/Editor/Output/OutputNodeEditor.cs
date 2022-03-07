using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    public abstract class OutputNodeEditor<NodeT> : InputOutputNodeEditor<NodeT> where NodeT : Nodes.BaseNode, Nodes.IInputOutputNode
    {
    }
}
