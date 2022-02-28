using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for filling a <paramref name="TileMap" /> with
    /// a single value. </summary>
    [CreateNodeMenu("Operations/TileMap/Fill", 10)]
    public class TileMapFillNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapBool _TileMapIn;
        /// <summary> Value to fill the TileMap with. </summary>
        [SerializeField, Input] private int _FillValue = 0;
        [SerializeField, Output] private Types.TileMapBool _TileMapOut;

        private int _CurrentFillValue = -1;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapBool _TileMapOutBuffer;

        private void Reset()
        {
            name = "Fill Tile-Map";
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
            else if (port.fieldName == "_FillValue")
            {
                return GetFillValue();
            }
            return null;
        }
        
        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentFillValue = GetFillValue();
                Utilities.TileMapOperations.FillTileMap(_TileMapOutBuffer, _CurrentFillValue);
            }
        }

        private int GetFillValue()
        {
            return GetInputValue<int>("_FillValue", _FillValue);
        }
    }
}
