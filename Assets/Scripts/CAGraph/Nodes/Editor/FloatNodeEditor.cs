using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.FloatNode))]
    public class FloatNodeEditor : BaseNodeEditor<Nodes.FloatNode>
    {
        private SerializedProperty _FloatOut, _FloatOutBuffer;

        protected override void OnNodeEnable()
        {
            _FloatOut       = serializedObject.FindProperty("_FloatOut");
            _FloatOutBuffer = serializedObject.FindProperty("_FloatOutBuffer");
        }

        protected override void NodeInputGUI()
        {
            graph.CAEditorUtilities.PortFieldMinLabel(_FloatOut);
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_FloatOutBuffer, new GUIContent("value"));
        }
    }
}
