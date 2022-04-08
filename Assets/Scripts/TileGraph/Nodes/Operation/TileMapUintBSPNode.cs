using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for applying Binary Space Partitioning
    /// <paramref name="TileMapUint" />. </summary>
    [CreateNodeMenu("Operations/TileMap/Uint/BSP", 5)]
    public class TileMapUintBSPNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapUint _TileMapIn;
        [SerializeField, Input] private int _Seed = 0;
        [SerializeField, Output] private Types.TileMapUint _TileMapOut;

        [SerializeField, Range(float.Epsilon, 1)]
        private float _DivisionChance = 0.5f;
        [SerializeField, Min(1)]
        private int _MinRooms = 1, _MaxRooms = 10, _MinRoomSize = 1, _MaxRoomSize = 50, _MinRoomArea = 1, _MaxRoomArea = 2500;
        [SerializeField, Min(0)]
        private int _MinWallWidth = 1;

        [SerializeField]
        private bool _ShowDebugLines = false;

        private int _CurrentSeed = 0, _CurrentMinRooms = 1, _CurrentMaxRooms = 1,
            _CurrentMinRoomSize = 1, _CurrentMaxRoomSize = 50, _CurrentMinRoomArea = 1, _CurrentMaxRoomArea = 2500,
            _CurrentMinWallWidth = 1;
        private float _CurrentDivisionChance = 0.5f;
        private bool _CurrentShowDebugLines = false;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapUint _TileMapOutBuffer;

        private void Reset()
        {
            name = "Binary-Space-Partitioning";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _Seed != _CurrentSeed
                    || _DivisionChance != _CurrentDivisionChance || _MinRooms != _CurrentMinRooms || _MaxRooms != _CurrentMaxRooms
                    || _MinRoomSize != _CurrentMinRoomSize || _MaxRoomSize != _CurrentMaxRoomSize
                    || _MinRoomArea != _CurrentMinRoomArea || _MaxRoomArea != _CurrentMaxRoomArea
                    || _MinWallWidth != _CurrentMinWallWidth
                    || _ShowDebugLines != _CurrentShowDebugLines
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentSeed = _Seed;
                _CurrentDivisionChance = _DivisionChance;
                _CurrentMinRooms = _MinRooms;
                _CurrentMaxRooms = _MaxRooms;

                _CurrentMinRoomSize = _MinRoomSize;
                _CurrentMaxRoomSize = _MaxRoomSize;
                _CurrentMinRoomArea = _MinRoomArea;
                _CurrentMaxRoomArea = _MaxRoomArea;
                _CurrentShowDebugLines = _ShowDebugLines;

                _CurrentMinWallWidth = _MinWallWidth;
                _Graph.functionLibrary.roguelike.BinarySpacePartitioning(
                    _TileMapOutBuffer,
                    _Seed, _DivisionChance,
                    _MinRooms, _MaxRooms,
                    _MinRoomSize, _MaxRoomSize,
                    _MinRoomArea, _MaxRoomArea,
                    _MinWallWidth,
                    _ShowDebugLines
                );
            }
        }
    }
}
