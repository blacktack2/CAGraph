namespace TileGraph.Nodes
{
    /// <summary> Interface used to identify input nodes. </summary>
    public interface IInputNode : IInputOutputNode
    {
        public void SetInput<T>(T input);
    }
}