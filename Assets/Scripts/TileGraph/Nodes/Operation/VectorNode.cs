using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    [CreateNodeMenu("Math/Vector", 5)]
    public class VectorNode : BaseNode
    {
        [SerializeField, Input] private float _X = 0f;
        [SerializeField, Input] private float _Y = 0f;
        [SerializeField, Input] private float _Z = 0f;

        public enum Dimension { Vec2, Vec3 }
        [SerializeField, NodeEnum]
        private Dimension _Dimension = Dimension.Vec2;

        private Dimension _CurrentDimension = Dimension.Vec2;

        public NodePort vector2Port = null, vector3Port = null;

        private void Reset()
        {
            name = "Vector";
        }

        protected override void Init()
        {
            base.Init();
            vector2Port = AddDynamicOutput(typeof(Vector2), ConnectionType.Override, fieldName: "_Vector2");
            UpdateDynamicPorts();
        }

        public override object GetValue(NodePort port)
        {
            UpdateDynamicPorts();
            if (port.fieldName == "_Vector2")
            {
                return new Vector2(GetInputValue<float>("_X", 0), GetInputValue<float>("_Y", 0));
            }
            else if (port.fieldName == "_Vector3")
            {
                return new Vector3(GetInputValue<float>("_X", 0), GetInputValue<float>("_Y", 0), GetInputValue<float>("_Z", 0));
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
                    case Dimension.Vec2:
                        GetInputPort("_Z").ClearConnections();

                        vector3Port = null;
                        RemoveDynamicPort("_Vector3");

                        vector2Port = AddDynamicOutput(typeof(Vector2), ConnectionType.Override, fieldName: "_Vector2");
                        break;
                    case Dimension.Vec3:
                        vector2Port = null;
                        RemoveDynamicPort("_Vector2");

                        vector3Port = AddDynamicOutput(typeof(Vector3), ConnectionType.Override, fieldName: "_Vector3");
                        break;
                }
            }
        }
    }
}
