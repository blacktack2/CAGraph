using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Convert/Continuous-Boolean", 0)]
    public class TileMapContToBoolNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Input, Range(0, 1)] private float _Threshold = 0.5f;
        [SerializeField, Output] private Types.TileMapBool _TileMapOut;

        private float _CurrentThreshold = 0f;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapBool _TileMapOutBuffer;

        private void Reset()
        {
            name = "Tile-Map (Continuous-Boolean)";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput<Types.TileMapCont, Types.TileMapBool>(
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
            Types.TileMapCont matrixIn = GetInputValue<Types.TileMapCont>("_TileMapIn");
            _TileMapOutBuffer = Utilities.TileMapOperations.CastContToBool(matrixIn, _Threshold);
        }
    }
}
