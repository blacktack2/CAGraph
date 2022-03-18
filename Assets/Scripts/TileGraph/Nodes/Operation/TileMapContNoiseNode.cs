using UnityEngine;
using XNode;
using static TileGraph.Utilities.FunctionLibrary.Noise;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for applying gradient noise to a
    /// <paramref name="TileMapCont" />. </summary>
    [CreateNodeMenu("Operations/TileMap/Cont/Noise", 5)]
    public class TileMapContNoiseNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Input] private Vector2 _Frequency = new Vector2(0.1f, 0.1f), _Offset = Vector2.zero;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField]
        private bool _RelativeFrequency = false;

        [SerializeField, NodeEnum]
        private Algorithm _Algorithm = Algorithm.Simplex;

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

        private Vector2 _CurrentFrequency = Vector2.zero, _CurrentOffset = Vector2.zero;
        private bool _CurrentRelativeFrequency = false;
        private Algorithm _CurrentAlgorithm = Algorithm.Simplex;
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
                _CurrentFrequency = _Frequency;
                _CurrentOffset = _Offset;
                _CurrentRelativeFrequency = _RelativeFrequency;
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
                    float a = 1f;
                    float f = 1f;
                    for (int o = 0; o < _CurrentOctaves - 1; o++)
                    {
                        a *= 0.5f;
                        f *= 2f;
                        _CurrentPersistence[o] = a;
                        _CurrentLacunarity[o] = f;
                    }
                    float t = _Detail - _CurrentOctaves;
                    _CurrentPersistence[_CurrentOctaves - 1] = a * t;
                    _CurrentLacunarity[_CurrentOctaves - 1] = f * t;
                }

                _CurrentAlgorithm = _Algorithm;
                switch (_Algorithm)
                {
                    case Algorithm.Perlin:
                        name = "Perlin Noise";
                        break;
                    case Algorithm.Simplex:
                        name = "Simplex Noise";
                        break;
                }
                Vector2 frequency = _Frequency / (_RelativeFrequency ? new Vector2(_TileMapOutBuffer.width, _TileMapOutBuffer.height) : Vector2.one);
                _Graph.functionLibrary.noise.GradientNoise2D(_TileMapOutBuffer, frequency, _Offset,
                                                             _CurrentOctaves, _CurrentLacunarity, _CurrentPersistence,
                                                             _Algorithm, GPUEnabled);
            }
        }

        private bool ParameterChanged()
        {
            if (_CurrentFrequency != _Frequency || _CurrentOffset != _Offset
                || _CurrentRelativeFrequency != _RelativeFrequency
                || _CurrentAlgorithm != _Algorithm
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
