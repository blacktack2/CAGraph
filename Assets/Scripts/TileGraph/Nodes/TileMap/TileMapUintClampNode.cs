using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Operations/TileMap/Integer/Clamp", 10)]
    public class TileMapUintClampNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapUint _TileMapIn;
        [SerializeField, Output] private Types.TileMapUint _TileMapOut;
        [SerializeField]
        private uint _Min = 0;
        [SerializeField]
        private uint _Max = 1;

        private uint _CurrentMin = 0, _CurrentMax = 1;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapUint _TileMapOutBuffer;

        private void Reset()
        {
            name = "Clamp";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentMin != _Min || _CurrentMax != _Max
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentMin = _Min;
                _CurrentMax = _Max;
                _Graph.functionLibrary.tileMapOperations.ClampTileMap(_TileMapOutBuffer, _Min, _Max);
            }
        }
    }
}
