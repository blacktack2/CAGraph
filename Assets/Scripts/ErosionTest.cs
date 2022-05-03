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

        int res = _Terrain.terrainData.heightmapResolution;
        float[,] heightmap = _Terrain.terrainData.GetHeights(0, 0, res, res);
        int tx = 0, ty = 0;
        for (int i = 0, x = 0, y = 0; i < heightmap.Length; i++, x++)
        {
            if (x >= res)
            {
                x = 0;
                y++;
                ty = (int) (y * (float) tileMap.height / res);
            }
            tx = (int) (x * (float) tileMap.width / res);
            heightmap[x,y] = cells[tx + ty * tileMap.width] / 3f;
        }
        _Terrain.terrainData.SetHeights(0, 0, heightmap);
    }
}
