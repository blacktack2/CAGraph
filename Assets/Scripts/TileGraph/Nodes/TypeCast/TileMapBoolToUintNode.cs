using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Type cast node for converting from Bool to Uint. </summary>
    [CreateNodeMenu("Convert/Boolean-Integer", 0)]
    public class TileMapBoolToUintNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapBool _TileMapIn;
        [SerializeField, Output] private Types.TileMapUint _TileMapOut;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapUint _TileMapOutBuffer;

        private void Reset()
        {
            name = "Tile-Map (Boolean-Integer)";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput<Types.TileMapBool, Types.TileMapUint>(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    false
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            Types.TileMapBool matrixIn = GetInputValue<Types.TileMapBool>("_TileMapIn");
            _TileMapOutBuffer = Utilities.TileMapOperations.CastBoolToUint(matrixIn);
        }
    }
}
