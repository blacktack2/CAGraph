using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileGraph.Utilities
{
    /// <summary> Utility class for general operations on matrices. </summary>
    public class TileMapOperations
    {
        /// <summary> Populate <paramref name="tileMap" /> with randomly
        /// distributed 0-1 values. </summary>
        /// <param name="tileMap"> <paramref name="TileMapBool" /> to randomize. </param>
        /// <param name="chance"> Probability for each value to be a 1.
        /// </param>
        /// <param name="seed"> Seed value to initialize the random number
        /// generator at. </param>
        public static void RandomizeTileMapBool(Types.TileMapBool tileMap, float chance, int seed)
        {
            Random.State state = Random.state;
            Random.InitState(seed);

            int[] cells = tileMap.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = Random.value < chance ? 1 : 0;
            tileMap.SetCells(cells);

            Random.state = state;
        }
        /// <summary> Populate <paramref name="tileMap" /> with randomly
        /// distributed 0-1 values. </summary>
        /// <param name="tileMap"> <paramref name="TileMapCont" /> to randomize. </param>
        /// </param>
        /// <param name="seed"> Seed value to initialize the random number
        /// generator at. </param>
        public static void RandomizeTileMapCont(Types.TileMapCont tileMap, int seed)
        {
            Random.State state = Random.state;
            Random.InitState(seed);

            float[] cells = tileMap.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = Random.value;
            tileMap.SetCells(cells);

            Random.state = state;
        }
        /// <summary> Populate <paramref name="tileMap" /> with randomly
        /// distributed 0-1 values. </summary>
        /// <param name="tileMap"> <paramref name="TileMapUint" /> to randomize. </param>
        /// <param name="max"> Maximum value to insert.
        /// </param>
        /// <param name="seed"> Seed value to initialize the random number
        /// generator at. </param>
        public static void RandomizeTileMapUint(Types.TileMapUint tileMap, uint max, int seed)
        {
            Random.State state = Random.state;
            Random.InitState(seed);

            uint[] cells = tileMap.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = (uint) Mathf.RoundToInt(Random.Range(0, max));
            tileMap.SetCells(cells);

            Random.state = state;
        }

        /// <summary> Set all values of <paramref name="tileMap" /> to
        /// <paramref name="value" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMap" /> to fill. </param>
        /// <param name="value"> Numeric value to fill
        /// <paramref name="tileMap" /> with (should match the datatype of
        /// <paramref name="tileMap" />). </param>
        public static void FillTileMap(Types.TileMap tileMap, IConvertible value)
        {
            IConvertible[] cells = tileMap.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = value;
            tileMap.SetCells(cells);
        }

        /// <summary> Replace all values in <paramref name="tileMap" /> matching
        /// a value in <paramref name="toReplace" /> with
        /// <paramref name="replaceWith" /> </summary>
        /// <param name="tileMap"> <paramref name="TileMap" /> to operate on. </param>
        /// <param name="toReplace"> List of all values in
        /// <paramref name="tileMap" /> which should be replaced with
        /// <paramref name="replaceWith" />. </param>
        /// <param name="replaceWith"> Value to replace all values in
        /// <paramref name="toReplace" /> with. </param>
        public static void ReplaceTileMapValues(Types.TileMap tileMap, List<IConvertible> toReplace, IConvertible replaceWith)
        {
            IConvertible[] cells = tileMap.GetCells();
            for (int c = 0; c < cells.Length; c++)
                if (toReplace.Contains((IConvertible) cells[c]))
                    cells[c] = replaceWith;
            tileMap.SetCells(cells);
        }

        public static Types.TileMapCont CastBoolToCont(Types.TileMapBool tileMap)
        {
            int[] cells = tileMap.GetCells();
            float[] newCells = new float[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                newCells[i] = (float) cells[i];
            Types.TileMapCont newTileMap = new Types.TileMapCont(tileMap.width, tileMap.height);
            newTileMap.SetCells(newCells);
            return newTileMap;
        }
        public static Types.TileMapUint CastBoolToUint(Types.TileMapBool tileMap)
        {
            int[] cells = tileMap.GetCells();
            uint[] newCells = new uint[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                newCells[i] = (uint) cells[i];
            Types.TileMapUint newTileMap = new Types.TileMapUint(tileMap.width, tileMap.height);
            newTileMap.SetCells(newCells);
            return newTileMap;
        }
        public static Types.TileMapBool CastContToBool(Types.TileMapCont tileMap, float threshold = 0.5f)
        {
            float[] cells = tileMap.GetCells();
            int[] newCells = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] < threshold)
                    newCells[i] = 0;
                else
                    newCells[i] = 1;
            }
            Types.TileMapBool newTileMap = new Types.TileMapBool(tileMap.width, tileMap.height);
            newTileMap.SetCells(newCells);
            return newTileMap;
        }
        public static Types.TileMapUint CastContToUint(Types.TileMapCont tileMap, uint max = 1)
        {
            float[] cells = tileMap.GetCells();
            uint[] newCells = new uint[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                newCells[i] = (uint) Mathf.Round(cells[i] * max);
            Types.TileMapUint newTileMap = new Types.TileMapUint(tileMap.width, tileMap.height);
            newTileMap.SetCells(newCells);
            return newTileMap;
        }
        public static Types.TileMapBool CastUintToBool(Types.TileMapUint tileMap, uint threshold = 1)
        {
            uint[] cells = tileMap.GetCells();
            int[] newCells = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] < threshold)
                    newCells[i] = 0;
                else
                    newCells[i] = 1;
            }
            Types.TileMapBool newTileMap = new Types.TileMapBool(tileMap.width, tileMap.height);
            newTileMap.SetCells(newCells);
            return newTileMap;
        }
        public static Types.TileMapCont CastUintToCont(Types.TileMapUint tileMap, uint max = 1)
        {
            uint[] cells = tileMap.GetCells();
            float[] newCells = new float[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                newCells[i] = Mathf.Clamp01((float) cells[i] / (float) max);
            Types.TileMapCont newTileMap = new Types.TileMapCont(tileMap.width, tileMap.height);
            newTileMap.SetCells(newCells);
            return newTileMap;
        }
    }
}
