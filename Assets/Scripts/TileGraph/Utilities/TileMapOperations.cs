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
        }
    }
}
