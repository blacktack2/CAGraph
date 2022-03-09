using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        private static readonly int
            _Cells0ID = Shader.PropertyToID("_Cells0"),
            _Cells1ID = Shader.PropertyToID("_Cells1"),
            _LifeRulesID = Shader.PropertyToID("_LifeRules"),
            _BufferFlagID = Shader.PropertyToID("_BufferFlag"),
            _ScaleXID = Shader.PropertyToID("_ScaleX"),
            _ScaleYID = Shader.PropertyToID("_ScaleY");
        
        private enum FunctionKernels { LifeLikeCA }

        private ComputeShader _ComputeShader;
            
        private ComputeBuffer _Cells0Buffer;
        private ComputeBuffer _Cells1Buffer;
        private ComputeBuffer _LifeRulesBuffer;

        private CellularAutomata _CellularAutomata;
        public CellularAutomata cellularAutomata {get {return _CellularAutomata;}}
        private TileMapOperations _TileMapOperations;
        public TileMapOperations tileMapOperations {get {return _TileMapOperations;}}
        private TileMapCast _TileMapCast;
        public TileMapCast tileMapCast {get {return _TileMapCast;}}

        public FunctionLibrary(ComputeShader computeShader)
        {
            _ComputeShader = computeShader;

            _CellularAutomata = new CellularAutomata(this);
            _TileMapOperations = new TileMapOperations(this);
            _TileMapCast = new TileMapCast(this);
        }

        public void Enable()
        {
            _Cells0Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(int));
            _Cells1Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(int));
            _LifeRulesBuffer = new ComputeBuffer(18, sizeof(int));
        }

        public void Disable()
        {
            _Cells0Buffer.Dispose();
            _Cells1Buffer.Dispose();
            _LifeRulesBuffer.Dispose();

            _Cells0Buffer = null;
            _Cells1Buffer = null;
            _LifeRulesBuffer = null;
        }
    }
}
