using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Convert/Integer-Boolean", 0)]
    public class TileMapUintToBoolNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapUint _TileMapIn;
        [SerializeField, Input] private uint _Threshold = 1;
        [SerializeField, Output] private Types.TileMapBool _TileMapOut;

        private uint _CurrentThreshold = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapBool _TileMapOutBuffer;

        private void Reset()
        {
            name = "Tile-Map (Integer-Boolean)";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput<Types.TileMapUint, Types.TileMapBool>(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentThreshold != _Threshold
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            _CurrentThreshold = _Threshold;
            Types.TileMapUint matrixIn = GetInputValue<Types.TileMapUint>("_TileMapIn");
            _TileMapOutBuffer = Utilities.TileMapOperations.CastUintToBool(matrixIn, _Threshold);
        }
    }
}
