using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Base node class containing operations and parameters common
    /// to all CAGraph nodes. </summary>
    public abstract class BaseNode : Node
    {
        protected TileGraph _Graph;

        protected override void Init()
        {
            base.Init();
            _Graph = (TileGraph) graph;
        }

        /// <summary> Check the input value at
        /// <paramref name="inputPortName" /> then update the given buffers and
        /// call <code>UpdateTileMapOutput(outputPortname)</code> if any values
        /// have been modified since the last change. </summary>
        /// <param name="inputPortName"> Name of the input port receiving the
        /// input value TileMap. </param>
        /// <param name="outputPortName"> Name of the port the output TileMap is
        /// to be sent to. </param>
        /// <param name="outBuffer"> Reference to the buffer used to store the
        /// output TileMap. </param>
        /// <param name="inBuffer"> Reference to the buffer used to store the
        /// ID of the input TileMap. </param>
        /// <param name="parameterChanged"> <c>true</c> if a value relevant to
        /// the output has been changed, otherwise <c>false</c>. </param>
        /// <typeparam name="MType"> Type of TileMap being operated on.
        /// </typeparam>
        protected void GetTileMapInput<MType>(string inputPortName, string outputPortName,
                                      ref MType outBuffer, ref long inBuffer, bool parameterChanged)
            where MType : Types.TileMap
        {
            MType tileMap = GetInputValue<MType>(inputPortName);
            if (tileMap == null)
            {
                inBuffer = 0L;
                outBuffer = null;
            }
            else if (outBuffer == null || tileMap.GetID() != inBuffer)
            {
                inBuffer = tileMap.GetID();
                outBuffer = (MType) tileMap.Clone();
                UpdateTileMapOutput(outputPortName);
            }
            else if (parameterChanged)
            {
                outBuffer = (MType) tileMap.Clone();
                UpdateTileMapOutput(outputPortName);
            }
        }

        /// <summary> Update the contents of the TileMap buffer being sent to
        /// the output port at <paramref name="portName" /> </summary>
        protected virtual void UpdateTileMapOutput(string portName)
        {
            
        }
    }
}
