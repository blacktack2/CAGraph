using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

using static TileGraph.Utilities.FunctionLibrary.Erosion;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Operations/TileMap/Cont/Erosion", 5)]
    public class TileMapContErosionNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField, Range(1, 10000)]
        private int _Iterations = 1;

        [SerializeField, NodeEnum]
        private Hydraulic.Algorithm _Algorithm = Hydraulic.Algorithm.PoorErosion;

        private int _CurrentIterations = 1;
        private Hydraulic.Algorithm _CurrentAlgorithm = Hydraulic.Algorithm.PoorErosion;
        
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
                switch (_Algorithm)
                {
                    case Hydraulic.Algorithm.PoorErosion:
                        _Graph.functionLibrary.erosion.hydraulic.Poor(_TileMapOutBuffer, _Iterations, GPUEnabled);
                        break;
                    case Hydraulic.Algorithm.StreamPowerLaw:
                        _Graph.functionLibrary.erosion.hydraulic.StreamPowerLaw(_TileMapOutBuffer, _Iterations, GPUEnabled);
                        break;
                }
            }
        } 
    }
}
