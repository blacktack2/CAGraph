using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Convert/Boolean-Continuous", 0)]
    public class TileMapBoolToContNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapBool _TileMapIn;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Tile-Map (Boolean-Continuous)";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput<Types.TileMapBool, Types.TileMapCont>(
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
            _TileMapOutBuffer = Utilities.TileMapOperations.CastBoolToCont(matrixIn);
        }
    }
}
