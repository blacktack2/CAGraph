using UnityEngine;

namespace CAGraph.Utilities
{
    public class MatrixOperations
    {
        public static void RandomizeMatrix(Types.Matrix matrix, float chance, int seed)
        {
            Random.State state = Random.state;
            Random.InitState(seed);

            int[] cells = matrix.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = Random.value < chance ? 1 : 0;
            matrix.SetCells(cells);

            Random.state = state;
        }

        public static void ClearMatrix(Types.Matrix matrix)
        {
            int[] cells = matrix.GetCells();
            for (int c = 0; c < cells.Length; c++)
                cells[c] = 0;
            matrix.SetCells(cells);
        }
    }
}
