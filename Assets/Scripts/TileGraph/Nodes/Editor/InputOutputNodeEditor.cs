using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    public abstract class InputOutputNodeEditor<NodeT> : BaseNodeEditor<NodeT> where NodeT : Nodes.BaseNode, Nodes.IInputOutputNode
    {
        protected SerializedProperty _Value, _Name;

        protected override void OnNodeEnable()
        {
            _Value = serializedObject.FindProperty("_Value");
            _Name  = serializedObject.FindProperty("_Name");
        }

        protected override void NodeInputGUI()
        {
            graph.CAEditorUtilities.PortFieldMinLabel(_Value);
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_Name, new GUIContent("name"));

            serializedObject.ApplyModifiedProperties();

            _Name.stringValue = graph.CheckInOutName(_Name.stringValue, (Nodes.IInputOutputNode) _Node);
        }
    }
}
