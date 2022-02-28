using UnityEngine;
using XNodeEditor;

namespace TileGraph
{
    [CustomNodeGraphEditor(typeof(TileGraph))]
    public class TileGraphEditor : NodeGraphEditor
    {
        public override void OnOpen()
        {
            base.OnOpen();
            window.titleContent = new GUIContent((target as TileGraph).name);
        }
    }
}
