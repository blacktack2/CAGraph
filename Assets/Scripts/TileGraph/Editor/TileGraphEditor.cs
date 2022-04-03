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
            TileGraph graph = target as TileGraph;
            window.titleContent = new GUIContent(graph.name);
        }

        public override void OnDropObjects(UnityEngine.Object[] objects)
        {
            foreach(Object o in objects)
            {
                if (o.GetType() == typeof(TileGraph))
                {
                    Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
                    
                    Nodes.SubGraphNode node = CreateNode(typeof(Nodes.SubGraphNode), pos) as Nodes.SubGraphNode;

                    // Types.SubGraph subGraph = ScriptableObject.CreateInstance(typeof(Types.SubGraph)) as Types.SubGraph;
                    // subGraph.SetSubGraph(o as TileGraph);
                    node.SetSubGraph(o as TileGraph);

                    NodeEditorWindow.current.AutoConnect(node);
                }
                else
                {
                    Debug.Log("OnDrop not supported for type " + o.GetType());
                }
            }
        }
    }
}
