using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Convert/Continuous-Integer", 0)]
    public class TileMapContToUintNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Input] private uint _Max = 1;
        [SerializeField, Output] private Types.TileMapUint _TileMapOut;

        private uint _CurrentMax = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapUint _TileMapOutBuffer;

        private void Reset()
        {
            name = "Tile-Map (Continuous-Integer)";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput<Types.TileMapCont, Types.TileMapUint>(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentMax != _Max
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            _CurrentMax = _Max;
            Types.TileMapCont matrixIn = GetInputValue<Types.TileMapCont>("_TileMapIn");
            _TileMapOutBuffer = Utilities.TileMapOperations.CastContToUint(matrixIn, _Max);
        }
    }
}
