using UnityEditor;
using UnityEngine;

namespace TileGraph.Editors
{
    [CustomNodeEditor(typeof(Nodes.TileMapUintBSPNode))]
    public class TileMapUintBSPNodeEditor : BaseNodeEditor<Nodes.TileMapUintBSPNode>
    {
        private SerializedProperty _TileMapIn, _Seed,
            _TileMapOut, _DivisionChance, _MinRooms, _MaxRooms,
            _MinRoomSize, _MaxRoomSize, _MinRoomArea, _MaxRoomArea, _MinWallWidth,
            _ShowDebugLines;

        protected override void OnNodeEnable()
        {
            _TileMapIn         = serializedObject.FindProperty("_TileMapIn");
            _Seed              = serializedObject.FindProperty("_Seed");
            _TileMapOut        = serializedObject.FindProperty("_TileMapOut");

            _DivisionChance    = serializedObject.FindProperty("_DivisionChance");
            _MinRooms          = serializedObject.FindProperty("_MinRooms");
            _MaxRooms          = serializedObject.FindProperty("_MaxRooms");

            _MinRoomSize       = serializedObject.FindProperty("_MinRoomSize");
            _MaxRoomSize       = serializedObject.FindProperty("_MaxRoomSize");
            _MinRoomArea       = serializedObject.FindProperty("_MinRoomArea");
            _MaxRoomArea       = serializedObject.FindProperty("_MaxRoomArea");
            _MinWallWidth      = serializedObject.FindProperty("_MinWallWidth");
            _ShowDebugLines    = serializedObject.FindProperty("_ShowDebugLines");

            AddPreview("_TileMapOut");
        }

        protected override void NodeInputGUI()
        {
            EditorGUILayout.BeginHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_TileMapIn);
            graph.editorUtilities.PortFieldMinLabel(_TileMapOut);

            EditorGUILayout.EndHorizontal();

            graph.editorUtilities.PortFieldMinLabel(_Seed);
        }

        protected override void NodeBodyGUI()
        {
            graph.editorUtilities.PropertyFieldMinLabel(_DivisionChance);
            
            graph.editorUtilities.PropertyFieldMinLabel(_MinRooms);
            graph.editorUtilities.PropertyFieldMinLabel(_MaxRooms);

            graph.editorUtilities.PropertyFieldMinLabel(_MinRoomSize);
            graph.editorUtilities.PropertyFieldMinLabel(_MaxRoomSize);
            graph.editorUtilities.PropertyFieldMinLabel(_MinRoomArea);
            graph.editorUtilities.PropertyFieldMinLabel(_MaxRoomArea);

            graph.editorUtilities.PropertyFieldMinLabel(_MinWallWidth);

            graph.editorUtilities.PropertyFieldMinLabel(_ShowDebugLines);
        }
    }
}