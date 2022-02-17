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
        private Utilities.CAEditorUtilities _CAEditorUtilities;
        public Utilities.CAEditorUtilities CAEditorUtilities {get {return _CAEditorUtilities;}}

        void OnEnable()
        {
            _CAHandler = new Utilities.CAHandler(_ComputeShader);
            _CAEditorUtilities = new Utilities.CAEditorUtilities();
            _CAHandler.Enable();
            _CAEditorUtilities.Enable();
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
