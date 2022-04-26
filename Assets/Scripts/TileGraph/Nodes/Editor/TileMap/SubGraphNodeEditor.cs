using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.SubGraphNode))]
    public class SubGraphNodeEditor : BaseNodeEditor<Nodes.SubGraphNode>
    {
        private SerializedProperty _SubGraph;

        protected override bool GPUToggleable => false;

        protected override void OnNodeEnable()
        {
            _SubGraph = serializedObject.FindProperty("_SubGraph");
        }

        protected override void NodeInputGUI()
        {
            List<NodePort> dynamicInputPorts  = _Node.dynamicInputPorts;
            List<NodePort> dynamicOutputPorts = _Node.dynamicOutputPorts;

            int numPorts = Mathf.Max(dynamicInputPorts.Count, dynamicOutputPorts.Count);
            for (int i = 0; i < numPorts; i++)
            {
                if (i < dynamicInputPorts.Count)
                    graph.editorUtilities.PortFieldMinLabel(dynamicInputPorts[i]);
                if (i < dynamicOutputPorts.Count)
                    graph.editorUtilities.PortFieldMinLabel(dynamicOutputPorts[i]);
            }
        }

        protected override void NodeBodyGUI()
        {
            EditorGUILayout.ObjectField(_SubGraph, typeof(TileGraph), GUIContent.none);

            serializedObject.ApplyModifiedProperties();
            
            _Node.CheckForChange();
        }
    }
}