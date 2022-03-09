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
            /// indexes 9-17 representing the survival rules. 0 will die/stay dead,
            /// 1 will survive/be born </param>
            /// <param name="iterations"> Number of iterations to run the simulation for. </param>
            public void LifeLikeCA(Types.TileMapBool tileMap, int[] rules, int iterations)
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
