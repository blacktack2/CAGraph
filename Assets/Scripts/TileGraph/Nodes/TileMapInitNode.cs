using System;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Constant node representing a TileMap input. </summary>
    [CreateNodeMenu("Constants/TileMap", 0)]
    public class TileMapInitNode : BaseNode
    {
        [SerializeField, Output] private Types.TileMapBool _TileMapBoolOut;
        [SerializeField, Output] private Types.TileMapCont _TileMapContOut;
        [SerializeField, Output] private Types.TileMapUint _TileMapUintOut;

        /// <summary> Bounds of the TileMap being initialized. </summary>
        [SerializeField, Range(2, Types.TileMapBool.maxTileMapSize)]
        private int _TileMapWidth = 100, _TileMapHeight = 100;

        public enum TileMapType { Boolean, Continuous, Integer }
        /// <summary> Which implementation of TileMap to output. </summary>
        [SerializeField, NodeEnum]
        private TileMapType _TileMapType = TileMapType.Boolean;

        private Types.TileMap _TileMapBuffer;

        private void Reset()
        {
            name = "Tile-Map";
        }

        public override object GetValue(NodePort port)
        {
            // Return the TileMap only if the port name matches the expected output type
            if (port.fieldName == "_TileMapBoolOut" && _TileMapType == TileMapType.Boolean ||
                port.fieldName == "_TileMapContOut" && _TileMapType == TileMapType.Continuous ||
                port.fieldName == "_TileMapUintOut" && _TileMapType == TileMapType.Integer)
            {
                UpdateTileMap();
                return _TileMapBuffer;
            }
            return null;
        }

        public void UpdateTileMap()
        {
            switch (_TileMapType)
            {
                case TileMapType.Boolean:
                    if (TileMapChanged<Types.TileMapBool>(_TileMapBuffer))
                        _TileMapBuffer = new Types.TileMapBool(_TileMapWidth, _TileMapHeight);
                    break;
                case TileMapType.Continuous:
                    if (TileMapChanged<Types.TileMapCont>(_TileMapBuffer))
                        _TileMapBuffer = new Types.TileMapCont(_TileMapWidth, _TileMapHeight);
                    break;
                case TileMapType.Integer:
                    if (TileMapChanged<Types.TileMapUint>(_TileMapBuffer))
                        _TileMapBuffer = new Types.TileMapUint(_TileMapWidth, _TileMapHeight);
                    break;
            }
        }

        private bool TileMapChanged<T>(Types.TileMap tileMap) where T : Types.TileMap
        {
            return tileMap == null || !(tileMap is T) || _TileMapWidth != tileMap.width || _TileMapHeight != tileMap.height;
        }
    }
}
