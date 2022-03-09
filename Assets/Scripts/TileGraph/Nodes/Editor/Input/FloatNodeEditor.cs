using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
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
            graph.editorUtilities.PortFieldMinLabel(_FloatOut);
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_FloatOut, new GUIContent("value"));
        }
    }
}
