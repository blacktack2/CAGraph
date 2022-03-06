using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for randomizing the values of a
    /// <paramref name="TileMapCont" />.</summary>
    [CreateNodeMenu("Operations/TileMap/Continuous/Randomize", 20)]
    public class TileMapContRandomizeNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        /// <summary> Seed for the random number generator. </summary>
        [SerializeField, Input] private int _Seed = 0;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        private int _CurrentSeed = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Randomize Tile-Map Continuous";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentSeed != GetSeed()
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentSeed = GetSeed();
                Utilities.TileMapOperations.RandomizeTileMapCont(_TileMapOutBuffer, _CurrentSeed);
            }
        }

        private int GetSeed()
        {
            return GetInputValue<int>("_Seed", _Seed);
        }

        public void SetSeed(int seed)
        {
            _Seed = seed;
        }
    }
}
