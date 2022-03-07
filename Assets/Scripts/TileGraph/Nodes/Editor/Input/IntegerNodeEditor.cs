using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.IntegerNode))]
    public class IntegerNodeEditor : InputNodeEditor<Nodes.IntegerNode>
    {
    }
}
