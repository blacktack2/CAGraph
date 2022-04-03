using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for creating a sub-graph. </summary>
    [CreateNodeMenu("Operations/Sub-Graph", 5)]
    public class SubGraphNode : BaseNode
    {
        [SerializeField]
        private TileGraph _SubGraph;
        public TileGraph subGraph {get {return _SubGraph;}}

        private List<NodePort> _DynamicInputPorts  = new List<NodePort>();
        public  List<NodePort> dynamicInputPorts {get {return _DynamicInputPorts;}}
        private List<NodePort> _DynamicOutputPorts = new List<NodePort>();
        public  List<NodePort> dynamicOutputPorts {get {return _DynamicOutputPorts;}}

        private int _CurrentSubGraphID = -1;

        private void Reset()
        {
            name = "Sub-Graph";
        }

        protected override void Init()
        {
            base.Init();
            _CurrentSubGraphID = -1;
            CheckForChange();
        }

        public override object GetValue(NodePort port)
        {
            if (_SubGraph == null)
                return null;
            
            int outputIndex = _DynamicOutputPorts.IndexOf(port);
            if (outputIndex == -1)
                return null;
            
            UpdateDynamicPorts();
            // Set the inputs
            for (int i = 0; i < _DynamicInputPorts.Count; i++)
            {
                NodePort inputPort = _DynamicInputPorts[i];
                MethodInfo inputInfo = typeof(TileGraph).GetMethod("SetInputValue");
                MethodInfo inputReference = inputInfo.MakeGenericMethod(inputPort.ValueType);
                inputReference.Invoke(_SubGraph, new object[] {inputPort.fieldName, inputPort.GetInputValue()});
            }

            // Get the output value
            MethodInfo outputInfo = typeof(TileGraph).GetMethod("GetOutputValue");
            MethodInfo outputReference = outputInfo.MakeGenericMethod(_DynamicOutputPorts[outputIndex].ValueType);
            return outputReference.Invoke(_SubGraph, new object[] {port.fieldName});
        }

        public void SetSubGraph(TileGraph subGraph)
        {
            _SubGraph = subGraph;
        }

        private void UpdateDynamicPorts()
        {
            List<GraphPort> inputNodes = _SubGraph.GetInputNodes();
            List<NodePort> newInputPorts = new List<NodePort>();
            for (int i = 0; i < inputNodes.Count; i++)
            {
                GraphPort newPort = inputNodes[i];
                NodePort oldPort = GetInputPort(newPort.portName);
                if (oldPort == null)
                {
                    newInputPorts.Add(AddDynamicInput(newPort.portType, fieldName: newPort.portName));
                }
                else if (oldPort.ValueType != newPort.portType)
                {
                    RemoveDynamicPort(oldPort.fieldName);
                    newInputPorts.Add(AddDynamicInput(newPort.portType, fieldName: newPort.portName));
                }
                else
                {
                    newInputPorts.Add(oldPort);
                }
            }
            _DynamicInputPorts = newInputPorts;

            List<GraphPort> outputNodes = _SubGraph.GetOutputNodes();
            List<NodePort> newOutputPorts = new List<NodePort>();
            for (int i = 0; i < outputNodes.Count; i++)
            {
                GraphPort newPort = outputNodes[i];
                NodePort oldPort = GetOutputPort(newPort.portName);
                if (oldPort == null)
                {
                    newOutputPorts.Add(AddDynamicOutput(newPort.portType, fieldName: newPort.portName));
                }
                else if (oldPort.ValueType != newPort.portType)
                {
                    RemoveDynamicPort(oldPort.fieldName);
                    newOutputPorts.Add(AddDynamicOutput(newPort.portType, fieldName: newPort.portName));
                }
                else
                {
                    newOutputPorts.Add(oldPort);
                }
            }
            _DynamicOutputPorts = newOutputPorts;
        }

        public void CheckForChange()
        {
            if (_SubGraph == null)
            {
                _CurrentSubGraphID = -1;
                ClearDynamicPorts();
            }
            else if (_CurrentSubGraphID != _SubGraph.id)
            {
                if (_SubGraph.id == _Graph.id)
                {
                    _SubGraph = null;
                    _CurrentSubGraphID = -1;
                    ClearDynamicPorts();
                    Debug.LogWarning("Cannot add a graph to itself");
                }
                else
                {
                    _CurrentSubGraphID = _SubGraph.id;
                    UpdateDynamicPorts();
                }
            }
        }
    }
}
