using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    private struct CellData
    {
        public int neighbours;
        public bool isLive;
    }

    private enum Preset { Glider, T3_Pulsar, }

    private static readonly string[] _PresetFiles = { "Glider", "T3_Pulsar" };

    [SerializeField]
    private Mesh _Mesh;
    [SerializeField]
    private Material _Material;

    private enum InitMode { Manual, Preset }

    [SerializeField]
    private InitMode _InitMode = InitMode.Manual;

    [SerializeField]
    private Vector2Int[] _InitialCellPositions;
    [SerializeField]
    private Preset _InitalPreset;

    [SerializeField]
    private float _IterationDuration = 0.5f;

    public BoundsInt simulationBounds;

    private Dictionary<Vector2Int, Transform> _Cells;
    private Dictionary<Vector2Int, CellData> _Neighbours;

    private float _IterationTime = 0f;

    void Awake()
    {
        _Cells = new Dictionary<Vector2Int, Transform>();
        if (_InitMode == InitMode.Manual)
        {
            for (int i = 0; i < _InitialCellPositions.Length; i++)
                CreateCell(_InitialCellPositions[i]);
        }
        else
        {
            LoadPreset(Vector2Int.zero);
        }
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
        simulationBounds.min = min;
        simulationBounds.max = max;
    }

    void IncrementNeighboursAt(Vector2Int position)
    {
        CellData cellData;
        if (!_Neighbours.TryGetValue(position, out cellData))
            cellData = new CellData {isLive = false, neighbours = 0};
        cellData.neighbours += 1;
        _Neighbours[position] = cellData;
    }

    void LoadPreset(Vector2Int offset)
    {
        string filename = "Presets/" + _PresetFiles[(int)_InitalPreset];
        Debug.Log(filename);
        TextAsset dataset = Resources.Load<TextAsset>(filename);
        string[] positions = dataset.text.Split(new char[] {','});
        foreach (string posText in positions)
        {
            string[] coords = posText.Split(new char[] {':'});
            CreateCell(offset + new Vector2Int(int.Parse(coords[0]), int.Parse(coords[1])));
        }
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
}
