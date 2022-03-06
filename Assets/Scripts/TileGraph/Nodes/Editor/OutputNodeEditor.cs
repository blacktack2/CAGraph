using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    public abstract class OutputNodeEditor<OutT> : BaseNodeEditor<OutT> where OutT : Nodes.BaseNode
    {
        private SerializedProperty _In, _OutputName;

        protected override void OnNodeEnable()
        {
            _In         = serializedObject.FindProperty("_In");
            _OutputName = serializedObject.FindProperty("_OutputName");
        }

        protected override void NodeInputGUI()
        {
            graph.CAEditorUtilities.PortFieldMinLabel(_In);
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_OutputName);

            serializedObject.ApplyModifiedProperties();

            _OutputName.stringValue = graph.CheckOutputName(_OutputName.stringValue, (Nodes.IOutputNode) _Node);
        }
    }
}
