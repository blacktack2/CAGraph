using System;
using XNode;

namespace CAGraph.Nodes
{
    public abstract class BaseNode : Node
    {
        protected override void Init()
        {
            base.Init();
        }

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

        protected virtual void UpdateMatrixOutput(string portName)
        {
            
        }
    }
}
