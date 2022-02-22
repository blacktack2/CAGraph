using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace CAGraph.Utilities
{
    /// <summary> Utility class for general operations on matrices. </summary>
    public class MatrixOperations
    {
        /// <summary> Populate <paramref name="matrix" /> with randomly
        /// distributed 0-1 values. </summary>
        /// <param name="matrix"> Boolean matrix to randomize. </param>
        /// <param name="chance"> Probability for each value to be a 1.
        /// </param>
        /// <param name="seed"> Seed value to initialize the random number
        /// generator at. </param>
        public static void RandomizeMatrix01(Types.Matrix01 matrix, float chance, int seed)
        {
            Random.State state = Random.state;
            Random.InitState(seed);

            int[] cells = matrix.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = Random.value < chance ? 1 : 0;
            matrix.SetCells(cells);

            Random.state = state;
        }

        /// <summary> Set all values of <paramref name="matrix" /> to
        /// <paramref name="value" />. </summary>
        /// <param name="matrix"> Matrix to fill. </param>
        /// <param name="value"> Numeric value to fill
        /// <paramref name="matrix" /> with (should match the datatype of
        /// <paramref name="matrix" />). </param>
        public static void FillMatrix(Types.Matrix matrix, IConvertible value)
        {
            IConvertible[] cells = matrix.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = value;
            matrix.SetCells(cells);
        }

        /// <summary> Replace all values in <paramref name="matrix" /> matching
        /// a value in <paramref name="toReplace" /> with
        /// <paramref name="replaceWith" /> </summary>
        /// <param name="matrix"> Matrix to operate on. </param>
        /// <param name="toReplace"> List of all values in
        /// <paramref name="matrix" /> which should be replaced with
        /// <paramref name="replaceWith" />. </param>
        /// <param name="replaceWith"> Value to replace all values in
        /// <paramref name="toReplace" /> with. </param>
        public static void ReplaceMatrixValues(Types.Matrix matrix, List<IConvertible> toReplace, IConvertible replaceWith)
        {
            IConvertible[] cells = matrix.GetCells();
            for (int c = 0; c < cells.Length; c++)
                if (toReplace.Contains((IConvertible) cells[c]))
                    cells[c] = replaceWith;
            matrix.SetCells(cells);
        }
    }
}
