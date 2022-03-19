using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        private static readonly int
            _TileMapBool0ID = Shader.PropertyToID("_TileMapBool0"),
            _TileMapBool1ID = Shader.PropertyToID("_TileMapBool1"),
            _TileMapCont0ID = Shader.PropertyToID("_TileMapCont0"),
            _TileMapCont1ID = Shader.PropertyToID("_TileMapCont1"),
            _TileMapUint0ID = Shader.PropertyToID("_TileMapUint0"),
            _TileMapUint1ID = Shader.PropertyToID("_TileMapUint1"),
            _BufferFlagID = Shader.PropertyToID("_BufferFlag"),
            _ScaleXID = Shader.PropertyToID("_ScaleX"),
            _ScaleYID = Shader.PropertyToID("_ScaleY"),
            _LifeRulesID = Shader.PropertyToID("_LifeRules"),
            _FrequencyID = Shader.PropertyToID("_Frequency"), 
            _OffsetID = Shader.PropertyToID("_Offset"),
            _OctavesID = Shader.PropertyToID("_Octaves"),
            _LacunarityID = Shader.PropertyToID("_Lacunarity"),
            _PersistenceID = Shader.PropertyToID("_Persistence"),
            _CentroidThresholdID = Shader.PropertyToID("_CentroidThreshold");
        
        private enum FunctionKernels {
            IterateLifeCells,
            ValueNoise1D, ValueNoise2D, ValueNoise3D,
            FractalValueNoise1D, FractalValueNoise2D, FractalValueNoise3D,
            PerlinNoise1D, PerlinNoise2D, PerlinNoise3D,
            FractalPerlinNoise1D, FractalPerlinNoise2D, FractalPerlinNoise3D,
            SimplexNoise1D, SimplexNoise2D, SimplexNoise3D,
            FractalSimplexNoise1D, FractalSimplexNoise2D, FractalSimplexNoise3D,
            VoronoiNoise2D
        }

        private ComputeShader _ComputeShader;
            
        private ComputeBuffer _TileMapBool0Buffer;
        private ComputeBuffer _TileMapBool1Buffer;
        private ComputeBuffer _TileMapCont0Buffer;
        private ComputeBuffer _TileMapCont1Buffer;
        private ComputeBuffer _TileMapUint0Buffer;
        private ComputeBuffer _TileMapUint1Buffer;
        private ComputeBuffer _LifeRulesBuffer;
        private ComputeBuffer _LacunarityBuffer;
        private ComputeBuffer _PersistenceBuffer;

        private CellularAutomata _CellularAutomata;
        public CellularAutomata cellularAutomata {get {return _CellularAutomata;}}
        private Noise _Noise;
        public Noise noise {get {return _Noise;}}
        private TileMapOperations _TileMapOperations;
        public TileMapOperations tileMapOperations {get {return _TileMapOperations;}}
        private TileMapCast _TileMapCast;
        public TileMapCast tileMapCast {get {return _TileMapCast;}}

        public FunctionLibrary(ComputeShader computeShader)
        {
            _ComputeShader = computeShader;

            _CellularAutomata = new CellularAutomata(this);
            _Noise = new Noise(this);
            _TileMapOperations = new TileMapOperations(this);
            _TileMapCast = new TileMapCast(this);
        }

        public void Enable()
        {
            _TileMapBool0Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(int));
            _TileMapBool1Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(int));
            _TileMapCont0Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(float));
            _TileMapCont1Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(float));
            _TileMapUint0Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(uint));
            _TileMapUint1Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(uint));
            _LifeRulesBuffer = new ComputeBuffer(18, sizeof(int));
            _LacunarityBuffer = new ComputeBuffer(20, sizeof(float));
            _PersistenceBuffer = new ComputeBuffer(20, sizeof(float));
        }

        public void Disable()
        {
            _TileMapBool0Buffer.Dispose();
            _TileMapBool1Buffer.Dispose();
            _TileMapCont0Buffer.Dispose();
            _TileMapCont1Buffer.Dispose();
            _TileMapUint0Buffer.Dispose();
            _TileMapUint1Buffer.Dispose();
            _LifeRulesBuffer.Dispose();
            _LacunarityBuffer.Dispose();
            _PersistenceBuffer.Dispose();

            _TileMapBool0Buffer = null;
            _TileMapBool1Buffer = null;
            _TileMapCont0Buffer = null;
            _TileMapCont1Buffer = null;
            _TileMapUint0Buffer = null;
            _TileMapUint1Buffer = null;
            _LifeRulesBuffer    = null;
            _LacunarityBuffer   = null;
            _PersistenceBuffer  = null;
        }

        int Random1D(int seed)
        {
            int state = (int) (seed * 74779640 + 2891336453);
            int word = ((state >> ((state >> 28) + 4)) ^ state) * 277803737;
            return (word >> 22) ^ word;
        }
        int Random1D(Vector2Int seed)
        {
            return Random1D(seed.x ^ Random1D(seed.y));
        }
        int Random1D(Vector3Int seed)
        {
            return Random1D(seed.x ^ Random1D(seed.y ^ Random1D(seed.z)));
        }
        Vector2Int Random2D(int seed)
        {
            int x = Random1D(seed);
            int y = Random1D(x);
            return new Vector2Int(x, y);
        }
        Vector2Int Random2D(Vector2Int seed)
        {
            int x = Random1D(seed);
            int y = Random1D(x);
            return new Vector2Int(x, y);
        }
        Vector2Int Random2D(Vector3Int seed)
        {
            int x = Random1D(seed);
            int y = Random1D(x);
            return new Vector2Int(x, y);
        }
        Vector3Int Random3D(int seed)
        {
            int x = Random1D(seed);
            int y = Random1D(x);
            int z = Random1D(y);
            return new Vector3Int(x, y, z);
        }
        Vector3Int Random3D(Vector2Int seed)
        {
            int x = Random1D(seed);
            int y = Random1D(x);
            int z = Random1D(y);
            return new Vector3Int(x, y, z);
        }
        Vector3Int Random3D(Vector3Int seed)
        {
            int x = Random1D(seed);
            int y = Random1D(x);
            int z = Random1D(y);
            return new Vector3Int(x, y, z);
        }
    }
}
