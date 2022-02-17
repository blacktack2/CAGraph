using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Operations/Matrix/Randomize", 20)]
    public class MatrixRandomizeNode : BaseNode
    {
        [SerializeField]
        private int _Seed = 0;
        [SerializeField, Range(0f, 1f)]
        private float _Chance = 0.5f;
        
        private int _CurrentSeed = 1;
        private float _CurrentChance = 0f;

        [SerializeField, Input] private Types.Matrix _MatrixIn;
        [SerializeField, Output] private Types.Matrix _MatrixOut;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix _MatrixOutBuffer;

        private void Reset()
        {
            name = "Randomize Matrix";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_MatrixOut")
            {
                GetMatrixInput(
                    "_MatrixIn", "_MatrixOut",
                    ref _MatrixOutBuffer, ref _MatrixInIDBuffer,
                    _CurrentSeed != _Seed || _CurrentChance != _Chance
                );
                return _MatrixOutBuffer;
            }
            return null;
        }

        protected override void UpdateMatrixOutput(string portName)
        {
            if (portName == "_MatrixOut")
            {
                _CurrentSeed = _Seed;
                _CurrentChance = _Chance;
                Utilities.MatrixOperations.RandomizeMatrix(_MatrixOutBuffer, _Chance, _Seed);
            }
        }

        public void SetSeed(int seed)
        {
            _Seed = seed;
        }
    }
}
