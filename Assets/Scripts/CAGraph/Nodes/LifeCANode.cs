using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    /// <summary> Operation node for applying a life-like Cellular Automata
    /// to a <paramref name="Matrix01" />. </summary>
    [CreateNodeMenu("Operations/Matrix/Lifelike CA", 5)]
    public class LifeCANode : BaseNode
    {
        [SerializeField, Input] private Types.Matrix01 _MatrixIn;
        [SerializeField, Output] private Types.Matrix01 _MatrixOut;

        /// <summary> Rules for the life-like CA. Rules 0-8 are the birth rules
        /// and rules 9-17 are the survival rules. </summary>
        [SerializeField, HideInInspector]
        private bool[] _Rules = new bool[18];
        /// <summary> Number of iterations to run the CA for. </summary>
        [SerializeField, HideInInspector]
        private int _Iterations = 1;

        private bool _RulesChanged = false;
        private int _CurrentIterations = 0;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix01 _MatrixOutBuffer;

        private void Reset()
        {
            name = "Lifelike CA";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_MatrixOut")
            {
                GetMatrixInput(
                    "_MatrixIn", "_MatrixOut",
                    ref _MatrixOutBuffer, ref _MatrixInIDBuffer,
                    _RulesChanged || _Iterations != _CurrentIterations
                );
                return _MatrixOutBuffer;
            }
            return null;
        }

        protected override void UpdateMatrixOutput(string portName)
        {
            if (portName == "_MatrixOut")
            {
                _RulesChanged = false;
                _CurrentIterations = _Iterations;

                int[] rules = new int[18];
                for (int i = 0; i < rules.Length; i++)
                    rules[i] = _Rules[i] ? 1 : 0;

                _Graph.CAHandler.IterateCells(_MatrixOutBuffer, rules, _Iterations);
            }
        }

        /// <returns> <c>false</c> if the given rules are the same as the
        /// buffered rules, otherwise <c>true</c>. </returns>
        public bool CompareNotation(string born, string survive)
        {
            if (_RulesChanged)
                return false;
            for (int i = 0; i < 9; i++)
            {
                string n = i.ToString();
                if (born.Contains(n) != _Rules[i] || survive.Contains(n) != _Rules[9 + i])
                {
                    _RulesChanged = true;
                    return true;
                }
            }
            return false;
        }

        /// <returns> Ordered strings of integers representing which conditions
        /// are defined as true. Index 0 represents birth rules, and index 1
        /// represents survival rules. </returns>
        public string[] GetRuleStrings()
        {
            string[] rules = new string[] {"", ""};

            for (int i = 0; i < 9; i++)
            {
                if (_Rules[i])
                    rules[0] += i.ToString();
                if (_Rules[9 + i])
                    rules[1] += i.ToString();
            }
            return rules;
        }
    }
}
