using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for filling a <paramref name="TileMapUint" />
    /// with a single value. </summary>
    [CreateNodeMenu("Operations/TileMap/Integer/Fill", 10)]
    public class TileMapUintFillNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapUint _TileMapIn;
        /// <summary> Value to fill the TileMap with. </summary>
        [SerializeField, Input] private uint _FillValue = 0;
        [SerializeField, Output] private Types.TileMapUint _TileMapOut;

        private uint _CurrentFillValue = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapUint _TileMapOutBuffer;

        private void Reset()
        {
            name = "Fill Tile-Map Integer";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    GetFillValue() != _CurrentFillValue
                );
                return _TileMapOutBuffer;
            }
            return null;
        }
        
        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentFillValue = GetFillValue();
                _Graph.functionLibrary.tileMapOperations.FillTileMap(_TileMapOutBuffer, _CurrentFillValue, GPUEnabled);
            }
        }

        private uint GetFillValue()
        {
            return GetInputValue<uint>("_FillValue", _FillValue);
        }
    }
}
