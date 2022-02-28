using UnityEngine;
using XNode;

namespace TileGraph
{
    [CreateAssetMenu]
    public class TileGraph : NodeGraph
    {
        [SerializeField]
        private ComputeShader _ComputeShader;
        public ComputeShader computeShader {get {return _ComputeShader;}}

        private Utilities.CAHandler _CAHandler;
        public Utilities.CAHandler CAHandler {get {return _CAHandler;}}
        private Utilities.EditorUtilities _CAEditorUtilities;
        public Utilities.EditorUtilities CAEditorUtilities {get {return _CAEditorUtilities;}}

        void OnEnable()
        {
            _CAHandler = new Utilities.CAHandler(_ComputeShader);
            _CAEditorUtilities = new Utilities.EditorUtilities();
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
