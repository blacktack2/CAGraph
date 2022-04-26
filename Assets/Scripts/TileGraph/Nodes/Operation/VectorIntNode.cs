using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Math/VectorInt", 5)]
    public class VectorIntNode : BaseNode
    {
        [SerializeField, Input] private int _X = 0;
        [SerializeField, Input] private int _Y = 0;
        [SerializeField, Input] private int _Z = 0;

        [SerializeField, NodeEnum]
        private VectorNode.Dimension _Dimension = VectorNode.Dimension.Vec2;

        private VectorNode.Dimension _CurrentDimension = VectorNode.Dimension.Vec2;

        public NodePort vector2Port = null, vector3Port = null;

        private void Reset()
        {
            name = "VectorInt";
        }

        protected override void Init()
        {
            base.Init();
            vector2Port = AddDynamicOutput(typeof(Vector2Int), ConnectionType.Override, fieldName: "_Vector2");
            UpdateDynamicPorts();
        }

        public override object GetValue(NodePort port)
        {
            UpdateDynamicPorts();
            if (port.fieldName == "_Vector2")
            {
                return new Vector2(GetInputValue<int>("_X", 0), GetInputValue<int>("_Y", 0));
            }
            else if (port.fieldName == "_Vector3")
            {
                return new Vector3(GetInputValue<int>("_X", 0), GetInputValue<int>("_Y", 0), GetInputValue<int>("_Z", 0));
            }
            return null;
        }

        public void UpdateDynamicPorts()
        {
            if (_CurrentDimension != _Dimension)
            {
                _CurrentDimension = _Dimension;
                switch (_Dimension)
                {
                    case VectorNode.Dimension.Vec2:
                        GetInputPort("_Z").ClearConnections();

                        vector3Port = null;
                        RemoveDynamicPort("_Vector3");

                        vector2Port = AddDynamicOutput(typeof(Vector2Int), ConnectionType.Override, fieldName: "_Vector2");
                        break;
                    case VectorNode.Dimension.Vec3:
                        vector2Port = null;
                        RemoveDynamicPort("_Vector2");

                        vector3Port = AddDynamicOutput(typeof(Vector3Int), ConnectionType.Override, fieldName: "_Vector3");
                        break;
                }
            }
        }
    }
}
