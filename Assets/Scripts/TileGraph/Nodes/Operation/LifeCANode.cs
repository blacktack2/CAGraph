using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Operation node for applying a life-like Cellular Automata
    /// to a <paramref name="TileMapBool" />. </summary>
    [CreateNodeMenu("Operations/TileMap/Bool/Lifelike CA", 5)]
    public class LifeCANode : BaseNode
    {
        public const int maxIterations = 5000;

        [SerializeField, Input] private Types.TileMapBool _TileMapIn;
        [SerializeField, Output] private Types.TileMapBool _TileMapOut;

        /// <summary> Rules for the life-like CA. Rules 0-8 are the birth rules
        /// and rules 9-17 are the survival rules. </summary>
        [SerializeField, HideInInspector]
        private bool[] _Rules = new bool[18];
        /// <summary> Number of iterations to run the CA for. </summary>
        [SerializeField, HideInInspector, Range(0, maxIterations)]
        private int _Iterations = 1;

        private bool _RulesChanged = false;
        private int _CurrentIterations = 0;

        private long _TileMapInIDBuffer = 0L;
        private Types.TileMapBool _TileMapOutBuffer;

        private void Reset()
        {
            name = "Lifelike CA";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "_TileMapOut")
            {
                GetTileMapInput(
                    "_TileMapIn", "_TileMapOut",
                    ref _TileMapOutBuffer, ref _TileMapInIDBuffer,
                    _RulesChanged || _Iterations != _CurrentIterations
                );
                return _TileMapOutBuffer;
            }
            return null;
        }

        protected override void UpdateTileMapOutput(string portName)
        {
            if (portName == "_TileMapOut")
            {
                _RulesChanged = false;
                _CurrentIterations = _Iterations;

                int[] rules = new int[18];
                for (int i = 0; i < rules.Length; i++)
                    rules[i] = _Rules[i] ? 1 : 0;

                _Graph.CAHandler.IterateCells(_TileMapOutBuffer, rules, _Iterations);
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
