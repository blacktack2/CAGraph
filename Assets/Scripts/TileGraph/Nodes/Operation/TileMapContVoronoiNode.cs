using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for applying voronoi noise to a
    /// <paramref name="TileMapCont" />. </summary>
    [CreateNodeMenu("Operations/TileMap/Cont/Voronoi", 5)]
    public class TileMapContVoronoiNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Input] private Vector2 _Frequency = new Vector2(0.1f, 0.1f), _Offset = Vector2.zero;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField]
        private bool _RelativeFrequency = false;

        private Vector2 _CurrentFrequency = Vector2.zero, _CurrentOffset = Vector2.zero;
        private bool _CurrentRelativeFrequency = false;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Voronoi Noise";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentFrequency != _Frequency || _CurrentOffset != _Offset
                    || _CurrentRelativeFrequency != _RelativeFrequency
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentFrequency = _Frequency;
                _CurrentOffset = _Offset;
                _CurrentRelativeFrequency = _RelativeFrequency;

                Vector2 frequency = _Frequency / (_RelativeFrequency ? new Vector2(_TileMapOutBuffer.width, _TileMapOutBuffer.height) : Vector2.one);
                _Graph.functionLibrary.noise.VoronoiNoise2D(_TileMapOutBuffer, frequency, _Offset, GPUEnabled);
            }
        }
    }
}
