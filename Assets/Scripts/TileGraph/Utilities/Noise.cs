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

            public void PerlinNoise2D(Types.TileMapCont tileMap, Vector2? magnitude = null, Vector2? offset = null,
                                      uint octaves = 1, float[] lacunarity = null, float[] persistence = null, bool useGPU = true)
            {
                if (magnitude == null)
                    magnitude = new Vector2(0.1f, 0.1f);
                if (offset == null)
                    offset = Vector2.zero;
                if (lacunarity == null)
                    lacunarity = new float[] {2f, 4f, 8f, 16f, 32f, 64f, 128f, 256f, 512f, 1024f, 2048f, 4096f, 8192f, 16384f, 32768f, 65536f, 131072f, 262144f, 524288f, 1048576f};
                if (persistence == null)
                    persistence = new float[] {0.5f, 0.25f, 0.125f, 0.0625f, 0.03125f, 0.015625f, 0.0078125f, 0.00390625f, 0.001953125f, 0.0009765625f, 0.00048828125f, 0.000244140625f, 0.0001220703125f, 6.103515625e-05f, 3.0517578125e-05f, 1.52587890625e-05f, 7.62939453125e-06f, 3.814697265625e-06f, 1.9073486328125e-06f, 9.5367431640625e-07f};
                if (octaves <= 1)
                {
                    if (useGPU)
                        PerlinNoise2DGPU(tileMap, (Vector2) magnitude, (Vector2) offset);
                    else
                        PerlinNoise2DCPU(tileMap, (Vector2) magnitude, (Vector2) offset);
                }
                else
                {
                    if (useGPU)
                        FractalPerlinNoise2DGPU(tileMap, (Vector2) magnitude, (Vector2) offset, octaves, lacunarity, persistence);
                    else
                        PerlinNoise2DCPU(tileMap, (Vector2) magnitude, (Vector2) offset);
                }
            }
            private void PerlinNoise2DCPU(Types.TileMapCont tileMap, Vector2 magnitude, Vector2 offset)
            {
                
            }
            private void PerlinNoise2DGPU(Types.TileMapCont tileMap, Vector2 magnitude, Vector2 offset)
            {
                const int kernelIndex = (int) FunctionLibrary.FunctionKernels.PerlinNoise2D;

                float[] cells = new float[tileMap.width * tileMap.height];

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetVector(_MagnitudeID, magnitude);
                _FunctionLibrary._ComputeShader.SetVector(_OffsetID, offset);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);

                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, false);
                
                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);

                _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }
            private void FractalPerlinNoise2DGPU(Types.TileMapCont tileMap, Vector2 magnitude, Vector2 offset,
                                                 uint octaves, float[] lacunarity, float[] persistence)
            {
                const int kernelIndex = (int) FunctionLibrary.FunctionKernels.FractalPerlinNoise2D;

                float[] cells = new float[tileMap.width * tileMap.height];

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);
                _FunctionLibrary._LacunarityBuffer.SetData(lacunarity, 0, 0, (int) octaves);
                _FunctionLibrary._PersistenceBuffer.SetData(persistence, 0, 0, (int) octaves);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetInt(_OctavesID, (int) Mathf.Max(2, octaves));
                _FunctionLibrary._ComputeShader.SetVector(_MagnitudeID, magnitude);
                _FunctionLibrary._ComputeShader.SetVector(_OffsetID, offset);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _LacunarityID, _FunctionLibrary._LacunarityBuffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _PersistenceID, _FunctionLibrary._PersistenceBuffer);

                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, false);
                
                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);

                _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }

            public void VoronoiNoise2D(Types.TileMapCont tileMap, Vector2? magnitude, Vector2? offset, bool useGpu = false)
            {
                if (magnitude == null)
                    magnitude = new Vector2(0.1f, 0.1f);
                if (offset == null)
                    offset = Vector2.zero;
                
                if (useGpu)
                    VoronoiNoise2DGPU(tileMap, (Vector2) magnitude, (Vector2) offset);
                else
                    VoronoiNoise2DCPU(tileMap, (Vector2) magnitude, (Vector2) offset);
            }

            private void VoronoiNoise2DCPU(Types.TileMapCont tileMap, Vector2 magnitude, Vector2 offset)
            {
                
            }

            private void VoronoiNoise2DGPU(Types.TileMapCont tileMap, Vector2 magnitude, Vector2 offset)
            {
                const int kernelIndex = (int) FunctionLibrary.FunctionKernels.VoronoiNoise2D;

                float[] cells = new float[tileMap.width * tileMap.height];

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetVector(_MagnitudeID, magnitude);
                _FunctionLibrary._ComputeShader.SetVector(_OffsetID, offset);
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
