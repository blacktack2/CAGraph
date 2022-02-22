using XNode;

namespace CAGraph.Nodes
{
    /// <summary> Base node class containing operations and parameters common
    /// to all CAGraph nodes. </summary>
    public abstract class BaseNode : Node
    {
        protected CAGraph _Graph;

        protected override void Init()
        {
            base.Init();
            _Graph = (CAGraph) graph;
        }

        /// <summary> Check the input value at
        /// <paramref name="inputPortName" /> then update the given buffers and
        /// call <code>UpdateMatrixOutput(outputPortname)</code> if any values
        /// have been modified since the last change. </summary>
        /// <param name="inputPortName"> Name of the input port receiving the
        /// input value matrix. </param>
        /// <param name="outputPortName"> Name of the port the output matrix is
        /// to be sent to. </param>
        /// <param name="outBuffer"> Reference to the buffer used to store the
        /// output matrix. </param>
        /// <param name="inBuffer"> Reference to the buffer used to store the
        /// ID of the input matrix. </param>
        /// <param name="parameterChanged"> <c>true</c> if a value relevant to
        /// the output has been changed, otherwise <c>false</c>. </param>
        /// <typeparam name="MType"> Type of matrix being operated on.
        /// </typeparam>
        protected void GetMatrixInput<MType>(string inputPortName, string outputPortName,
                                      ref MType outBuffer, ref long inBuffer, bool parameterChanged)
            where MType : Types.Matrix
        {
            MType matrix = GetInputValue<MType>(inputPortName);
            if (matrix == null)
            {
                inBuffer = 0L;
                outBuffer = null;
            }
            else if (outBuffer == null || matrix.GetID() != inBuffer)
            {
                inBuffer = matrix.GetID();
                outBuffer = (MType) matrix.Clone();
                UpdateMatrixOutput(outputPortName);
            }
            else if (parameterChanged)
            {
                outBuffer = (MType) matrix.Clone();
                UpdateMatrixOutput(outputPortName);
            }
        }

        /// <summary> Update the contents of the matrix buffer being sent to
        /// the output port at <paramref name="portName" /> </summary>
        protected virtual void UpdateMatrixOutput(string portName)
        {
            
        }
    }
}
