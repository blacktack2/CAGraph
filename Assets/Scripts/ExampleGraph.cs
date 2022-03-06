using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TileGraph.Types;

public class ExampleGraph : MonoBehaviour
{
    [SerializeField]
    private TileGraph.TileGraph _ExampleGraph;

    [SerializeField]
    private GameObject _TilePrefab;

    private Transform _TileMapBool, _TileMapCont, _TileMapUint;

    private void Awake()
    {
        _TileMapBool = new GameObject("TileMap Boolean").transform;
        _TileMapBool.SetParent(transform);
        _TileMapBool.position = new Vector3(-100, 0, 0);

        _TileMapCont = new GameObject("TileMap Continuous").transform;
        _TileMapCont.SetParent(transform);
        _TileMapCont.position = new Vector3(0, 0, 0);

        _TileMapUint = new GameObject("TileMap Integer").transform;
        _TileMapUint.SetParent(transform);
        _TileMapUint.position = new Vector3(100, 0, 0);
    }

    private void Start()
    {
        GenerateTileMapBool(_ExampleGraph.GetOutputValue<TileMapBool>("TMBoolOut"));
        GenerateTileMapCont(_ExampleGraph.GetOutputValue<TileMapCont>("TMContOut"));
        GenerateTileMapUint(_ExampleGraph.GetOutputValue<TileMapUint>("TMUintOut"));
    }

    private void GenerateTileMapBool(TileMapBool tileMap)
    {
        for (int x = 0; x < tileMap.width; x++)
        {
            for (int y = 0; y < tileMap.height; y++)
            {
                Transform tile = Instantiate(
                    _TilePrefab,
                    new Vector3(x - tileMap.width / 2, y - tileMap.height / 2, 0),
                    Quaternion.identity
                ).transform;
                tile.SetParent(_TileMapBool, false);
                tile.GetComponent<Renderer>().material.color = tileMap.GetCellAt(x, y) == 0 ? Color.black : Color.white;
            }
        }
    }

    private void GenerateTileMapCont(TileMapCont tileMap)
    {
        for (int x = 0; x < tileMap.width; x++)
        {
            for (int y = 0; y < tileMap.height; y++)
            {
                Transform tile = Instantiate(
                    _TilePrefab,
                    new Vector3(x - tileMap.width / 2, y - tileMap.height / 2, 0),
                    Quaternion.identity
                ).transform;
                tile.SetParent(_TileMapCont, false);
                tile.GetComponent<Renderer>().material.color = Color.Lerp(Color.black, Color.white, tileMap.GetCellAt(x, y));
            }
        }
    }

    private void GenerateTileMapUint(TileMapUint tileMap)
    {
        for (int x = 0; x < tileMap.width; x++)
        {
            for (int y = 0; y < tileMap.height; y++)
            {
                Transform tile = Instantiate(
                    _TilePrefab,
                    new Vector3(x - tileMap.width / 2, y - tileMap.height / 2, 0),
                    Quaternion.identity
                ).transform;
                tile.SetParent(_TileMapUint, false);

                Color c;
                uint value = tileMap.GetCellAt(x, y);
                if (value == 0)
                {
                    c = Color.black;
                }
                else if (value == 1)
                {
                    c = Color.white;
                }
                else
                {
                    Random.State state = Random.state;
                    Random.InitState((int) value);
                    c = Random.ColorHSV(0f, 1f);
                    Random.state = state;
                }
                tile.GetComponent<Renderer>().material.color = c;
            }
        }
    }
}
