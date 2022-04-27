using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Operations/TileMap/Cont/Clamp", 10)]
    public class TileMapContClampNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;
        [SerializeField, Range(0f, 1f)]
        private float _Min = 0f;
        [SerializeField, Range(0f, 1f)]
        private float _Max = 1f;

        private float _CurrentMin = 0f, _CurrentMax = 1f;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

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
