using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for replacing specific values in a
    /// <paramref name="TileMap" /> with another value. </summary>
    [CreateNodeMenu("Operations/TileMap/Replace", 10)]
    public class TileMapReplaceNode : BaseNode
    {
        [SerializeField, Input] private Types.TileMapBool _TileMapIn;
        [SerializeField, Output] private Types.TileMapBool _TileMapOut;

        /// <summary> List of all values to be replaced by
        /// <paramref name="_Replacement" />. </summary>
        [SerializeField]
        private List<int> _ToReplace = new List<int>();
        /// <summary> Value to replace contents of
        /// <paramref name="_ToReplace" /> with. </summary>
        [SerializeField]
        private int _Replacement = 0;

        public bool toReplaceChanged = false;
        private List<int> _CurrentToReplace = null;
        private int _CurrentReplacement = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapBool _TileMapOutBuffer;

        private void OnValidate()
        {
            toReplaceChanged = !CompareLists(_ToReplace, _CurrentToReplace);
        }

        private void Reset()
        {
            name = "Replace Value";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _Replacement != _CurrentReplacement || toReplaceChanged
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _CurrentReplacement = _Replacement;
                toReplaceChanged = false;
                // Cast contents of _ToReplace to IConvertible
                List<IConvertible> toReplace = new List<IConvertible>();
                foreach (int v in _ToReplace)
                    toReplace.Add(v);
                Utilities.TileMapOperations.ReplaceTileMapValues(_TileMapOutBuffer, toReplace, _Replacement);
            }
        }

        private bool CompareLists<T>(List<T> a, List<T> b)
        {
            if (a == b)
                return true;
            if (a == null || b == null)
                return false;
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
                if (!a[i].Equals(b[i]))
                    return false;
            return true;
        }
    }
}
