using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

using static TileGraph.Utilities.FunctionLibrary;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Operations/TileMap/Cont/Erosion", 5)]
    public class TileMapContErosionNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField, Range(1, 1000)]
        private int _Iterations = 1;

        [SerializeField, NodeEnum]
        private Erosion.Algorithm _Algorithm = Erosion.Algorithm.Hydraulic;
        [SerializeField]
        private float _TerrainHardness = 1f;
        [SerializeField]
        private float _SedimentHardness = 1f;
        [SerializeField]
        private float _DepositionRate = 1f;
        [SerializeField]
        private float _RainRate = 0.5f;
        [SerializeField]
        private float _RainAmount = 1f;
        [SerializeField]
        private float _MaxSlope = 3.6f;
        [SerializeField]
        private float _ThermalRate = 0.146f;

        private int _CurrentIterations = 1;
        private Erosion.Algorithm _CurrentAlgorithm = Erosion.Algorithm.Hydraulic;
        private float _CurrentTerrainHardness = 1f, _CurrentSedimentHardness = 1f, _CurretnDepositionRate = 1f,
            _CurrentRainRate = 0.5f, _CurrentRainAmount = 1f,
            _CurrentMaxSlope = 3.6f, _CurrentThermalRate = 0.146f;
        
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
                    _CurrentIterations != _Iterations || _CurrentAlgorithm != _Algorithm
                    || _CurrentTerrainHardness != _TerrainHardness || _CurrentSedimentHardness != _SedimentHardness || _CurretnDepositionRate != _DepositionRate
                    || _CurrentRainRate != _RainRate || _CurrentRainAmount != _RainAmount
                    || _CurrentMaxSlope != _MaxSlope || _CurrentThermalRate != _ThermalRate
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentIterations = _Iterations;
                _CurrentAlgorithm = _Algorithm;

                _CurrentTerrainHardness = _TerrainHardness;
                _CurrentSedimentHardness = _SedimentHardness;
                _CurretnDepositionRate = _DepositionRate;

                _CurrentRainRate = _RainRate;
                _CurrentRainAmount = _RainAmount;

                _CurrentMaxSlope = _MaxSlope;
                _CurrentThermalRate = _ThermalRate;
                switch (_Algorithm) 
                {
                    case Erosion.Algorithm.Hydraulic:
                        _Graph.functionLibrary.erosion.Hydraulic(_TileMapOutBuffer, _Iterations,
                            _TerrainHardness, _SedimentHardness, _DepositionRate,
                            _RainRate, _RainAmount,
                            GPUEnabled);
                        break;
                    case Erosion.Algorithm.Fluvial:
                        _Graph.functionLibrary.erosion.Fluvial(_TileMapOutBuffer, _Iterations, GPUEnabled);
                        break;
                    case Erosion.Algorithm.Thermal:
                        _Graph.functionLibrary.erosion.Thermal(_TileMapOutBuffer, _Iterations, _MaxSlope, _ThermalRate, GPUEnabled);
                        break;
                }
            }
        } 
    }
}
