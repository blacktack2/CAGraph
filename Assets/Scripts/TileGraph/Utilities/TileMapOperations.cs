using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileGraph.Utilities
{
    /// <summary> Utility class for general operations on matrices. </summary>
    public class TileMapOperations
    {
        /// <summary> Randomly assign either 0 or 1 to all cells in
        /// <paramref name="tileMap" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMapBool" /> to
        /// randomize. </param>
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
        /// <summary> Randomly assign a value between 0-1 (inclusive) to all
        /// cells in <paramref name="tileMap" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMapCont" /> to
        /// randomize. </param>
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
        /// <summary> Randomly assign values between 0-<paramref name="max" />
        /// (inclusive) to all cells in <paramref name="tileMap" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMapUint" /> to
        /// randomize. </param>
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

        /// <summary> Cast a <paramref name="TileMapBool" /> to a
        /// <paramref name="TileMapCont" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMapBool" /> to cast.
        /// </param>
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
        /// <summary> Cast a <paramref name="TileMapBool" /> to a
        /// <paramref name="TileMapUint" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMapBool" /> to cast.
        /// </param>
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
        /// <summary> Cast a <paramref name="TileMapCont" /> to a
        /// <paramref name="TileMapBool" />. Values from
        /// 0-<paramref name="threshold" /> will become 0, and values from
        /// <paramref name="TileMapCont" />-1 will become 1 </summary>
        /// <param name="tileMap"> <paramref name="TileMapCont" /> to cast.
        /// </param>
        /// <param name="threshold"> Threshold value, below which all values
        /// are cast to 0. </param>
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
        /// <summary> Cast a <paramref name="TileMapCont" /> to a
        /// <paramref name="TileMapUint" />. Values will be normalized from 0-1
        /// to 0-<paramref name="max" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMapCont" /> to cast.
        /// </param>
        /// <param name="max"> Maximum value to normalize to. </param>
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
        /// <summary> Cast a <paramref name="TileMapUint" /> to a
        /// <paramref name="TileMapBool" />. Values will be converted to 0 and
        /// 1 based on <paramref name="threshold" />. </summary>
        /// <param name="tileMap"> <paramref name="TileMapUint" /> to cast.
        /// </param>
        /// <param name="threshold"> Lowest value in
        /// <paramref name="tileMap" /> to be considered a 1. All other values
        /// will become 0. </param>
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
        /// <summary> Cast a <paramref name="TileMapUint" /> to a
        /// <paramref name="TileMapCont" />. Values will be normalized from
        /// 0-<paramref name="max" /> to 0-1. </summary>
        /// <param name="tileMap"> <paramref name="TileMapUint" /> to cast.
        /// </param>
        /// <param name="max"> Maximum uint value to expect from
        /// <paramref name="tileMap" />. Values greater than
        /// <paramref name="max" /> will be clamped at 1. </param>
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
