using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for randomizing the values of a
    /// <paramref name="TileMapUint" />.</summary>
    [CreateNodeMenu("Operations/TileMap/Integer/Randomize", 20)]
    public class TileMapUintRandomizeNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapUint _TileMapIn;
        /// <summary> Seed for the random number generator. </summary>
        [SerializeField, Input] private int _Seed = 0;
        /// <summary> Probability of a given cell being a 1. </summary>
        [SerializeField, Input, Min(1)] private uint _Max = 1;
        [SerializeField, Output] private Types.TileMapUint _TileMapOut;

        private int _CurrentSeed = 0;
        private uint _CurrentMax = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapUint _TileMapOutBuffer;

        private void Reset()
        {
            name = "Randomize Tile-Map Integer";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentSeed != GetSeed() || _CurrentMax != GetMax()
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
                _CurrentMax = GetMax();
                _Graph.functionLibrary.tileMapOperations.RandomizeTileMapUint(_TileMapOutBuffer, _CurrentMax, _CurrentSeed);
            }
        }

        private int GetSeed()
        {
            return GetInputValue<int>("_Seed", _Seed);
        }

        private uint GetMax()
        {
            return GetInputValue<uint>("_Max", _Max);
        }

        public void SetSeed(int seed)
        {
            _Seed = seed;
        }
    }
}
