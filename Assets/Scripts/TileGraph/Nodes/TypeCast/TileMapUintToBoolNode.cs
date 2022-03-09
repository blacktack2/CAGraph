using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Type cast node for converting from Uint to Bool. </summary>
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
                    _CurrentThreshold != GetThreshold()
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentThreshold = GetThreshold();
                Types.TileMapUint matrixIn = GetInputValue<Types.TileMapUint>("_TileMapIn");
                _TileMapOutBuffer = _Graph.functionLibrary.tileMapCast.CastUintToBool(matrixIn, _Threshold);
            }
        }

        private uint GetThreshold()
        {
            return GetInputValue<uint>("_Threshold", _Threshold);
        }
    }
}
