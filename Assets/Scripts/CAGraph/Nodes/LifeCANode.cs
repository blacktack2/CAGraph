using System.Text.RegularExpressions;
using UnityEngine;
using XNode;

namespace CAGraph.Nodes
{
    [CreateNodeMenu("Operations/Matrix/Lifelike CA", 5)]
    public class LifeCANode : Node
    {
        [SerializeField, Input] private Types.Matrix _MatrixIn;
        [SerializeField, Output] private Types.Matrix _MatrixOut;

        [SerializeField, HideInInspector]
        private bool[] _Rules = new bool[18];
        [SerializeField, HideInInspector]
        private int _Iterations = 1;

        private bool _RulesChanged = false;
        private int _CurrentIterations = 0;

        private CAGraph _Graph;

        private long _MatrixInIDBuffer = 0L;
        private Types.Matrix _MatrixOutBuffer;

        protected override void Init()
        {
            base.Init();
            _Graph = (CAGraph) graph;
        }

        private void Reset()
        {
            name = "Lifelike CA";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_MatrixOut")
                return GetIteratedMatrix();
            return null;
        }

        private Types.Matrix GetIteratedMatrix()
        {
            Types.Matrix matrix = GetInputValue<Types.Matrix>("_MatrixIn");
            if (matrix == null)
            {
                _MatrixInIDBuffer = 0L;
                _MatrixOutBuffer = null;
                return null;
            }
            else if (_MatrixOutBuffer == null || matrix.id != _MatrixInIDBuffer)
            {
                _MatrixInIDBuffer = matrix.id;
                _MatrixOutBuffer = matrix.Copy();
                Iterate();
            }
            else if (_RulesChanged || _Iterations != _CurrentIterations)
            {
                _MatrixOutBuffer = matrix.Copy();
                Iterate();
            }
            return _MatrixOutBuffer;
        }

        private void Iterate()
        {
            _RulesChanged = false;
            _CurrentIterations = _Iterations;

            int[] rules = new int[18];
            for (int i = 0; i < rules.Length; i++)
                rules[i] = _Rules[i] ? 1 : 0;

            _Graph.CAHandler.IterateCells(_MatrixOutBuffer, rules, _Iterations);
        }

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
