using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class Noise : SubLibrary
        {
            public Noise(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }

            public void PerlinNoise2D(Types.TileMapCont tileMap, bool useGPU = true)
            {
                if (useGPU)
                    PerlinNoise2DGPU(tileMap);
                else
                    PerlinNoise2DCPU(tileMap);
            }
            private void PerlinNoise2DCPU(Types.TileMapCont tileMap)
            {
                
            }
            private void PerlinNoise2DGPU(Types.TileMapCont tileMap)
            {
                const int kernelIndex = (int) FunctionLibrary.FunctionKernels.PerlinNoise2D;

                float[] cells = new float[tileMap.width * tileMap.height];

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetVector(_MagnitudeID, new Vector3(0.1f, 0.1f, 0.1f));
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);

                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, false);
                
                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);

                _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }
        }
    }
}
