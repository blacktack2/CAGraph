using System.Collections.Generic;
using UnityEngine;

public abstract class CellGrid : MonoBehaviour
{
    protected enum Preset { Glider, T3_Pulsar, }
    private static readonly string[] _PresetFiles = { "Glider", "T3_Pulsar" };

    public abstract int GetCellCount();

    public abstract int GetIteration();

    protected Vector2Int[] LoadPreset(Preset preset)
    {
        string filename = "Presets/" + _PresetFiles[(int)preset];
        TextAsset dataset = Resources.Load<TextAsset>(filename);
        string[] positions = dataset.text.Split(new char[] {','});
        Vector2Int[] cells = new Vector2Int[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            string[] coords = positions[i].Split(new char[] {':'});
            cells[i] = new Vector2Int(int.Parse(coords[0]), int.Parse(coords[1]));
        }
        return cells;
    }

    protected Vector2Int[] LoadRandomized(int scale, float chance)
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        for (int x = -scale / 2; x < scale / 2; x++)
            for (int y = -scale / 2; y < scale / 2; y++)
                if (Random.value < chance)
                    cells.Add(new Vector2Int(x, y));
        return cells.ToArray();
    }

    protected int[] PositionToGrid(Vector2Int[] positions, int scale)
    {
        int[] cells = new int[scale * scale];
        Vector2Int center = new Vector2Int(scale / 2, scale / 2);
        foreach(Vector2Int pos in positions)
        {
            Vector2Int centeredPos = pos + center;
            if (centeredPos.x >= 0 && centeredPos.x < scale && centeredPos.y >= 0 && centeredPos.y < scale)
                cells[centeredPos.x + centeredPos.y * scale] = 1;
        }
        return cells;
    }
}
