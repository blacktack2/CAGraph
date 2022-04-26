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

        [SerializeField]
        private List<Erosion.ErosionPass> _Passes = new List<Erosion.ErosionPass>();

        private int _CurrentIterations = 1;
        private List<Erosion.ErosionPass> _CurrentPasses = new List<Erosion.ErosionPass>();
        
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
                _CurrentIterations = _Iterations;
                _CurrentPasses = new List<Erosion.ErosionPass>(_Passes);
                _Graph.functionLibrary.erosion.Combined(_TileMapOutBuffer, _Passes, _Iterations, GPUEnabled);
            }
        }

        private bool ParameterChanged()
        {
            if (_CurrentIterations != _Iterations || _CurrentPasses.Count != _Passes.Count)
                return true;

            for (int i = 0; i < _Passes.Count; i++)
                if (!_Passes[i].Equals(_CurrentPasses[i]))
                    return true;

            return false;
        }
    }
}
