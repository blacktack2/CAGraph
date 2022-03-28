using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for applying erosion to a
    /// <paramref name="TileMapCont" />. </summary>
    [CreateNodeMenu("Operations/TileMap/Cont/Erosion", 5)]
    public class TileMapContErosionNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField]
        private float _ThermalErosionTimeScale = 1f, _ThermalErosionRate = 0.15f,
            _TalusAngleTangentCoeff = 0.8f, _TalusAngleTangentBias = 0.1f,
            _SedimentCapacity = 1f, _MaxErosionDepth = 10f,
            _SuspensionRate = 0.5f, _DepositionRate = 1f, _SedimentSofteningRate = 5f,
            _DeltaTime = 1f, _PipeArea = 20f, _PipeLength = 1f / 256f,
            _Evaporation = 0.015f, _RainRate = 0.012f;
        [SerializeField]
        private Vector2 _CellScale = Vector2.one;
        [SerializeField, Min(0)]
        private int _Iterations = 1;

        
        private float _CurrentThermalErosionTimeScale = 1f, _CurrentThermalErosionRate = 0.15f,
            _CurrentTalusAngleTangentCoeff = 0.8f, _CurrentTalusAngleTangentBias = 0.1f,
            _CurrentSedimentCapacity = 1f, _CurrentMaxErosionDepth = 10f,
            _CurrentSuspensionRate = 0.5f, _CurrentDepositionRate = 1f, _CurrentSedimentSofteningRate = 5f,
            _CurrentDeltaTime = 1f, _CurrentPipeArea = 20f, _CurrentPipeLength = 1f / 256f,
            _CurrentEvaporation = 0.015f, _CurrentRainRate = 0.012f;
        private Vector2 _CurrentCellScale = Vector2.one;
        private int _CurrentIterations = 1;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Erosion";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                       _CurrentThermalErosionTimeScale != _ThermalErosionTimeScale || _CurrentThermalErosionRate != _ThermalErosionRate
                    || _CurrentTalusAngleTangentCoeff != _TalusAngleTangentCoeff || _CurrentTalusAngleTangentBias != _TalusAngleTangentBias
                    || _CurrentSedimentCapacity != _SedimentCapacity || _CurrentMaxErosionDepth != _MaxErosionDepth
                    || _CurrentSuspensionRate != _SuspensionRate || _CurrentDepositionRate != _DepositionRate || _CurrentSedimentSofteningRate != _SedimentSofteningRate
                    || _CurrentDeltaTime != _DeltaTime || _CurrentPipeArea != _PipeArea || _CurrentPipeLength != _PipeLength
                    || _CurrentEvaporation != _Evaporation || _CurrentRainRate != _RainRate
                    || _CurrentCellScale != _CellScale || _CurrentIterations != _Iterations
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentThermalErosionTimeScale = _ThermalErosionTimeScale;
                _CurrentThermalErosionRate = _ThermalErosionRate;

                _CurrentTalusAngleTangentCoeff = _TalusAngleTangentCoeff;
                _CurrentTalusAngleTangentBias = _TalusAngleTangentBias;

                _CurrentSedimentCapacity = _SedimentCapacity;
                _CurrentMaxErosionDepth = _MaxErosionDepth;

                _CurrentSuspensionRate = _SuspensionRate;
                _CurrentDepositionRate = _DepositionRate;
                _CurrentSedimentSofteningRate = _SedimentSofteningRate;

                _CurrentDeltaTime = _DeltaTime;
                _CurrentPipeArea = _PipeArea;
                _CurrentPipeLength = _PipeLength;

                _CurrentEvaporation = _Evaporation;
                _CurrentRainRate = _RainRate;

                _CurrentCellScale = _CellScale;
                _CurrentIterations = _Iterations;

                _Graph.functionLibrary.erosion.HydraulicErosion(_TileMapOutBuffer,
                    _SedimentCapacity, _MaxErosionDepth,
                    _SuspensionRate, _DepositionRate, _SedimentSofteningRate,
                    _DeltaTime, _PipeArea, _PipeLength,
                    _Evaporation, _RainRate,
                    _ThermalErosionTimeScale, _ThermalErosionRate,
                    _TalusAngleTangentCoeff, _TalusAngleTangentBias,
                    _CellScale, _Iterations, GPUEnabled);
            }
        }
    }
}
