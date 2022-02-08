using System.Collections.Generic;
using UnityEngine;

public class CellGridGOL_CPU : CellGrid
{
    private struct CellData
    {
        public int neighbours;
        public bool isLive;
    }

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
    [SerializeField, Range(2, 200)]
    private int _RandomResolution = 10;
    [SerializeField, Range(0, 1)]
    private float _RandomChance = 0.5f;

    [SerializeField]
    private float _IterationDuration = 0.5f;

    private BoundsInt _SimulationBounds;
    public BoundsInt simulationBounds => _SimulationBounds;

    private Dictionary<Vector2Int, Transform> _Cells;
    private Dictionary<Vector2Int, CellData> _Neighbours;

    private float _IterationTime = 0f;

    private int _Iteration;

    void Awake()
    {
        _Iteration = 0;
        _Cells = new Dictionary<Vector2Int, Transform>();
        // if (_InitMode == InitMode.Manual)
        // {
        //     for (int i = 0; i < _InitialCellPositions.Length; i++)
        //         CreateCell(_InitialCellPositions[i]);
        // }
        // else if (_InitMode == InitMode.Preset)
        // {
        //     LoadPreset(Vector2Int.zero);
        // }
        // else
        // {
        //     for (int x = 0; x < _RandomResolution; x++)
        //         for (int y = 0; y < _RandomResolution; y++)
        //             if (Random.value < _RandomChance)
        //                 CreateCell(new Vector2Int(x - _RandomResolution / 2, y - _RandomResolution / 2));
        // }
        Vector2Int[] cells;
        switch (_InitMode)
        {
            case InitMode.Manual:
                cells = _InitialCellPositions;
                break;
            case InitMode.Preset:
                cells = LoadPreset(_InitalPreset);
                break;
            case InitMode.Random: default:
                cells = LoadRandomized(_RandomResolution, _RandomChance);
                break;
        }
        foreach (Vector2Int position in cells)
            CreateCell(position);
    }

    void Update()
    {
        _IterationTime += Time.deltaTime;
        if (_IterationTime > _IterationDuration)
        {
            IterateCells();
            _IterationTime -= _IterationDuration;
        }
    }

    void IterateCells()
    {
        CalculateNeighbours();
        UpdateCells();
        _Iteration++;
    }

    void CalculateNeighbours()
    {
        _Neighbours = new Dictionary<Vector2Int, CellData>();
        CellData defaultCellData = new CellData();
        foreach (Vector2Int position in _Cells.Keys)
        {
            Transform cell = _Cells[position];
            CellData cellData = defaultCellData;
            
            _Neighbours.TryGetValue(position, out cellData);
            cellData.isLive = true;
            _Neighbours[position] = cellData;

            IncrementNeighboursAt(position + new Vector2Int(-1, -1));
            IncrementNeighboursAt(position + new Vector2Int(0, -1));
            IncrementNeighboursAt(position + new Vector2Int(1, -1));
            IncrementNeighboursAt(position + new Vector2Int(-1, 0));
            IncrementNeighboursAt(position + new Vector2Int(1, 0));
            IncrementNeighboursAt(position + new Vector2Int(-1, 1));
            IncrementNeighboursAt(position + new Vector2Int(0, 1));
            IncrementNeighboursAt(position + new Vector2Int(1, 1));
        }
    }
    void UpdateCells()
    {
        DestroyCells();
        _Cells = new Dictionary<Vector2Int, Transform>();
        Vector3Int min = Vector3Int.zero;
        Vector3Int max = Vector3Int.one;
        foreach (Vector2Int position in _Neighbours.Keys)
        {
            CellData cellData = _Neighbours[position];
            if (cellData.neighbours == 3 || (cellData.isLive && cellData.neighbours == 2))
            {
                CreateCell(position);

                min.x = Mathf.Min(position.x - 1, min.x);
                min.y = Mathf.Min(position.y - 1, min.y);
                max.x = Mathf.Max(position.x + 1, max.x);
                max.y = Mathf.Max(position.y + 1, max.y);
            }
        }
        _SimulationBounds.min = min;
        _SimulationBounds.max = max;
    }

    void IncrementNeighboursAt(Vector2Int position)
    {
        CellData cellData;
        if (!_Neighbours.TryGetValue(position, out cellData))
            cellData = new CellData {isLive = false, neighbours = 0};
        cellData.neighbours += 1;
        _Neighbours[position] = cellData;
    }

    void CreateCell(Vector2Int position)
    {
        GameObject cell = new GameObject("Cell x-" + position.x + " y-" + position.y);
        cell.transform.position = (Vector2) position;
        cell.transform.SetParent(transform);
        cell.AddComponent<MeshFilter>().mesh = _Mesh;
        cell.AddComponent<MeshRenderer>().material = _Material;
        _Cells.Add(position, cell.transform);
    }

    void DestroyCells()
    {
        foreach (Vector2Int position in _Cells.Keys)
        {
            Destroy(_Cells[position].gameObject);
        }
    }

    public override int GetCellCount()
    {
        return _Cells.Count;
    }
    public override int GetIteration()
    {
        return _Iteration;
    }
}
