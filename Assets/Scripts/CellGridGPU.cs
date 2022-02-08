using UnityEngine;

public class CellGridGPU : CellGrid
{
    const int _MaxScale = 5000;

    private static readonly int
        _PositionsID = Shader.PropertyToID("_Positions"),
        _CellsID = Shader.PropertyToID("_Cells"),
        _Cells0ID = Shader.PropertyToID("_Cells0"),
        _Cells1ID = Shader.PropertyToID("_Cells1"),
        _BufferFlagID = Shader.PropertyToID("_BufferFlag"),
        _ScaleID = Shader.PropertyToID("_Scale");

    [SerializeField]
    private ComputeShader _ComputeShader;

    [SerializeField]
    private Mesh _Mesh;
    [SerializeField]
    private Material _Material;

    private enum InitMode { Manual, Preset, Random }

    [SerializeField]
    private InitMode _InitMode = InitMode.Manual;

    [SerializeField]
    private Vector2Int[] _InitialCellPositions;
    [SerializeField]
    private Preset _InitalPreset;
    [SerializeField, Range(2, _MaxScale)]
    private int _Scale = 10;
    [SerializeField, Range(0, 1)]
    private float _RandomChance = 0.5f;

    [SerializeField]
    private float _IterationDuration = 0.5f;

    private float _IterationTime = 0f;

    private int _Iteration;
    private bool _BufferFlag;

    private ComputeBuffer _Cells0Buffer;
    private ComputeBuffer _Cells1Buffer;
    private ComputeBuffer _PositionsBuffer;

    void OnEnable()
    {
        _Iteration = 0;
        _BufferFlag = false;
        Camera.main.orthographicSize = _Scale / 2;
        _Cells0Buffer = new ComputeBuffer(_MaxScale * _MaxScale, 4);
        _Cells1Buffer = new ComputeBuffer(_MaxScale * _MaxScale, 4);
        _PositionsBuffer = new ComputeBuffer(_MaxScale * _MaxScale, 3 * 4);
        int[] cells = new int[_Scale * _Scale];
        switch (_InitMode)
        {
            case InitMode.Manual:
                cells = PositionToGrid(_InitialCellPositions, _Scale);
                break;
            case InitMode.Preset:
                cells = PositionToGrid(LoadPreset(_InitalPreset), _Scale);
                break;
            case InitMode.Random: default:
                cells = PositionToGrid(LoadRandomized(_Scale, _RandomChance), _Scale);
                break;
        }
        _Cells0Buffer.SetData(cells);
        _Cells1Buffer.SetData(cells);
        Vector3[] positions = new Vector3[_Scale * _Scale];
        for (int i = 0; i < positions.Length; i++)
            positions[i] = new Vector3(i % _Scale - (float)_Scale / 2f, (i / _Scale) - (float)_Scale / 2f, 0);
        _PositionsBuffer.SetData(positions);
        _Material.SetBuffer(_PositionsID, _PositionsBuffer);
    }

    void OnDisable()
    {
        _Cells0Buffer.Release();
        _Cells1Buffer.Release();
        _PositionsBuffer.Release();
        _Cells0Buffer = null;
        _Cells1Buffer = null;
        _PositionsBuffer = null;
    }

    void Update()
    {
        _IterationTime += Time.deltaTime;
        if (_IterationTime > _IterationDuration)
        {
            IterateCells();
            _IterationTime -= _IterationDuration;
        }

        Bounds bounds = new Bounds(Vector3.zero, Vector3.one * _Scale);

        if (_BufferFlag)
            _Material.SetBuffer(_CellsID, _Cells0Buffer);
        else
            _Material.SetBuffer(_CellsID, _Cells1Buffer);
        _Material.SetInt(_ScaleID, _Scale);
        Graphics.DrawMeshInstancedProcedural(_Mesh, 0, _Material, bounds, _Scale * _Scale);
    }

    void IterateCells()
    {
        _BufferFlag = !_BufferFlag;

        _ComputeShader.SetBool(_BufferFlagID, _BufferFlag);
        _ComputeShader.SetInt(_ScaleID, _Scale);
        int kernelIndex = 0;
        int groups = Mathf.CeilToInt(_Scale / 8f);


        _ComputeShader.SetBuffer(kernelIndex, _Cells0ID, _Cells0Buffer);
        _ComputeShader.SetBuffer(kernelIndex, _Cells1ID, _Cells1Buffer);
        _ComputeShader.Dispatch(kernelIndex, groups, groups, 1);

        _Iteration++;
    }

    public override int GetCellCount()
    {
        return 0;
    }
    public override int GetIteration()
    {
        return _Iteration;
    }
}
