using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for randomizing the values of a
    /// <paramref name="TileMapBool" />.</summary>
    [CreateNodeMenu("Operations/TileMap/Bool/Randomize", 20)]
    public class TileMapBoolRandomizeNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapBool _TileMapIn;
        /// <summary> Seed for the random number generator. </summary>
        [SerializeField, Input] private int _Seed = 0;
        /// <summary> Probability of a given cell being a 1. </summary>
        [SerializeField, Input, Range(0f, 1f)] private float _Chance = 0.5f;
        [SerializeField, Output] private Types.TileMapBool _TileMapOut;

        private int _CurrentSeed = 0;
        private float _CurrentChance = 0f;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapBool _TileMapOutBuffer;

        private void Reset()
        {
            name = "Randomize Tile-Map Bool";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _CurrentSeed != GetSeed() || _CurrentChance != GetChance()
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
                _CurrentChance = GetChance();
                _Graph.functionLibrary.tileMapOperations.RandomizeTileMapBool(_TileMapOutBuffer, _CurrentChance, _CurrentSeed);
            }
        }

        private int GetSeed()
        {
            return GetInputValue<int>("_Seed", _Seed);
        }

        private float GetChance()
        {
            return GetInputValue<float>("_Chance", _Chance);
        }

        public void SetSeed(int seed)
        {
            _Seed = seed;
        }
    }
}
