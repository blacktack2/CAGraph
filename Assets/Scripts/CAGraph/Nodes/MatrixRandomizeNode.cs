using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Operations/Matrix/Randomize", 20)]
    public class MatrixRandomizeNode : BaseNode
    {
        [SerializeField, Input] private Types.Matrix _MatrixIn;
        [SerializeField, Input] private int _Seed = 0;
        [SerializeField, Input, Range(0f, 1f)] private float _Chance = 0.5f;
        [SerializeField, Output] private Types.Matrix _MatrixOut;

        private int _CurrentSeed = 0;
        private float _CurrentChance = 0f;

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
                    _CurrentSeed != GetSeed() || _CurrentChance != GetChance()
                );
                return _MatrixOutBuffer;
            }
            else if (port.fieldName == "_Seed")
            {
                return GetSeed();
            }
            else if (port.fieldName == "_Chance")
            {
                return GetChance();
            }
            return null;
        }

        protected override void UpdateMatrixOutput(string portName)
        {
            if (portName == "_MatrixOut")
            {
                _CurrentSeed = GetSeed();
                _CurrentChance = GetChance();
                Utilities.MatrixOperations.RandomizeMatrix(_MatrixOutBuffer, _CurrentChance, _CurrentSeed);
            }
        }

        private int GetSeed()
        {
            return GetInputValue<int>("_Seed", _Seed);
        }

        private float GetChance()
        {
            return GetInputValue<float>("_Chance", _Chance);
        }

        public void SetSeed(int seed)
        {
            _Seed = seed;
        }
    }
}
