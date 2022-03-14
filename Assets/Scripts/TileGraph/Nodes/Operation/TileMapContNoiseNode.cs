using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for applying gradient noise to a
    /// <paramref name="TileMapCont" />. </summary>
    [CreateNodeMenu("Operations/TileMap/Cont/Noise", 5)]
    public class TileMapContNoiseNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Input] private float _NoiseScaleX = 0.1f, _NoiseScaleY = 0.1f;
        [SerializeField, Input] private float _OffsetX = 0f, _OffsetY = 0f;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField]
        private bool _RelativeScale = false;

        private float _CurrentNoiseScaleX = 0f, _CurrentNoiseScaleY = 0f;
        private float _CurrentOffsetX = 0f, _CurrentOffsetY = 0f;
        private bool _CurrentRelativeScale = false;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Noise";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentNoiseScaleX != _NoiseScaleX || _CurrentNoiseScaleY != _NoiseScaleY
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
                _CurrentNoiseScaleX = _NoiseScaleX;
                _CurrentNoiseScaleY = _NoiseScaleY;
                _CurrentOffsetX = _OffsetX;
                _CurrentOffsetY = _OffsetY;
                _CurrentRelativeScale = _RelativeScale;

                Vector2 magnitude;
                if (_RelativeScale)
                    magnitude = new Vector2(_NoiseScaleX / _TileMapOutBuffer.width, _NoiseScaleY / _TileMapOutBuffer.height);
                else
                    magnitude = new Vector2(_NoiseScaleX, _NoiseScaleY);

                _Graph.functionLibrary.noise.PerlinNoise2D(_TileMapOutBuffer, magnitude, new Vector2(_OffsetX, _OffsetY), GPUEnabled);
            }
        }
    }
}
