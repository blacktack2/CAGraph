using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Interface used to identify output nodes. </summary>
    public interface IOutputNode
    {
        public string GetName();
        public T GetOutput<T>();
    }
}
