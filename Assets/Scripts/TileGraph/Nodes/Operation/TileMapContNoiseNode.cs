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

        [SerializeField, Range(1, 20)]
        private float _Detail = 0f;

        [SerializeField, Range(1, 20)]
        private uint _Octaves = 1;
        [SerializeField]
        private float[] _Lacunarity = {2f, 4f, 8f, 16f, 32f, 64f, 128f, 256f, 512f, 1024f, 2048f, 4096f, 8192f, 16384f, 32768f, 65536f, 131072f, 262144f, 524288f, 1048576f};
        [SerializeField]
        private float[] _Persistence = {0.5f, 0.25f, 0.125f, 0.0625f, 0.03125f, 0.015625f, 0.0078125f, 0.00390625f, 0.001953125f, 0.0009765625f, 0.00048828125f, 0.000244140625f, 0.0001220703125f, 6.103515625e-05f, 3.0517578125e-05f, 1.52587890625e-05f, 7.62939453125e-06f, 3.814697265625e-06f, 1.9073486328125e-06f, 9.5367431640625e-07f};

        [SerializeField]
        private bool _Advanced = false;

        private float _CurrentNoiseScaleX = 0f, _CurrentNoiseScaleY = 0f;
        private float _CurrentOffsetX = 0f, _CurrentOffsetY = 0f;
        private bool _CurrentRelativeScale = false;
        private bool _CurrentAdvanced = false;
        private float _CurrentDetail = 0f;
        private uint _CurrentOctaves = 1;
        private float[] _CurrentLacunarity = new float[20], _CurrentPersistence = new float[20];

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
                    ParameterChanged()
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
                _CurrentAdvanced = _Advanced;
                if (_Advanced)
                {
                    _CurrentOctaves = _Octaves;
                    for (int i = 0; i < _Lacunarity.Length; i++)
                    {
                        _CurrentLacunarity[i] = _Lacunarity[i];
                        _CurrentPersistence[i] = _Persistence[i];
                    }
                }
                else
                {
                    _CurrentDetail = _Detail;
                    _CurrentOctaves = (uint) Mathf.Max(0, Mathf.FloorToInt(_Detail)) + 1;
                    float amplitude = 1f;
                    float frequency = 1f;
                    for (int o = 0; o < _CurrentOctaves - 1; o++)
                    {
                        amplitude *= 0.5f;
                        frequency *= 2f;
                        _CurrentPersistence[o] = amplitude;
                        _CurrentLacunarity[o] = frequency;
                    }
                    float t = _Detail - _CurrentOctaves;
                    _CurrentPersistence[_CurrentOctaves - 1] = amplitude * t;
                    _CurrentLacunarity[_CurrentOctaves - 1] = frequency * t;
                }

                Vector2 magnitude;
                if (_RelativeScale)
                    magnitude = new Vector2(_NoiseScaleX / _TileMapOutBuffer.width, _NoiseScaleY / _TileMapOutBuffer.height);
                else
                    magnitude = new Vector2(_NoiseScaleX, _NoiseScaleY);

                _Graph.functionLibrary.noise.PerlinNoise2D(_TileMapOutBuffer, magnitude, new Vector2(_OffsetX, _OffsetY),
                                                           _CurrentOctaves, _CurrentLacunarity, _CurrentPersistence, GPUEnabled);
            }
        }

        private bool ParameterChanged()
        {
            if (_CurrentNoiseScaleX != _NoiseScaleX || _CurrentNoiseScaleY != _NoiseScaleY
                || _CurrentOffsetX != _OffsetX || _CurrentOffsetY != _OffsetY
                || _CurrentRelativeScale != _RelativeScale
                || _CurrentAdvanced != _Advanced)
                return true;
            if (_Advanced)
            {
                if (_CurrentOctaves != _Octaves)
                    return true;
                for (int i = 0; i < _Lacunarity.Length; i++)
                {
                    if (_CurrentLacunarity[i] != _Lacunarity[i] || _CurrentPersistence[i] != _Persistence[i])
                        return true;
                }
            }
            else
            {
                return _CurrentDetail != _Detail;
            }
            return false;
        }
    }
}
