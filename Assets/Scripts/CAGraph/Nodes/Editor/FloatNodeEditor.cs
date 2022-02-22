using UnityEditor;
using UnityEngine;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.FloatNode))]
    public class FloatNodeEditor : BaseNodeEditor<Nodes.FloatNode>
    {
        private SerializedProperty _FloatOut;

        protected override void OnNodeEnable()
        {
            _FloatOut = serializedObject.FindProperty("_FloatOut");
        }

        protected override void NodeInputGUI()
        {
            graph.CAEditorUtilities.PortFieldMinLabel(_FloatOut);
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_FloatOut, new GUIContent("value"));
        }
    }
}
