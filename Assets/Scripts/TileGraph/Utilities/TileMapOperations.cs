using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class TileMapOperations : SubLibrary
        {
            public TileMapOperations(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }
            /// <summary> Randomly assign either 0 or 1 to all cells in
            /// <paramref name="tileMap" />. </summary>
            /// <param name="tileMap"> <paramref name="TileMapBool" /> to
            /// randomize. </param>
            /// <param name="chance"> Probability for each value to be a 1.
            /// </param>
            /// <param name="seed"> Seed value to initialize the random number
            /// generator at. </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public void RandomizeTileMapBool(Types.TileMapBool tileMap, float chance, int seed, bool useGPU = true)
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
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public void RandomizeTileMapCont(Types.TileMapCont tileMap, int seed, bool useGPU = true)
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
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public void RandomizeTileMapUint(Types.TileMapUint tileMap, uint max, int seed, bool useGPU = true)
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
            /// <param name="tileMap"> <paramref name="TileMap" /> to fill.
            /// </param>
            /// <param name="value"> Numeric value to fill
            /// <paramref name="tileMap" /> with (should match the datatype of
            /// <paramref name="tileMap" />). </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public void FillTileMap(Types.TileMap tileMap, IConvertible value, bool useGPU = true)
            {
                IConvertible[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] = value;
                tileMap.SetCells(cells);
            }

            /// <summary> Replace all values in <paramref name="tileMap" />
            /// matching a value in <paramref name="toReplace" /> with
            /// <paramref name="replaceWith" /> </summary>
            /// <param name="tileMap"> <paramref name="TileMap" /> to operate
            /// on. </param>
            /// <param name="toReplace"> List of all values in
            /// <paramref name="tileMap" /> which should be replaced with
            /// <paramref name="replaceWith" />. </param>
            /// <param name="replaceWith"> Value to replace all values in
            /// <paramref name="toReplace" /> with. </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public void ReplaceTileMapValues(Types.TileMap tileMap, List<IConvertible> toReplace, IConvertible replaceWith, bool useGPU = true)
            {
                IConvertible[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    if (toReplace.Contains((IConvertible) cells[c]))
                        cells[c] = replaceWith;
                tileMap.SetCells(cells);
            }

            /// <summary> Clamp all values in <paramref name="tileMap" />
            /// to between <paramref name="min" /> and
            /// <paramref name="max" />. </summary>
            /// <param name="tileMap"> <paramref name="TileMapCont" /> to
            /// operate on. </param>
            /// <param name="min"> Minimum value to clamp to (inclusive)
            /// </param>
            /// <param name="max"> Maximum value to clamp to (inclusive)
            /// </param>
            public void ClampTileMap(Types.TileMapCont tileMap, float min, float max)
            {
                float[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] = Mathf.Clamp(cells[c], min, max);
                tileMap.SetCells(cells);
            }
            /// <summary> Clamp all values in <paramref name="tileMap" />
            /// to between <paramref name="min" /> and
            /// <paramref name="max" />. </summary>
            /// <param name="tileMap"> <paramref name="TileMapUint" /> to
            /// operate on. </param>
            /// <param name="min"> Minimum value to clamp to (inclusive)
            /// </param>
            /// <param name="max"> Maximum value to clamp to (inclusive)
            /// </param>
            public void ClampTileMap(Types.TileMapUint tileMap, uint min, uint max)
            {
                uint[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] = (uint) Mathf.Clamp(cells[c], min, max);
                tileMap.SetCells(cells);
            }

            public void TileMapAdd(Types.TileMapCont tileMap, float value)
            {
                float[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] += value;
                tileMap.SetCells(cells);
            }
            public void TileMapAdd(Types.TileMapUint tileMap, uint value)
            {
                uint[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] += value;
                tileMap.SetCells(cells);
            }
            public void TileMapAdd(ref Types.TileMapCont tileMapA, Types.TileMapCont tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                float[] cells = new float[w * h];
                float[] cellsA = tileMapA.GetCells();
                float[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] + cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapCont(w, h);
                tileMapA.SetCells(cells);
            }
            public void TileMapAdd(ref Types.TileMapUint tileMapA, Types.TileMapUint tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                uint[] cells = new uint[w * h];
                uint[] cellsA = tileMapA.GetCells();
                uint[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] + cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapUint(w, h);
                tileMapA.SetCells(cells);
            }
            public void TileMapSubtract(Types.TileMapCont tileMap, float value)
            {
                float[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] -= value;
                tileMap.SetCells(cells);
            }
            public void TileMapSubtract(Types.TileMapUint tileMap, uint value)
            {
                uint[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] -= value;
                tileMap.SetCells(cells);
            }
            public void TileMapSubtract(ref Types.TileMapCont tileMapA, Types.TileMapCont tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                float[] cells = new float[w * h];
                float[] cellsA = tileMapA.GetCells();
                float[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] - cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapCont(w, h);
                tileMapA.SetCells(cells);
            }
            public void TileMapSubtract(ref Types.TileMapUint tileMapA, Types.TileMapUint tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                uint[] cells = new uint[w * h];
                uint[] cellsA = tileMapA.GetCells();
                uint[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] - cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapUint(w, h);
                tileMapA.SetCells(cells);
            }
            public void TileMapMultiply(Types.TileMapCont tileMap, float value)
            {
                float[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] *= value;
                tileMap.SetCells(cells);
            }
            public void TileMapMultiply(Types.TileMapUint tileMap, uint value)
            {
                uint[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] *= value;
                tileMap.SetCells(cells);
            }
            public void TileMapMultiply(ref Types.TileMapCont tileMapA, Types.TileMapCont tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                float[] cells = new float[w * h];
                float[] cellsA = tileMapA.GetCells();
                float[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] * cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapCont(w, h);
                tileMapA.SetCells(cells);
            }
            public void TileMapMultiply(ref Types.TileMapUint tileMapA, Types.TileMapUint tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                uint[] cells = new uint[w * h];
                uint[] cellsA = tileMapA.GetCells();
                uint[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] * cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapUint(w, h);
                tileMapA.SetCells(cells);
            }
            public void TileMapDivide(Types.TileMapCont tileMap, float value)
            {
                float[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] *= value;
                tileMap.SetCells(cells);
            }
            public void TileMapDivide(Types.TileMapUint tileMap, uint value)
            {
                uint[] cells = tileMap.GetCells();
                for (int c = 0; c < cells.Length; c++)
                    cells[c] *= value;
                tileMap.SetCells(cells);
            }
            public void TileMapDivide(ref Types.TileMapCont tileMapA, Types.TileMapCont tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                float[] cells = new float[w * h];
                float[] cellsA = tileMapA.GetCells();
                float[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] / cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapCont(w, h);
                tileMapA.SetCells(cells);
            }
            public void TileMapDivide(ref Types.TileMapUint tileMapA, Types.TileMapUint tileMapB, int offsetX = 0, int offsetY = 0)
            {
                int w = Mathf.Max(0, Mathf.Min(tileMapA.width,  tileMapB.width)  - Mathf.Max(0, offsetX));
                int h = Mathf.Max(0, Mathf.Min(tileMapA.height, tileMapB.height) - Mathf.Max(0, offsetY));
                uint[] cells = new uint[w * h];
                uint[] cellsA = tileMapA.GetCells();
                uint[] cellsB = tileMapB.GetCells();
                int counter = 0;
                int startX = Mathf.Max(0, offsetX);
                int startY = Mathf.Max(0, offsetY);
                for (int x = startX; x < startX + w; x++)
                    for (int y = startY; y < startY + h; y++)
                    {
                        cells[counter++] = cellsA[x + y * tileMapA.width] / cellsB[x + y * tileMapB.width];
                        Debug.Log(x + " " + y);
                    }
                tileMapA = new Types.TileMapUint(w, h);
                tileMapA.SetCells(cells);
            }
        }
    }
}
