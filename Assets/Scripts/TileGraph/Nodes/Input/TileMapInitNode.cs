using System;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Input node representing a TileMap input. </summary>
    [CreateNodeMenu("TileMap", 0)]
    public class TileMapInitNode : BaseNode
    {
        [SerializeField, Input, Range(2, Types.TileMapBool.maxTileMapSize)] private int _TileMapWidth = 100;
        [SerializeField, Input, Range(2, Types.TileMapBool.maxTileMapSize)] private int _TileMapHeight = 100;
        [SerializeField, Output] private Types.TileMapBool _TileMapBoolOut;
        [SerializeField, Output] private Types.TileMapCont _TileMapContOut;
        [SerializeField, Output] private Types.TileMapUint _TileMapUintOut;

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
                        _TileMapBuffer = new Types.TileMapBool(GetTileMapWidth(), GetTileMapHeight());
                    break;
                case TileMapType.Continuous:
                    if (TileMapChanged<Types.TileMapCont>(_TileMapBuffer))
                        _TileMapBuffer = new Types.TileMapCont(GetTileMapWidth(), GetTileMapHeight());
                    break;
                case TileMapType.Integer:
                    if (TileMapChanged<Types.TileMapUint>(_TileMapBuffer))
                        _TileMapBuffer = new Types.TileMapUint(GetTileMapWidth(), GetTileMapHeight());
                    break;
            }
        }
 
        private bool TileMapChanged<T>(Types.TileMap tileMap) where T : Types.TileMap
        {
            return tileMap == null || !(tileMap is T) || GetTileMapWidth() != tileMap.width || GetTileMapHeight() != tileMap.height;
        }

        private int GetTileMapWidth()
        {
            return Mathf.Clamp(GetInputValue<int>("_TileMapWidth", _TileMapWidth), 2, Types.TileMapBool.maxTileMapSize);
        }
        private int GetTileMapHeight()
        {
            return Mathf.Clamp(GetInputValue<int>("_TileMapHeight", _TileMapHeight), 2, Types.TileMapBool.maxTileMapSize);
        }
    }
}
