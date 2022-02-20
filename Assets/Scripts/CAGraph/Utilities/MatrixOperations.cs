using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CAGraph.Utilities
{
    public class MatrixOperations
    {
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

        public static void FillMatrix(Types.Matrix matrix, IConvertible value)
        {
            IConvertible[] cells = matrix.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = value;
            matrix.SetCells(cells);
        }

        public static void ReplaceMatrixValues<MType, DType>(Types.Matrix matrix, List<DType> toReplace, DType replaceWith)
            where MType : Types.Matrix where DType : IConvertible
        {
            IConvertible[] cells = matrix.GetCells();
            for (int c = 0; c < cells.Length; c++)
                if (toReplace.Contains((DType) cells[c]))
                    cells[c] = replaceWith;
            matrix.SetCells(cells);
        }
    }
}
