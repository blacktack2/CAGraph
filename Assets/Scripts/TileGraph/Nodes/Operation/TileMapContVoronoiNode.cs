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
        [SerializeField, Input] private float _ScaleX = 0.1f, _ScaleY = 0.1f;
        [SerializeField, Input] private float _OffsetX = 0f, _OffsetY = 0f;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField]
        private bool _RelativeScale = false;

        private float _CurrentScaleX = 0f, _CurrentScaleY = 0f;
        private float _CurrentOffsetX = 0f, _CurrentOffsetY = 0f;
        private bool _CurrentRelativeScale = false;

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
                    _CurrentScaleX != _ScaleX || _CurrentScaleY != _ScaleY
                    || _CurrentOffsetX != _OffsetX || _CurrentOffsetY != _OffsetY
                    || _CurrentRelativeScale != _RelativeScale
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentScaleX = _ScaleX;
                _CurrentScaleY = _ScaleY;
                _CurrentOffsetX = _OffsetX;
                _CurrentOffsetY = _OffsetY;
                _CurrentRelativeScale = _RelativeScale;

                Vector2 magnitude;
                if (_RelativeScale)
                    magnitude = new Vector2(_ScaleX / _TileMapOutBuffer.width, _ScaleY / _TileMapOutBuffer.height);
                else
                    magnitude = new Vector2(_ScaleX, _ScaleY);

                _Graph.functionLibrary.noise.VoronoiNoise2D(_TileMapOutBuffer, magnitude, new Vector2(_OffsetX, _OffsetY), GPUEnabled);
            }
        }
    }
}
