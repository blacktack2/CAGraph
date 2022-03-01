using UnityEngine;
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
        /// <typeparam name="MTypeIn"> Type of TileMap being operated on.
        /// <param name="setOutBuffer"> <c>true</c> if
        /// <paramref name="outBuffer" /> should be automatically reset,
        /// otherwise <c>false</c> if <paramref name="outBuffer" /> should not
        /// be changed. </param>
        /// </typeparam>
        protected void GetTileMapInput<MTypeIn>(string inputPortName, string outputPortName,
                                                ref MTypeIn outBuffer, ref long inBuffer,
                                                bool parameterChanged = false)
            where MTypeIn : Types.TileMap
        {
            MTypeIn tileMap = GetInputValue<MTypeIn>(inputPortName);
            if (tileMap == null)
            {
                inBuffer = 0L;
                outBuffer = null;
            }
            else if (outBuffer == null || tileMap.GetID() != inBuffer)
            {
                inBuffer = tileMap.GetID();
                outBuffer = (MTypeIn) tileMap.Clone();
                UpdateTileMapOutput(outputPortName);
            }
            else if (parameterChanged)
            {
                outBuffer = (MTypeIn) tileMap.Clone();
                UpdateTileMapOutput(outputPortName);
            }
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
        /// <typeparam name="MTypeIn"> Type of TileMap being operated on.
        /// <param name="setOutBuffer"> <c>true</c> if
        /// <paramref name="outBuffer" /> should be automatically reset,
        /// otherwise <c>false</c> if <paramref name="outBuffer" /> should not
        /// be changed. </param>
        /// </typeparam>
        protected void GetTileMapInput<MTypeIn, MTypeOut>(string inputPortName, string outputPortName,
                                                ref MTypeOut outBuffer, ref long inBuffer,
                                                bool parameterChanged = false)
            where MTypeIn : Types.TileMap where MTypeOut : Types.TileMap
        {
            MTypeIn tileMap = GetInputValue<MTypeIn>(inputPortName);
            if (tileMap == null)
            {
                inBuffer = 0L;
                outBuffer = null;
            }
            else if (outBuffer == null || tileMap.GetID() != inBuffer)
            {
                inBuffer = tileMap.GetID();
                UpdateTileMapOutput(outputPortName);
            }
            else if (parameterChanged)
            {
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
