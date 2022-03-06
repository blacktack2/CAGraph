using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Output node for <paramref name="TileMapUint" />. </summary>
    [CreateNodeMenu("Output/TileMap/Integer", 50)]
    public class TileMapUintOutputNode : OutputNode<Types.TileMapUint>
    {
        private void Reset()
        {
            name = "Output Tile-Map Continuous";
        }
    }
}
