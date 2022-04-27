using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Operations/TileMap/Cont/Math", 10)]
    public class TileMapContMathNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapCont _TileMapIn;
        [SerializeField, Input] private Types.TileMapCont _TileMapBIn;
        [SerializeField, Output] private Types.TileMapCont _TileMapOut;

        [SerializeField]
        private float _Value = 0f;

        [SerializeField]
        private int _OffsetX = 0;
        [SerializeField]
        private int _OffsetY = 0;

        public enum Operation
        {
            Add, Subtract,
            Multiply, Divide
        }
        [SerializeField, NodeEnum]
        private Operation _Operation = Operation.Add;

        private float _CurrentValue = 0f;
        private int _CurrentOffsetX = 0, _CurrentOffsetY = 0;
        private Operation _CurrentOperation = Operation.Add;

        private long _TileMapInIDBuffer = 0L;
        private long _TileMapBInIDBuffer = 0L;
        private Types.TileMapCont _TileMapOutBuffer;

        private void Reset()
        {
            name = "Node";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                if (GetInputPort("_TileMapBIn").ConnectionCount == 0)
                {
                    GetTileMapInput(
                        "_TileMapIn", "_TileMapOut",
                        ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                        _CurrentValue != _Value || _CurrentOperation != _Operation
                    );
                }
                else
                {
                    GetTileMapInput(
                        "_TileMapIn", "_TileMapBIn", "_TileMapOut",
                        ref _TileMapOutBuffer, ref _TileMapInIDBuffer, ref _TileMapBInIDBuffer,
                        _CurrentOperation != _Operation || _CurrentOffsetX != _OffsetX || _CurrentOffsetY != _OffsetY
                    );
                }
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentOperation = _Operation;
                if (GetInputPort("_TileMapBIn").ConnectionCount == 0)
                {
                    switch (_Operation)
                    {
                        case Operation.Add:
                            _Graph.functionLibrary.tileMapOperations.TileMapAdd(_TileMapOutBuffer, _Value);
                            break;
                        case Operation.Subtract:
                            _Graph.functionLibrary.tileMapOperations.TileMapSubtract(_TileMapOutBuffer, _Value);
                            break;
                        case Operation.Multiply:
                            _Graph.functionLibrary.tileMapOperations.TileMapMultiply(_TileMapOutBuffer, _Value);
                            break;
                        case Operation.Divide:
                            _Graph.functionLibrary.tileMapOperations.TileMapDivide(_TileMapOutBuffer, _Value);
                            break;
                    }
                }
                else
                {
                    _CurrentOffsetX = _OffsetX;
                    _CurrentOffsetY = _OffsetY;
                    switch (_Operation)
                    {
                        case Operation.Add:
                            _Graph.functionLibrary.tileMapOperations.TileMapAdd(ref _TileMapOutBuffer, GetInputValue<Types.TileMapCont>("_TileMapBIn"), _OffsetX, _OffsetY);
                            break;
                        case Operation.Subtract:
                            _Graph.functionLibrary.tileMapOperations.TileMapSubtract(ref _TileMapOutBuffer, GetInputValue<Types.TileMapCont>("_TileMapBIn"), _OffsetX, _OffsetY);
                            break;
                        case Operation.Multiply:
                            _Graph.functionLibrary.tileMapOperations.TileMapMultiply(ref _TileMapOutBuffer, GetInputValue<Types.TileMapCont>("_TileMapBIn"), _OffsetX, _OffsetY);
                            break;
                        case Operation.Divide:
                            _Graph.functionLibrary.tileMapOperations.TileMapDivide(ref _TileMapOutBuffer, GetInputValue<Types.TileMapCont>("_TileMapBIn"), _OffsetX, _OffsetY);
                            break;
                    }
                }
            }
        }
    }
}
