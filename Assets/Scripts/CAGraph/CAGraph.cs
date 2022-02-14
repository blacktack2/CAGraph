using UnityEngine;
using XNode;

namespace CAGraph
{
    [CreateAssetMenu]
    public class CAGraph : NodeGraph
    {
        [SerializeField]
        private ComputeShader _ComputeShader;
        public ComputeShader computeShader {get {return _ComputeShader;}}

        private Utilities.CAHandler _CAHandler;
        public Utilities.CAHandler CAHandler {get {return _CAHandler;}}

        void OnEnable()
        {
            if (_CAHandler == null)
                _CAHandler = new Utilities.CAHandler(_ComputeShader);
            _CAHandler.Enable();
        }

        void OnDisable()
        {
            _CAHandler.Disable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _CAHandler.Disable();
        }
    }
}
