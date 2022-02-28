using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for filling a <paramref name="TileMap" /> with
    /// a single value. </summary>
    [CreateNodeMenu("Operations/TileMap/Continuous/Fill", 10)]
    public class TileMapContFillNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        /// <summary> Value to fill the TileMap with. </summary>
        [SerializeField, Input, Range(0, 1)] private float _FillValue = 0;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        private float _CurrentFillValue = 0f;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Fill Tile-Map Continuous";
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

        private float GetFillValue()
        {
            return GetInputValue<float>("_FillValue", _FillValue);
        }
    }
}
