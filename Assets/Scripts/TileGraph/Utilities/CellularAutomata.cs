using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class CellularAutomata : SubLibrary
        {
            public CellularAutomata(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }

            /// <summary> Run a lifelike CA simulation on
            /// <paramref name="tileMap" /> for a given set of rules and
            /// number of itetrations. </summary>
            /// <param name="tileMap"> <paramref name="TileMapBool" /> to run the simulation on.
            /// </param>
            /// <param name="rules"> Array of 18 0-1 integers representing the
            /// lifelike CA rules. Indexes 0-8 represent the birth rules for a
            /// number of neighbours corresponding to each index. Likewise with
            /// indexes 9-17 representing the survival rules. 0: will die/stay
            /// dead, 1: will survive/be born </param>
            /// <param name="iterations"> Number of iterations to run the
            /// simulation for. </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public void LifeLikeCA(Types.TileMapBool tileMap, int[] rules, int iterations, bool useGPU = true)
            {
                if (useGPU)
                    LifeLikeCAGPU(tileMap, rules, iterations);
                else
                    LifeLikeCACPU(tileMap, rules, iterations);
            }

            private void LifeLikeCACPU(Types.TileMapBool tileMap, int[] rules, int iterations)
            {
                bool bufferFlag = false;
                int[] cells0 = tileMap.GetCells();
                int[] cells1 = new int[cells0.Length];

                for (int i = 0; i < iterations; i++)
                {
                    bufferFlag = !bufferFlag;
                    // Alternate read/write operations between cells0 and cells1
                    if (bufferFlag)
                    {
                        for (int c = 0, x = 0, y = 0; c < cells0.Length; c++, x++)
                        {
                            if (x >= tileMap.width)
                            {
                                x = 0;
                                y++;
                            }
                            int neighbours = 0;
                            if (y > 0)
                            {
                                if (x > 0)
                                    neighbours += cells0[c - 1 - tileMap.width];
                                neighbours += cells0[c - tileMap.width];
                                if (x < tileMap.width - 1)
                                    neighbours += cells0[c + 1 - tileMap.width];
                            }
                            if (x > 0)
                                neighbours += cells0[c - 1];
                            if (x < tileMap.width - 1)
                                neighbours += cells0[c + 1];
                            if (y < tileMap.height - 1)
                            {
                                if (x > 0)
                                    neighbours += cells0[c - 1 + tileMap.width];
                                neighbours += cells0[c + tileMap.width];
                                if (x < tileMap.width - 1)
                                    neighbours += cells0[c + 1 + tileMap.width];
                            }
                            cells1[c] = rules[cells0[c] * 9 + neighbours];
                        }
                    }
                    else
                    {
                        for (int c = 0, x = 0, y = 0; c < cells1.Length; c++, x++)
                        {
                            if (x >= tileMap.width)
                            {
                                x = 0;
                                y++;
                            }
                            int neighbours = 0;
                            if (y > 0)
                            {
                                if (x > 0)
                                    neighbours += cells1[c - 1 - tileMap.width];
                                neighbours += cells1[c - tileMap.width];
                                if (x < tileMap.width - 1)
                                    neighbours += cells1[c + 1 - tileMap.width];
                            }
                            if (x > 0)
                                neighbours += cells1[c - 1];
                            if (x < tileMap.width - 1)
                                neighbours += cells1[c + 1];
                            if (y < tileMap.height - 1)
                            {
                                if (x > 0)
                                    neighbours += cells1[c - 1 + tileMap.width];
                                neighbours += cells1[c + tileMap.width];
                                if (x < tileMap.width - 1)
                                    neighbours += cells1[c + 1 + tileMap.width];
                            }
                            cells0[c] = rules[cells1[c] * 9 + neighbours];
                        }
                    }
                }
                if (bufferFlag)
                    tileMap.SetCells(cells0);
                else
                    tileMap.SetCells(cells1);
            }
            private void LifeLikeCAGPU(Types.TileMapBool tileMap, int[] rules, int iterations)
            {
                int kernelIndex = (int) FunctionLibrary.FunctionKernels.LifeLikeCA;
                bool bufferFlag = false;

                int[] cells = tileMap.GetCells();

                _FunctionLibrary._Cells0Buffer.SetData(cells);
                _FunctionLibrary._Cells1Buffer.SetData(cells);
                _FunctionLibrary._LifeRulesBuffer.SetData(rules);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _Cells0ID, _FunctionLibrary._Cells0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _Cells1ID, _FunctionLibrary._Cells1Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _LifeRulesID, _FunctionLibrary._LifeRulesBuffer);

                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                for (int i = 0; i < iterations; i++)
                {
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);

                    _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);
                }

                if (bufferFlag)
                    _FunctionLibrary._Cells0Buffer.GetData(cells);
                else
                    _FunctionLibrary._Cells1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }
        }
    }
}
