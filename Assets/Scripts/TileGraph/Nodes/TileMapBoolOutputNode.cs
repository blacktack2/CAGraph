using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Output node for <paramref name="TileMapBool" />. </summary>
    [CreateNodeMenu("Output/TileMap/Bool", 50)]
    public class TileMapBoolOutputNode : OutputNode<Types.TileMapBool>
    {
        private void Reset()
        {
            name = "Output Tile-Map Boolean";
        }
    }
}
