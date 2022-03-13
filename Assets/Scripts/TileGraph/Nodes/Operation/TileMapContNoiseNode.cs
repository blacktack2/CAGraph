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
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

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
                    false
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _Graph.functionLibrary.noise.PerlinNoise2D(_TileMapOutBuffer, GPUEnabled);
            }
        }
    }
}
