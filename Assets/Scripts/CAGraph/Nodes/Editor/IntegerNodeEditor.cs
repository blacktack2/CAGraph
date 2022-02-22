using UnityEditor;
using UnityEngine;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.IntegerNode))]
    public class IntegerNodeEditor : BaseNodeEditor<Nodes.IntegerNode>
    {
        private SerializedProperty _IntegerOut;

        protected override void OnNodeEnable()
        {
            _IntegerOut = serializedObject.FindProperty("_IntegerOut");
        }

        protected override void NodeInputGUI()
        {
            graph.CAEditorUtilities.PortFieldMinLabel(_IntegerOut);
        }

        protected override void NodeBodyGUI()
        {
            graph.CAEditorUtilities.PropertyFieldMinLabel(_IntegerOut, new GUIContent("value"));
        }
    }
}
