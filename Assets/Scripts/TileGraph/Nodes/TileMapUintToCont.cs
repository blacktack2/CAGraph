using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Convert/Integer-Continuous", 0)]
    public class TileMapUintToContNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapUint _TileMapIn;
        [SerializeField, Input] private uint _Max = 1;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        private uint _CurrentMax = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Tile-Map (Integer-Continuous)";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput<Types.TileMapUint, Types.TileMapCont>(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentMax != GetMax()
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentMax = GetMax();
                Types.TileMapUint matrixIn = GetInputValue<Types.TileMapUint>("_TileMapIn");
                _TileMapOutBuffer = Utilities.TileMapOperations.CastUintToCont(matrixIn, _Max);
            }
        }

        private uint GetMax()
        {
            return GetInputValue<uint>("_Max");
        }
    }
}
