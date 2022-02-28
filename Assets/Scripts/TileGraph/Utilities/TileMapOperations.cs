using System;
using System.Collections.Generic;
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
    }
}
