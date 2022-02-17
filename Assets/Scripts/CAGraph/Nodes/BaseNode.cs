using XNode;

namespace CAGraph.Nodes
{
    public abstract class BaseNode : Node
    {
        protected override void Init()
        {
            base.Init();
        }

        protected void GetMatrixInput(string inputPortName, string outputPortName, ref Types.Matrix outBuffer, ref long inBuffer, bool parameterChanged)
        {
            Types.Matrix matrix = GetInputValue<Types.Matrix>(inputPortName);
            if (matrix == null)
            {
                inBuffer = 0L;
                outBuffer = null;
            }
            else if (outBuffer == null || matrix.id != inBuffer)
            {
                inBuffer = matrix.id;
                outBuffer = matrix.Copy();
                UpdateMatrixOutput(outputPortName);
            }
            else if (parameterChanged)
            {
                outBuffer = matrix.Copy();
                UpdateMatrixOutput(outputPortName);
            }
        }

        protected virtual void UpdateMatrixOutput(string portName)
        {
            
        }
    }
}
