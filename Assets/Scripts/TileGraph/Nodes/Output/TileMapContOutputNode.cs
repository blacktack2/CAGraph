using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Output node for <paramref name="TileMapCont" />. </summary>
    [CreateNodeMenu("Output/TileMap/Continuous", 50)]
    public class TileMapContOutputNode : OutputNode<Types.TileMapCont>
    {
        private void Reset()
        {
            name = "Output Tile-Map Continuous";
        }
    }
}
