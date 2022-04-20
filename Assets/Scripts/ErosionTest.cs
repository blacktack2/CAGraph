using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ErosionTest : MonoBehaviour
{
    [SerializeField]
    private TileGraph.TileGraph _TileGraph;

    [SerializeField]
    private Terrain _Terrain;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTerrain();
    }

    public void UpdateTerrain()
    {
        TileGraph.Types.TileMapCont tileMap = _TileGraph.GetOutputValue<TileGraph.Types.TileMapCont>("TileMapOut");
        float[] cells = tileMap.GetCells();
        float[,] heightmap = new float[tileMap.width, tileMap.height];
        for (int i = 0, x = 0, y = 0; i < heightmap.Length; i++, x++)
        {
            if (x >= tileMap.width)
            {
                x = 0;
                y++;
            }
            heightmap[x,y] = cells[i] / 3f;
        }
        _Terrain.terrainData.SetHeights(0, 0, heightmap);
    }
}
