using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.IntegerNode))]
    public class IntegerNodeEditor : BaseNodeEditor<Nodes.IntegerNode>
    {
        private SerializedProperty _IntegerOut, _IntegerOutBuffer;

        protected override void OnNodeEnable()
        {
            _IntegerOut       = serializedObject.FindProperty("_IntegerOut");
            _IntegerOutBuffer = serializedObject.FindProperty("_IntegerOutBuffer");
        }

        protected override void NodeInputGUI()
        {
            NodeEditorGUILayout.PropertyField(_IntegerOut);
        }

        protected override void NodeBodyGUI()
        {
            NodeEditorGUILayout.PropertyField(_IntegerOutBuffer, new GUIContent("value"));
        }
    }
}
